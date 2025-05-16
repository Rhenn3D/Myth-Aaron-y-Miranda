using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour

{
    public Animator animator;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    public GroundSensor groundSensor;
    public bool IsUp;

    // Start is called before the first frame update

    void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundSensor = GetComponentInChildren<GroundSensor>();
       
    }
    
    
}
