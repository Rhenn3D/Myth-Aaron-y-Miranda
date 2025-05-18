using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatataLanzada : MonoBehaviour
{
    public float lifetime = 3f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Destruir al enemigo
            Destroy(other.gameObject);

            // Destruir la patata
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player") && !other.isTrigger)
        {
            // Si choca con algo que no sea el jugador, también se destruye
            Destroy(gameObject);
        }
    }
}
