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
        Phetonisio enemigo = other.GetComponent<Phetonisio>();
        if (enemigo != null)
        {
            enemigo.TakeDamage(1); // Le quitas 1 de vida o la cantidad que quieras
        }
        Destroy(gameObject); // Destruyes la patata al impactar
    }
}
    
}
