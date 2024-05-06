using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] private Slider slider; // Slider UI pour représenter visuellement le bouclier

    private Image fill; // L'image de remplissage du slider

    private void Awake()
    {
        // Initialisation du composant Image pour le remplissage du slider
        if (slider != null && slider.fillRect != null)
        {
            fill = slider.fillRect.GetComponent<Image>();
        }
    }

    // Définit le bouclier maximal pour le slider et met à jour la valeur initiale
    public void SetMaxShield(int shield)
    {
        slider.maxValue = shield;
        slider.value = shield;
    }

    // Met à jour la valeur actuelle du bouclier sur le slider et contrôle la visibilité de la barre
    public void SetShield(int shield)
    {
        slider.value = shield;
        // Active ou désactive le gameObject selon si le bouclier est supérieur à 0
        gameObject.SetActive(shield > 0);
    }
}