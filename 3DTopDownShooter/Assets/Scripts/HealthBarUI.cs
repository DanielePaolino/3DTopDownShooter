using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private Gradient gradient;

    private float healthFillAmount = 1f;

    private void OnEnable()
    {
        healthBarSprite.fillAmount = 1f;
        healthFillAmount = 1f;
        healthBarSprite.color = gradient.Evaluate(healthFillAmount);
    }

    public void RefillHealthBar()
    {
        healthBarSprite.fillAmount = 1;
        healthFillAmount = 1;
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if(currentHealth < 0)
            currentHealth = 0;

        healthFillAmount = (float)currentHealth / maxHealth;     
        healthBarSprite.fillAmount = healthFillAmount;
        healthBarSprite.color = gradient.Evaluate(healthFillAmount);
    }
}
