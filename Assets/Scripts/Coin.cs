using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    private AudioSource audioSource;
    public AudioClip CoinSFX;
    private SpriteRenderer coinrenderer;
    GameManager gameManager;
    public GameObject particlerender;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        coinrenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            Interact();
        }
    }
    void Interact()
    {
        boxCollider.enabled = false;
        coinrenderer.enabled = false;
        particlerender.SetActive(false);
        audioSource.PlayOneShot(CoinSFX);
        Destroy(gameObject, CoinSFX.length);
    }
}
