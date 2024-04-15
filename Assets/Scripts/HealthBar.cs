using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Configuration de la barre de vie")]
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isPortal;
    [SerializeField] private Slider lifeBar;
    [SerializeField] private Gradient gradient;

    private Image fill;
    private int maxHealth;
    private PortalController portalController;

    void Start()
    {
        if (lifeBar == null)
        {
            Debug.LogError("Slider lifeBar n'est pas assigné sur " + gameObject.name);
            return;
        }

        fill = lifeBar.fillRect.GetComponent<Image>();
        if (fill == null)
        {
            Debug.LogError("Image fill n'est pas trouvée sur le Slider " + gameObject.name);
            return;
        }

        SetupHealthBar();
    }

    private void SetupHealthBar()
    {
        if (isPlayer)
        {
            maxHealth = GameManager.Instance.life;
            lifeBar.maxValue = maxHealth;
            lifeBar.value = maxHealth;
        }
        else if (isPortal)
        {
            portalController = GetComponentInParent<PortalController>();
            if (portalController != null)
            {
                maxHealth = (int)portalController.portalHealth;
                lifeBar.maxValue = maxHealth;
                lifeBar.value = maxHealth;
            }
            else
            {
                Debug.LogError("PortalController non trouvé sur " + gameObject.name);
                return;
            }
        }
        else
        {
            EnemyData enemyData = GetComponentInParent<EnemyController>()?.enemyData;
            if (enemyData != null)
            {
                maxHealth = enemyData.maxHealth;
                lifeBar.maxValue = maxHealth;
                lifeBar.value = enemyData.health;
            }
            else
            {
                Debug.LogError("EnemyData non trouvé sur " + gameObject.name);
                return;
            }
        }

        fill.color = gradient.Evaluate(1f);
    }

    void Update()
    {
        int currentHealth;
        if (isPlayer)
        {
            currentHealth = GameManager.Instance.life;
        }
        else if (isPortal && portalController != null)
        {
            currentHealth = (int)portalController.currentHealth;
        }
        else
        {
            currentHealth = GetComponentInParent<EnemyController>()?.enemyData.health ?? 0;
        }

        lifeBar.value = currentHealth;
        fill.color = gradient.Evaluate(lifeBar.normalizedValue);
    }
}