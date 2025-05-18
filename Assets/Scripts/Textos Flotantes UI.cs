using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextosFlotantesUI : MonoBehaviour
{
    public float amplitud = 5f;      // Qué tanto se mueve verticalmente (en píxeles o unidades de canvas)
    public float velocidad = 2f;     // Qué tan rápido se mueve

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * velocidad) * amplitud;
        transform.localPosition = posicionInicial + new Vector3(0, offset, 0);
    }
}
