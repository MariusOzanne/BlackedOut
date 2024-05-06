using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Range(0, 100)] public int maxShield; // D�finir le bouclier maximal de l'objet
    [Range(0, 100)] public int currentShield; // D�finir le bouclier actuel de l'objet

    public ShieldBar shieldBar; // R�f�rence � la barre de bouclier pour les mises � jour visuelles

    private void Start()
    {
        shieldBar.SetMaxShield(maxShield); // Initialise la valeur maximale de la barre de bouclier
        currentShield = 0; // D�marre avec un bouclier de z�ro
        UpdateShieldBar(); // Met � jour visuellement la barre de bouclier
    }

    private void OnValidate()
    {
        if (shieldBar != null && Application.isPlaying)
        {
            UpdateShieldBar(); // Met � jour la barre de bouclier lors des modifications dans l'inspecteur, si l'application est en cours d'ex�cution
        }
    }

    public void AddShield(int amount)
    {
        currentShield += amount; // Ajoute la quantit� sp�cifi�e au bouclier actuel
        currentShield = Mathf.Clamp(currentShield, 0, maxShield); // S'assure que le bouclier reste dans les limites autoris�es
        UpdateShieldBar(); // Met � jour la barre de bouclier pour refl�ter le changement
    }

    public void TakeShieldDamage(int damage)
    {
        currentShield -= damage; // R�duit le bouclier par la quantit� de dommage re�ue
        currentShield = Mathf.Clamp(currentShield, 0, maxShield); // Assure que le bouclier ne tombe pas en dessous de z�ro
        UpdateShieldBar(); // Met � jour la barre de bouclier pour refl�ter le changement
    }

    private void UpdateShieldBar()
    {
        shieldBar.SetShield(currentShield); // Met � jour la barre de bouclier avec la valeur actuelle
        shieldBar.gameObject.SetActive(currentShield > 0); // Active ou d�sactive la barre de bouclier selon si le bouclier est sup�rieur � z�ro
    }
}