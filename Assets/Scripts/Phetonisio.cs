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
    public float damage = 5f;
    public float damageCooldown = 1f;
    private Dictionary<OskarController, float> lastHitTime = new Dictionary<OskarController, float>();

    [Header("Salud del enemigo")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip hitSound;

    [Header("Animaciones")]
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogWarning("No AudioSource found on " + gameObject.name);
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        transform.Translate(Vector2.right * speed * Time.deltaTime * (movingRight ? 1 : -1));

        bool noGround = !Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            OskarController player = other.GetComponent<OskarController>();
            if (player != null)
            {
                if (!lastHitTime.ContainsKey(player)) lastHitTime[player] = -Mathf.Infinity;

                if (Time.time >= lastHitTime[player] + damageCooldown && !player.IsInvulnerable)
                {
                    player.TakeDamage((int)damage, transform);
                    lastHitTime[player] = Time.time;
                    PlayHitSound();
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            isDead = true;
            audioSource.PlayOneShot(deathSound);
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("isDeath");
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

    public void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}