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
  private AudioSource runSound;
  public AudioClip runVFX;





  void Awake()
  {
    _rigidBody = GetComponentInParent<Rigidbody2D>();
    runSound = GetComponent<AudioSource>();

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
