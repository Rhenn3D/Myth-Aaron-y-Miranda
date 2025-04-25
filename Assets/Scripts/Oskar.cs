using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oskar : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rigidBody; 

    public float playerSpeed = 5f;
    public float jumpForce = 8f;

    public GroundSensor groundSensor;
    private float inputHorizontal;
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        groundSensor = GetComponentInChildren<GroundSensor>();
    }

    void Update()
    {
        if(inputHorizontal > 0)
            {
	        transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        else if(inputHorizontal < 0)
            {
	        transform.rotation = Quaternion.Euler(0, 180, 0);
            }

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump") && groundSensor.isGrounded)
        {
            _rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
        
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * playerSpeed, _rigidBody.velocity.y);

    }
   
    


}

