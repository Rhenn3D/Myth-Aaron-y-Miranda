using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Barra de vida")]
    public Image fillImage;

    [Range(0, 100)]
    public float maxHealth = 20f;
    private float currentHealth;

    
    void Start()
    {
        maxHealth = 20f; // o sincronizar desde el personaje
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateHealthBar();
    }

    public void SetHealth(float current, float max)
    {
        currentHealth = Mathf.Clamp(current, 0f, max);
        maxHealth = max;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        fillImage.fillAmount = currentHealth / maxHealth;
    }
}
