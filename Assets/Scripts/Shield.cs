using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Range(0, 100)] public int maxShield; // Définir le bouclier maximal de l'objet
    [Range(0, 100)] public int currentShield; // Définir le bouclier actuel de l'objet

    public ShieldBar shieldBar; // Référence à la barre de bouclier pour les mises à jour visuelles

    private void Start()
    {
        shieldBar.SetMaxShield(maxShield); // Initialise la valeur maximale de la barre de bouclier
        currentShield = 0; // Démarre avec un bouclier de zéro
        UpdateShieldBar(); // Met à jour visuellement la barre de bouclier
    }

    private void OnValidate()
    {
        if (shieldBar != null && Application.isPlaying)
        {
            UpdateShieldBar(); // Met à jour la barre de bouclier lors des modifications dans l'inspecteur, si l'application est en cours d'exécution
        }
    }

    public void AddShield(int amount)
    {
        currentShield += amount; // Ajoute la quantité spécifiée au bouclier actuel
        currentShield = Mathf.Clamp(currentShield, 0, maxShield); // S'assure que le bouclier reste dans les limites autorisées
        UpdateShieldBar(); // Met à jour la barre de bouclier pour refléter le changement
    }

    public void TakeShieldDamage(int damage)
    {
        currentShield -= damage; // Réduit le bouclier par la quantité de dommage reçue
        currentShield = Mathf.Clamp(currentShield, 0, maxShield); // Assure que le bouclier ne tombe pas en dessous de zéro
        UpdateShieldBar(); // Met à jour la barre de bouclier pour refléter le changement
    }

    private void UpdateShieldBar()
    {
        shieldBar.SetShield(currentShield); // Met à jour la barre de bouclier avec la valeur actuelle
        shieldBar.gameObject.SetActive(currentShield > 0); // Active ou désactive la barre de bouclier selon si le bouclier est supérieur à zéro
    }
}