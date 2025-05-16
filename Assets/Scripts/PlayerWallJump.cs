using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour

{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;

    [Header("Detección de suelo y pared")]
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    [Header("Wall Sliding")]
    public float wallSlideSpeed = 2f;
    private bool isWallSliding;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingWall;
    private int wallDirX;

    private float horizontalInput;

    private bool canWallJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Verificamos si está tocando el suelo o una pared
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        // Movimiento horizontal
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Wall Slide
        if (isTouchingWall && !isGrounded && horizontalInput != 0)
        {
            isWallSliding = true;
            canWallJump = true; // Se habilita el wall jump al tocar la pared
            wallDirX = (int)transform.localScale.x;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }

        // Saltar
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (isWallSliding && canWallJump)
            {
                rb.velocity = new Vector2(-wallDirX * wallJumpForceX, wallJumpForceY);
                canWallJump = false; // Solo permite un salto hasta volver a tocar la pared
                Flip();
            }
            
        }
    }

    // Voltea la dirección del personaje
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }
}
