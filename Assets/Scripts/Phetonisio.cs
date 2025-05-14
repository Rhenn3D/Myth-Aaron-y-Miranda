using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phetonisio : MonoBehaviour
{

    private Animator animator;
    private AudioSource audioSource;
    private Slider healthBar;
    public AudioClip phetonisiodeathSFX;
    private Rigidbody2D rigidBody;
    public int direction = -1;
    public float speed = 5;
    public float maxHealth = 5;
    private float currentHealth;
    private GameManager gameManager;

    private BoxCollider2D boxCollider;
    public float inputHorizontal;
    public SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        healthBar = GetComponentInChildren<Slider>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    void Start()
    {
        speed = 2;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(direction * speed, rigidBody.velocity.y);
    }


 
    public void Death()
    {
        
        audioSource.PlayOneShot(phetonisiodeathSFX);
        direction = 0;
        rigidBody.gravityScale = 0;
        animator.SetTrigger("IsDead");
        boxCollider.enabled = false;
        Destroy(gameObject, phetonisiodeathSFX.length);
       
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth-= damage;
        healthBar.value = currentHealth;
        if(currentHealth <= 0)
        {
            Death();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            direction *= -1;
        }
        
        if(collision.gameObject.CompareTag("Player"))
        {
            //Destroy(collision.gameObject);
            Oskar playerScript = collision.gameObject.GetComponent<Oskar>();
           
        }
        
    }

void Movement()
    {
        rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rigidBody.velocity.y);

        if(inputHorizontal > 0)
            {
	        transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        else if(inputHorizontal < 0)
            {
	        transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            


        inputHorizontal = Input.GetAxisRaw("Horizontal");
    }
}

