using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cofre : MonoBehaviour

{
    [Header("UI del Panel")]
    public GameObject panelUI;           // Panel del objeto
    public Text tituloTexto;             // Texto del título
    public Text descripcionTexto;        // Texto de la descripción
    public Image imagenObjeto;           // Imagen del objeto

    [Header("Contenido del Cofre")]
    public string titulo;
    [TextArea]
    public string descripcion;
    public Sprite spriteObjeto;          // Sprite del objeto a mostrar

    private bool yaFueAbierto = false;

    private void Start()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(false); // Oculta el panel al inicio
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!yaFueAbierto && collision.CompareTag("Player"))
        {
            MostrarPanel();
            yaFueAbierto = true;
        }
    }

    private void MostrarPanel()
    {
        if (panelUI != null)
        {
            tituloTexto.text = titulo;
            descripcionTexto.text = descripcion;

            if (imagenObjeto != null && spriteObjeto != null)
            {
                imagenObjeto.sprite = spriteObjeto;
                imagenObjeto.enabled = true;
            }

            panelUI.SetActive(true);
        }
    }

    public void CerrarPanel()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(false);
        }
    }
}

