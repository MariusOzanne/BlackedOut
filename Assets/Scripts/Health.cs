using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0, 100)] public int maxHealth; // Définir la santé maximale de l'objet
    [Range(0, 100)] public int currentHealth; // Définir la santé actuelle de l'objet

    public HealthBar healthBar; // Référence à la barre de santé pour les mises à jour visuelles

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth); // Initialise la valeur maximale de la barre de santé
        currentHealth = maxHealth; // Assigne la santé maximale au démarrage
        UpdateHealthBar(); // Met à jour visuellement la barre de santé
    }

    private void OnValidate()
    {
        if (healthBar != null && Application.isPlaying)
        {
            UpdateHealthBar(); // Met à jour la barre de santé lors des modifications dans l'inspecteur, si l'application est en cours d'exécution
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount; // Ajoute la quantité spécifiée à la santé actuelle
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // S'assure que la santé reste dans les limites autorisées
        UpdateHealthBar(); // Met à jour la barre de santé pour refléter le changement
    }

    public void TakeDamage(int damage)
    {
        Shield shield = GetComponent<Shield>(); // Tente de récupérer un composant Shield
        if (shield != null && shield.currentShield > 0)
        {
            int damageToShield = Mathf.Min(damage, shield.currentShield); // Calcule le dommage absorbé par le bouclier
            shield.TakeShieldDamage(damageToShield); // Applique le dommage au bouclier
            damage -= damageToShield; // Réduit le dommage par la quantité absorbée par le bouclier
        }

        if (damage > 0)
        {
            currentHealth -= damage; // Applique le reste des dommages à la santé
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Assure que la santé ne tombe pas en dessous de zéro
            UpdateHealthBar(); // Met à jour la barre de santé pour refléter le changement
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.SetHealth(currentHealth); // Met à jour la barre de santé avec la valeur actuelle
    }
}