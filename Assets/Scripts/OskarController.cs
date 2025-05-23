using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OskarController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;




    [Header("Salto Variable")]
    public float jumpTime = 0.25f;
    public float jumpForceMultiplier = 1f;
    private float jumpTimeCounter;
    private bool isJumping;

    [Header("Doble Salto")]
    public int maxJumps = 2;
    private int jumpCount;

    [Header("Wall isJumping")]
    public float wallJumpForce = 14f;
    public Vector2 wallJumpDirection = new Vector2(1, 1);
    public float wallJumpDuration = 0.2f;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 2f;
    public Vector2 wallCheckOffset = new Vector2(0.5f, 0f);
    public float wallCheckRadius = 0.2f;
    public LayerMask wallLayer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private float lastDashTime = -Mathf.Infinity;
    public ParticleSystem dashParticles;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isDashing;

    private float wallJumpTime;
    private float moveInput;
    private int facingDirection = 1;
    private Animator animator;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    private bool isFacingRight = true;


    [Header("Daño e Invulnerabilidad")]
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public AudioClip hurtSound;
    public float invulnerabilidadDuracion = 1f;
    private bool isKnockedBack = false;
    public bool IsInvulnerable = false;

    public Phetonisio enemyScript;

    [Header("Vida")]
    public int maxHealth = 50;
    private int currentHealth;
    private Slider barraVida;
    public HealthBar healthBar;
    public bool isDead = false;


    [Header("Patatas")]
    public int cantidadPatatas = 0; // Cantidad actual de patatas
    public GameObject prefabPatata; // Prefab de la patata para lanzar
    public Transform puntoLanzamiento; // Punto desde donde se lanza la patata
    public float fuerzaLanzamiento = 10f;
    public Text patatasCountText;



    [Header("Sonidos")]
    private AudioSource audioSource;
    public AudioSource footstepSource;
    public AudioClip footstepClip;
    private bool footstepClipPlaying = false;
    public AudioClip jumpSound;

    public AudioClip dashSound;

    public AudioClip attackSound;
    public AudioClip deathSound;






    void Awake()
    {
        Time.timeScale = 1f;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallJumpDirection.Normalize();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        UpdatePatatasUI();

        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);

        }

    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip del personaje y attackPoint
        if (moveInput != 0 && !isWallJumping)
        {
            if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
            {
                Flip();
            }
        }

        // Sonido pasos
        if (isGrounded && moveInput != 0 && !footstepClipPlaying)
        {
            footstepClipPlaying = true;
            footstepSource.clip = footstepClip;
            footstepSource.Play();
        }
        else if ((!isGrounded || moveInput == 0) && footstepClipPlaying)
        {
            footstepClipPlaying = false;
            footstepSource.Stop();
        }

        // Ataque
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0)) // Click izquierdo
            {
                AttackAnimation();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        if (Input.GetMouseButtonDown(1)) // Botón derecho del ratón
        {
            LanzarPatata();
        }


        // Chequeos de suelo y pared
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Vector2 wallCheckPos = (Vector2)transform.position + new Vector2(facingDirection * wallCheckOffset.x, wallCheckOffset.y);
        isTouchingWall = Physics2D.OverlapCircle(wallCheckPos, wallCheckRadius, wallLayer);

        if (isGrounded)
        {
            jumpCount = maxJumps;
        }

        isWallSliding = isTouchingWall && !isGrounded && moveInput != 0 && !isWallJumping && !isDashing;

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }

        // Saltos
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("isJumping", true);
            if (isGrounded || (jumpCount > 0 && !isWallSliding))
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                audioSource.PlayOneShot(jumpSound);

                rb.velocity = new Vector2(rb.velocity.x, 0f); // Reinicia velocidad en Y
                rb.velocity += Vector2.up * jumpForce;

                jumpCount--;
            }
            else if (isWallSliding)
            {
                isWallJumping = true;
                wallJumpTime = Time.time + wallJumpDuration;

                rb.velocity = new Vector2(-facingDirection * wallJumpForce * wallJumpDirection.x,
                                           wallJumpForce * wallJumpDirection.y);

                // Ya no uses spriteRenderer.flipX aquí
                // spriteRenderer.flipX = facingDirection > 0;
                jumpCount = maxJumps;
            }
        }

        if (Input.GetButton("Jump") && isJumping && !isDashing)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpForceMultiplier);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            animator.SetTrigger("isJumping");    // Al iniciar salto
        }

        if (Time.time > wallJumpTime)
        {
            isWallJumping = false;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(PerformDash());
            audioSource.PlayOneShot(dashSound);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Animaciones booleanas
        animator.SetBool("isJumping", !isGrounded && !isWallSliding);
        animator.SetBool("isWallSliding", isWallSliding);
        animator.SetBool("Dashing", isDashing);
    }


    void FixedUpdate()
    {
        if (isKnockedBack) return;

        if (!isWallJumping && !isWallSliding && !isDashing)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        animator.SetTrigger("Dashing");

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Partículas
        if (dashParticles != null)
        {
            float offsetX = -facingDirection * 0.5f;
            Vector3 particlePos = transform.position + new Vector3(offsetX, 0f, 0f);
            dashParticles.transform.position = particlePos;

            float rotationZ = facingDirection == 1 ? 180f : 0f;
            dashParticles.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

            dashParticles.Play();
        }

        rb.velocity = new Vector2(facingDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Vector2 gizmoPos = Vector2.zero;

        // Usa la posición mundial del attackPoint para el gizmo
        if (attackPoint != null)
        {
            gizmoPos = attackPoint.position;
        }
        else
        {
            // Como fallback, usa la posición del personaje + offset
            gizmoPos = (Vector2)transform.position + new Vector2(facingDirection * wallCheckOffset.x, wallCheckOffset.y);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoPos, attackRange);
    }
    void Attack()
    {


        // Detectar enemigos dentro del rango de ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Phetonisio enemyScript = enemy.GetComponent<Phetonisio>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }
    void AttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            audioSource.PlayOneShot(attackSound);
            Attack();
        }
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        facingDirection = isFacingRight ? 1 : -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Opcional: si attackPoint no es hijo o no se mueve, ajustar su posición aquí
    }

    public void TakeDamage(int amount, Transform enemyTransform)
    {
        if (IsInvulnerable) return;

        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);

        }

        audioSource.PlayOneShot(hurtSound);

        Vector2 knockbackDir = (transform.position - enemyTransform.position).normalized;
        StartCoroutine(ApplyKnockback(knockbackDir));
        StartCoroutine(Invulnerabilidad());

        if (currentHealth <= 0)
        {
            Die();
        }
    }



    IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            rb.velocity = new Vector2(direction.x * knockbackForce, direction.y * knockbackForce * 0.5f);
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockedBack = false;
    }

    IEnumerator Invulnerabilidad()
    {
        IsInvulnerable = true;
        float tiempo = 0f;
        float intervalo = 0.1f;

        while (tiempo < invulnerabilidadDuracion)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(intervalo);
            tiempo += intervalo;
        }

        spriteRenderer.enabled = true; // Asegura que quede visible
        IsInvulnerable = false;
    }

    public void recogerPatata()
    {
        cantidadPatatas++;
        ActualizarUI();
    }
    public void LanzarPatata()
    {
        if (cantidadPatatas > 0)
        {
            GameObject patata = Instantiate(prefabPatata, puntoLanzamiento.position, Quaternion.identity);
            Rigidbody2D rbPatata = patata.GetComponent<Rigidbody2D>();
            rbPatata.velocity = new Vector2(fuerzaLanzamiento * facingDirection, 0);
            cantidadPatatas--;
            ActualizarUI();
        }
    }
    void UpdatePatatasUI()
    {
        if (patatasCountText != null)
            patatasCountText.text = "Patatas: " + cantidadPatatas.ToString();
    }
    void ActualizarUI()
    {
        if (patatasCountText != null)
        {
            patatasCountText.text = "x " + cantidadPatatas.ToString();
        }

        if (barraVida != null)
        {
            barraVida.value = (float)currentHealth / maxHealth;
        }
    }
    public void Die()
    {
        animator.SetTrigger("isDeath");
        audioSource.PlayOneShot(deathSound);
        Time.timeScale = 1f;
        Invoke("LoadGameOverScene", 2f);

        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Death");
        audioSource.PlayOneShot(deathSound);

        StartCoroutine(LoadDeathSceneAfterDelay(1f));
    }
    void LoadGameOverScene()
    {
        SceneManager.LoadScene(2);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Die(); // Llama a tu método de muerte
        }
    }
    
    IEnumerator LoadDeathSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // NO se ve afectado por Time.timeScale
        SceneManager.LoadScene("GameOver"); // Cambia "GameOver" por el nombre de tu escena de muerte
    }
    
    
    
}