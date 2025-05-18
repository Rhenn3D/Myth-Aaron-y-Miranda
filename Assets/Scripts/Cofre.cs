using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cofre : MonoBehaviour

{
    public Sprite cofreCerrado;
    public Sprite cofreAbierto;
    private SpriteRenderer spriteRenderer;

    public GameObject panelObjeto;
    public Text textoDescripcion;
    public Image imagenObjeto;

    public string descripcionObjeto;
    public Sprite spriteObjeto;

    public GameObject textoInteractuarUI;
    private Vector3 textoOriginalPos;

    public float velocidadFlotacion = 1f;
    public float amplitudFlotacion = 0.2f;

    private bool jugadorCerca = false;
    private bool cofreAbiertoFlag = false;

    public AudioSource sonidoApertura; // ← Añade esta referencia

    private GameObject player;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cofreCerrado;

        if (panelObjeto != null)
            panelObjeto.SetActive(false);

        if (textoInteractuarUI != null)
        {
            textoOriginalPos = textoInteractuarUI.transform.localPosition;
            textoInteractuarUI.SetActive(false);
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (jugadorCerca && !cofreAbiertoFlag)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AbrirCofre();
            }

            if (textoInteractuarUI != null)
            {
                float offsetY = Mathf.Sin(Time.time * velocidadFlotacion) * amplitudFlotacion;
                textoInteractuarUI.transform.localPosition = textoOriginalPos + new Vector3(0f, offsetY, 0f);
            }
        }
    }

    private void AbrirCofre()
    {
        cofreAbiertoFlag = true;
        spriteRenderer.sprite = cofreAbierto;

        if (textoInteractuarUI != null)
            textoInteractuarUI.SetActive(false);

        if (panelObjeto != null)
        {
            panelObjeto.SetActive(true);

            if (textoDescripcion != null)
                textoDescripcion.text = descripcionObjeto;

            if (imagenObjeto != null && spriteObjeto != null)
                imagenObjeto.sprite = spriteObjeto;
        }

        if (sonidoApertura != null)
            sonidoApertura.Play();

        // Aquí aumentamos las patatas del jugador
        if (player != null)
        {
            OskarController oskar = player.GetComponent<OskarController>();
            if (oskar != null)
            {
                oskar.cantidadPatatas++;  // Suma 1 patata al jugador
                oskar.cantidadPatatas++;  // Suma 1 patata al jugador
                oskar.cantidadPatatas++;  // Suma 1 patata al jugador
                Debug.Log("Patata recogida. Total: " + oskar.cantidadPatatas);
            }
        }
    }

    public void CerrarPanel()
    {
        if (panelObjeto != null)
            panelObjeto.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !cofreAbiertoFlag)
        {
            jugadorCerca = true;
            if (textoInteractuarUI != null)
            {
                textoOriginalPos = textoInteractuarUI.transform.localPosition;
                textoInteractuarUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (textoInteractuarUI != null)
                textoInteractuarUI.SetActive(false);
        }
    }
}

