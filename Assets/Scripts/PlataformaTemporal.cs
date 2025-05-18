using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaTemporal : MonoBehaviour
{
    public float tiempoAntesDeDesaparecer = 2f;  // Tiempo antes de empezar a desvanecer
    public float tiempoDesvanecimiento = 1f;     // Tiempo que tarda en desaparecer (opacidad 0)
    public float tiempoInactiva = 3f;             // Tiempo que permanece invisible

    public Sprite spriteNormal;
    public Sprite spriteCuandoPisa;

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    private bool procesoEnCurso = false;  // Para controlar si ya empezó la desaparición
    private float timer = 0f;

    private enum Estado { Normal, Esperando, Desvaneciendo, Inactiva }
    private Estado estado = Estado.Normal;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer.sprite = spriteNormal;
    }

    void Update()
    {
        if (estado == Estado.Esperando)
        {
            timer += Time.deltaTime;
            if (timer >= tiempoAntesDeDesaparecer)
            {
                timer = 0f;
                estado = Estado.Desvaneciendo;
            }
        }
        else if (estado == Estado.Desvaneciendo)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / tiempoDesvanecimiento);
            SetAlpha(alpha);

            if (timer >= tiempoDesvanecimiento)
            {
                // Desaparece la plataforma
                spriteRenderer.enabled = false;
                platformCollider.enabled = false;
                estado = Estado.Inactiva;
                timer = 0f;
            }
        }
        else if (estado == Estado.Inactiva)
        {
            timer += Time.deltaTime;
            if (timer >= tiempoInactiva)
            {
                // Reactiva plataforma
                spriteRenderer.enabled = true;
                platformCollider.enabled = true;
                SetAlpha(1f);
                spriteRenderer.sprite = spriteNormal;
                estado = Estado.Normal;
                procesoEnCurso = false;
                timer = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spriteRenderer.sprite = spriteCuandoPisa;

            if (!procesoEnCurso)
            {
                procesoEnCurso = true;
                estado = Estado.Esperando;
                timer = 0f;
            }
        }
    }

    // Eliminamos OnCollisionExit2D o lo dejamos vacío porque no debe afectar el proceso

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}