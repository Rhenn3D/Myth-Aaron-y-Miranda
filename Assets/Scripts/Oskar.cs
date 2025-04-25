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
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        groundSensor = GetComponentInChildren<GroundSensor>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
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
        if(Input.GetButtonDown("Jump") && groundSensor.isGrounded == true)
        {
            _rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsJumping", true);
        }
        _animator.SetBool("IsJumping", !groundSensor.isGrounded);
        
        
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerSpeed, _rigidBody.velocity.y);

    }

    


}

