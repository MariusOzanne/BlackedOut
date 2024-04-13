using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Variables & Composants vie joueur

    [Header("Variables & Composants Vie joueur")]
    public int maxHealth;                   
    public Slider lifeBar;
    public Gradient gradient;
    public Image fill;

    #endregion

    private EnemyData enemyData;

    private void Start()
    {
        // Définir la vie du joueur
        maxHealth = GameManager.Instance.life;
        lifeBar.maxValue = maxHealth;

        // On évalue à quel niveau ce trouve la couleur (1 étant la couleur verte car les HP sont au max)
        fill.color = gradient.Evaluate(1f);

    }

    private void Update()
    {
        // Mise à jour de la barre de vie
        lifeBar.value = GameManager.Instance.life;
        // Mise à jour du dégradé de couleur en fonction de la vie qui reste au joueur
        fill.color = gradient.Evaluate(lifeBar.normalizedValue);
    }
}