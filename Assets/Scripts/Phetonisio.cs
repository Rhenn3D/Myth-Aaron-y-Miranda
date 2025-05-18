using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phetonisio : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    private bool movingRight = true;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckDistance = 1f;

    [Header("Detección de pared")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;

    [Header("Capas")]
    public LayerMask groundLayer;

    [Header("Daño al jugador")]
    public float damage = 20f;
    public float damageCooldown = 1f;
    private float lastHitTime;

    [Header("Salud del enemigo")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    [Header("Animaciones")]
    public Animator animator;


    void Update()
    {
        if (isDead) return;
        // Movimiento
        transform.Translate(Vector2.right * speed * Time.deltaTime * (movingRight ? 1 : -1));

        // Comprobar suelo
        bool noGround = !Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        // Comprobar pared
        Vector2 wallDirection = movingRight ? Vector2.right : Vector2.left;
        bool wallAhead = Physics2D.Raycast(wallCheck.position, wallDirection, wallCheckDistance, groundLayer);

        if (noGround || wallAhead)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OskarController player = other.GetComponent<OskarController>();
            if (player != null)
            {
                player.TakeDamage(1, transform);
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) TryDamagePlayer(other);
    }

    void TryDamagePlayer(Collider2D other)
    {
        if (isDead) return;  // No hace daño si está muriendo

        if (Time.time >= lastHitTime + damageCooldown)
        {
            HealthBar healthBar = FindObjectOfType<HealthBar>();
            if (healthBar != null)
            {
                healthBar.TakeDamage(damage);
                lastHitTime = Time.time;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // Si ya está muerto, no sigue restando salud ni nada

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("isDeath");
        // Destruir después de 1 segundo (o el tiempo que dure la animación)
        StartCoroutine(DestroyAfterDelay(1f));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 direction = movingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + direction * wallCheckDistance);
        }
    }
    IEnumerator WaitAndDie()
    {
        // Espera a que termine la animación de muerte (ajusta el tiempo si es necesario)
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

}
