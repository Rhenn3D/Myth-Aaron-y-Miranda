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

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
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
        if (other.CompareTag("Player")) TryDamagePlayer(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) TryDamagePlayer(other);
    }

    void TryDamagePlayer(Collider2D other)
    {
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
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Aquí puedes poner animación de muerte, partículas, etc.
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
}
