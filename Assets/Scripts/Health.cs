using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0, 100)] public int maxHealth; // D�finir la sant� maximale de l'objet
    [Range(0, 100)] public int currentHealth; // D�finir la sant� actuelle de l'objet

    public HealthBar healthBar; // R�f�rence � la barre de sant� pour les mises � jour visuelles

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth); // Initialise la valeur maximale de la barre de sant�
        currentHealth = maxHealth; // Assigne la sant� maximale au d�marrage
        UpdateHealthBar(); // Met � jour visuellement la barre de sant�
    }

    private void OnValidate()
    {
        if (healthBar != null && Application.isPlaying)
        {
            UpdateHealthBar(); // Met � jour la barre de sant� lors des modifications dans l'inspecteur, si l'application est en cours d'ex�cution
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount; // Ajoute la quantit� sp�cifi�e � la sant� actuelle
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // S'assure que la sant� reste dans les limites autoris�es
        UpdateHealthBar(); // Met � jour la barre de sant� pour refl�ter le changement
    }

    public void TakeDamage(int damage)
    {
        Shield shield = GetComponent<Shield>(); // Tente de r�cup�rer un composant Shield
        if (shield != null && shield.currentShield > 0)
        {
            int damageToShield = Mathf.Min(damage, shield.currentShield); // Calcule le dommage absorb� par le bouclier
            shield.TakeShieldDamage(damageToShield); // Applique le dommage au bouclier
            damage -= damageToShield; // R�duit le dommage par la quantit� absorb�e par le bouclier
        }

        if (damage > 0)
        {
            currentHealth -= damage; // Applique le reste des dommages � la sant�
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Assure que la sant� ne tombe pas en dessous de z�ro
            UpdateHealthBar(); // Met � jour la barre de sant� pour refl�ter le changement
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.SetHealth(currentHealth); // Met � jour la barre de sant� avec la valeur actuelle
    }
}