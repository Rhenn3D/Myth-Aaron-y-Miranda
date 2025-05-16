using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Oskar : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rigidBody; 
    private BoxCollider2D boxcollider2D;
    public float playerSpeed = 5f;
    public float jumpForce = 8f;

    public GroundSensor groundSensor;
    public float inputHorizontal;
    private Animator _animator;

    private AudioSource jumpSound;
    public AudioClip jumpSFX;
    [SerializeField] private AudioSource runAudioSource;
  public AudioClip runSFX;
  private bool _alredyPlaying = false;
  private ParticleSystem particleSystemm;
  private Transform particlesTransform;
  private Vector3 particlesPosition;
private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private float attackDamage = 5f;
    private float attackRadius = 0.5f;
    public Transform hitBoxPosition;
    public LayerMask enemyLayer;
    public AudioClip attackSFX;
    private AudioSource _SFXSource;
    public float dashForce = 20;
    public float dashDuration = 0.5f;
    public float dashCoolDown;
    private bool canDash = true;
    private bool isDashing = false; 



    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;

    public Transform wallCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    [Header("Wall Sliding")]
    public float wallSlideSpeed = 2f;
    private bool isWallSliding;

    private bool isTouchingWall;
    private bool canWallJump;
    

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        boxcollider2D = GetComponent<BoxCollider2D>();
        groundSensor = GetComponentInChildren<GroundSensor>();
        _animator = GetComponent<Animator>();
        jumpSound = GetComponent<AudioSource>();
        jumpSound.clip = jumpSFX;
        particleSystemm = GetComponentInChildren<ParticleSystem>();
        particlesTransform = particleSystemm.transform;
        particlesPosition = particlesTransform.localPosition;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _SFXSource = GetComponent<AudioSource>();
        

    }
    void Start()
    {
        runAudioSource.loop = true;
        runAudioSource.clip = runSFX;
    }

    void Update()
    {
        if(!gameManager.IsPlaying)
        {
            return;
        }
        if(gameManager.isPaused)
        {
            return;
        }
       
        if (isDashing)
        {
            return;
        }

        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        if (isTouchingWall && !groundSensor.isGrounded && inputHorizontal != 0)
        {
            isWallSliding = true;
            canWallJump = true; // Se habilita el wall jump al tocar la pared
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, -wallSlideSpeed);
        }

        if(Input.GetButtonDown("Jump"))
        {
            if(groundSensor.isGrounded || groundSensor.canDobleJump)
            {
               Jump(); 
            }
            else if (isWallSliding && canWallJump)
            {
                _rigidBody.AddForce(Vector2.up * wallJumpForceY, ForceMode2D.Impulse);
                canWallJump = false; // Solo permite un salto hasta volver a tocar la pared
                //Flip();
            }
        }
        PlayerStepsSounds();

        _animator.SetBool("IsJumping", !groundSensor.isGrounded);
        
        if(Input.GetButtonDown("Fire1"))
        {
            NormalAttack();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        
       
    }

    void FixedUpdate()
    {
        

        if(isDashing)
        {
            return;
        }
        
        Movement();
    }

    void Movement()
    {
        _rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerSpeed, _rigidBody.velocity.y);

        if(inputHorizontal > 0)
            {
	        transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
            }
        else if(inputHorizontal < 0)
            {
	        transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
            }
            else
            _animator.SetBool("IsRunning", false);
            


        inputHorizontal = Input.GetAxisRaw("Horizontal");
    }
    void Jump()
    {
        if(!groundSensor.isGrounded)
        {
            groundSensor.canDobleJump = false;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);
        }
        
        _rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpSound.PlayOneShot(jumpSFX);
    }

    void PlayerStepsSounds()
    {
    if(groundSensor.isGrounded && Input.GetAxisRaw("Horizontal") != 0 && !_alredyPlaying)
        {
        
        runAudioSource.Play();
        particleSystemm.Play();
        particlesTransform.SetParent(gameObject.transform);
        particlesTransform.localPosition = particlesPosition;
        particlesTransform.rotation = transform.rotation;
        _alredyPlaying = true;
        }
        else if(!groundSensor.isGrounded || Input.GetAxisRaw("Horizontal") == 0)
        {
        
         runAudioSource.Stop();
         particleSystemm.Stop();
         particlesTransform.SetParent(null);
        _alredyPlaying = false;
        }
    }

    
    public void Death()
    {
        spriteRenderer.enabled = false;
        gameManager.IsPlaying = false;
        boxcollider2D.enabled = false;
        _rigidBody.velocity = Vector2.zero;
        inputHorizontal = 0;
        SceneManager.LoadScene(1);     
    }
    
    void NormalAttack()
    {
        Debug.Log("Attack");
        _animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitBoxPosition.position, attackRadius, enemyLayer);
        _SFXSource.PlayOneShot(attackSFX);
        
       

        foreach(Collider2D enemy in hitEnemies)
        {
            Phetonisio enemyScript = enemy.GetComponent<Phetonisio>();
            enemyScript.TakeDamage(attackDamage);
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitBoxPosition.position, attackRadius);

        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }

    IEnumerator Dash()
    {
        float gravity = _rigidBody.gravityScale;
        _rigidBody.gravityScale = 0;
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);

        isDashing = true;
        canDash = false;
        _rigidBody.AddForce(transform.right * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);
        _rigidBody.gravityScale = gravity;
        isDashing = false;
        
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    

}


