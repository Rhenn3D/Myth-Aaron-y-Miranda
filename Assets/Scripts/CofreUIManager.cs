using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofreUIManager : MonoBehaviour
{
    [Header("Panel del cofre")]
    public GameObject chestUIPanel; // Asigna tu panel de UI aquí en el inspector

    void Start()
    {
        chestUIPanel.SetActive(false); // Asegúrate de que inicia desactivado
    }

    void Update()
    {
        // Cierra con tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape) && chestUIPanel.activeSelf)
        {
            CloseChestUI();
        }
    }

    // Llama a esto desde el script del cofre al abrirlo
    public void OpenChestUI()
    {
        chestUIPanel.SetActive(true);
    }

    // Llama a esto desde el botón "Cerrar"
    public void CloseChestUI()
    {
        chestUIPanel.SetActive(false);
    }
}
