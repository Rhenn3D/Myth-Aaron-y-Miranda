using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [Header("Static Ground")]
  private Rigidbody2D _rigidBody;
 
  [Header("Oshkarsh Jump")]
  public bool isGrounded;
  public float jumpForce = 12;




  void Awake()
  {
    _rigidBody = GetComponentInParent<Rigidbody2D>();
  }
   
  void OnTriggerEnter2D(Collider2D collider)
  {
    if(collider.gameObject.layer == 3)
    {
      isGrounded = true;
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
