using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] private Slider slider; // Slider UI pour repr�senter visuellement le bouclier

    private Image fill; // L'image de remplissage du slider

    private void Awake()
    {
        // Initialisation du composant Image pour le remplissage du slider
        if (slider != null && slider.fillRect != null)
        {
            fill = slider.fillRect.GetComponent<Image>();
        }
    }

    // D�finit le bouclier maximal pour le slider et met � jour la valeur initiale
    public void SetMaxShield(int shield)
    {
        slider.maxValue = shield;
        slider.value = shield;
    }

    // Met � jour la valeur actuelle du bouclier sur le slider et contr�le la visibilit� de la barre
    public void SetShield(int shield)
    {
        slider.value = shield;
        // Active ou d�sactive le gameObject selon si le bouclier est sup�rieur � 0
        gameObject.SetActive(shield > 0);
    }
}