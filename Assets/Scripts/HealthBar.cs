using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Configuration de la barre de vie")]
    [SerializeField] private bool isPlayer;
    [SerializeField] private Slider lifeBar;
    [SerializeField] private Gradient gradient;

    private Image fill;
    private int maxHealth;

    void Start()
    {
        if (lifeBar == null)
        {
            Debug.LogError("Slider lifeBar n'est pas assigné sur " + gameObject.name);
            return;
        }

        // Recherche de l'Image du Fill dynamiquement
        fill = lifeBar.fillRect.GetComponent<Image>();
        if (fill == null)
        {
            Debug.LogError("Image fill n'est pas trouvée sur le Slider " + gameObject.name);
            return;
        }

        // Configuration initiale de la barre de vie
        SetupHealthBar();
    }

    private void SetupHealthBar()
    {
        if (isPlayer)
        {
            maxHealth = GameManager.Instance.life;
        }
        else
        {
            EnemyData enemyData = GetComponentInParent<EnemyController>()?.enemyData;
            if (enemyData != null)
            {
                maxHealth = enemyData.maxHealth;
            }
            else
            {
                Debug.LogError("EnemyData non trouvé sur " + gameObject.name);
                return;
            }
        }

        lifeBar.maxValue = maxHealth;
        lifeBar.value = maxHealth;
        fill.color = gradient.Evaluate(1f);
    }

    void Update()
    {
        int currentHealth = isPlayer ? GameManager.Instance.life : GetComponentInParent<EnemyController>()?.enemyData.health ?? 0;
        lifeBar.value = currentHealth;
        fill.color = gradient.Evaluate(lifeBar.normalizedValue);
    }
}