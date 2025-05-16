using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [Header("Static Ground")]
  private Rigidbody2D _rigidBody;
 
  [Header("Oskar Jump")]
  public bool isGrounded;
  public float jumpForce = 12;
  public bool canDobleJump = true;
  private BoxCollider2D boxCollider2D;
  public GroundSensor groundSensor;
  public Oskar oskar;
  private Animator animator;
  private bool platform;
  private bool IsUp; 

  void Awake()
  {
    _rigidBody = GetComponentInParent<Rigidbody2D>();
    boxCollider2D = GetComponent<BoxCollider2D>();
    oskar = GetComponentInParent<Oskar>();
    groundSensor = GetComponentInChildren<GroundSensor>();  
    animator = GetComponentInParent<Animator>();

  }

   
  void OnTriggerEnter2D(Collider2D collider)
  {
    if(collider.gameObject.layer == 3)
    {
      isGrounded = true;
      canDobleJump = true;
    }

    if(collider.gameObject.layer == 8)
    {
      oskar.Death();
    }
    
  } 



  void OnTriggerStay2D(Collider2D collider)
  {
    if(collider.gameObject.layer == 3)
    {
      isGrounded = true;
    }
  }


  void OnTriggerExit2D(Collider2D collider)
  {
    if(collider.gameObject.layer == 3)
    {
      isGrounded = false;
    }
  }


  

}
