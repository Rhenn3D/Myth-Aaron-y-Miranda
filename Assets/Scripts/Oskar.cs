using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oskar : MonoBehaviour
{
    [Header("Physics, RigidBody, GroundSensor, BoxCollider")]
    public Rigidbody2D rigidBody;
    private SpriteRenderer _spriteRenderer;
    public GroundSensor _groundSensor;
    private BoxCollider2D _boxCollider;


    [Header("Key")]
    [SerializeField]private float inputHorizontal;


    [Header("Run")]
    public float playerSpeed = 5f;
    public int direction = 1;


    [Header("Jump")]
    public float jumpForce = 25;

     [Header("Hit")]
     [SerializeField] private float _attackDamage = 5;
     [SerializeField] private float _attackRadius = 0.4f;
     [SerializeField] private Transform _hitBoxPosition;
     [SerializeField] private float _baseChargedAttackDamage = 15;
     [SerializeField] private float _maxChargedAttackDamage = 40;
     private float _chargedAttackDamage;
     [SerializeField] private LayerMask _enemyLayer;
     





    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
   
    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");


        if(Input.GetButtonDown("Jump") && _groundSensor.isGrounded == true)
        {
            Jump();
        }

        /*if(Input.GetButtonDown("Fire2"))
        {
            NormalAttack();
        }*/

        if(Input.GetButton("Fire2"))
        {
            AttackCharge();
        }

        if(Input.GetButtonUp("Fire2"))
        {
            ChargedAttack();
        }
    }
    


    void FixedUpdate()
    {
        rigidBody.velocity = new  Vector2(inputHorizontal * playerSpeed, rigidBody.velocity.y);
    }


    void Movement()
    {
        if(inputHorizontal > 0)
        {
           transform.rotation = Quaternion.Euler(0, 0, 0);
            //_animator.SetBool("IsRunning", true);
        }


      else if(inputHorizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //_animator.SetBool("IsRunning", true);      
        }
      /*else
        {
            _animator.SetBool("IsRunning", false);
        }
        */
    }


    void Jump()
    {
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void NormalAttack()
    {
        Collider2D[] phetonisios = Physics2D.OverlapCircleAll(_hitBoxPosition.position, _attackRadius, _enemyLayer);
        foreach(Collider2D phetonisio in phetonisios)
        {

            Phetonisio enemyScript = phetonisio.GetComponent<Phetonisio>();
            enemyScript.TakeDamage(_attackDamage);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_hitBoxPosition.position, _attackRadius);
    }

    void AttackCharge()
    {
        if(_chargedAttackDamage < _maxChargedAttackDamage)
        {
        _chargedAttackDamage += Time.deltaTime;
         Debug.Log(_chargedAttackDamage); 
        }
        else 
        {
            _chargedAttackDamage = _maxChargedAttackDamage;
            
        }
        
    
    }
    

    void ChargedAttack()
    {
      
    
        Collider2D[] phetonisios = Physics2D.OverlapCircleAll(_hitBoxPosition.position, _attackRadius, _enemyLayer);
        foreach(Collider2D phetonisio in phetonisios)
        {

            Phetonisio enemyScript = phetonisio.GetComponent<Phetonisio>();
            enemyScript.TakeDamage(_chargedAttackDamage);
        }
        _chargedAttackDamage = _baseChargedAttackDamage;
    }

}

