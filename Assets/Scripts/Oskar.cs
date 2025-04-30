using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oskar : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rigidBody; 

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
  private ParticleSystem particleSystem;
  private Transform particlesTransform;
  private Vector3 particlesPosition;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        groundSensor = GetComponentInChildren<GroundSensor>();
        _animator = GetComponent<Animator>();
        jumpSound = GetComponent<AudioSource>();
        jumpSound.clip = jumpSFX;
        particleSystem = GetComponentInChildren<ParticleSystem>();
        particlesTransform = particleSystem.transform;
        particlesPosition = particlesTransform.localPosition;
        

    }
    void Start()
    {
        runAudioSource.loop = true;
        runAudioSource.clip = runSFX;
    }

    void Update()
    {
        Jump();
        PlayerStepsSounds();
    }

    void FixedUpdate()
    {
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
        if(Input.GetButtonDown("Jump") && groundSensor.isGrounded == true)
        {
            _rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsJumping", true);
            jumpSound.PlayOneShot(jumpSFX);
        }
        _animator.SetBool("IsJumping", !groundSensor.isGrounded);
        }

    void PlayerStepsSounds()
    {
    if(groundSensor.isGrounded && Input.GetAxisRaw("Horizontal") != 0 && !_alredyPlaying)
        {
        
        runAudioSource.Play();
        particleSystem.Play();
        particlesTransform.SetParent(gameObject.transform);
        particlesTransform.localPosition = particlesPosition;
        particlesTransform.rotation = transform.rotation;
        _alredyPlaying = true;
        }
        else if(!groundSensor.isGrounded || Input.GetAxisRaw("Horizontal") == 0)
        {
        
         runAudioSource.Stop();
         particleSystem.Stop();
         particlesTransform.SetParent(null);
        _alredyPlaying = false;
        }
    }

}


