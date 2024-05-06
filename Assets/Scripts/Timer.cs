using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text countdownText; // Référence au texte d'affichage du chronomètre
    [SerializeField][Range(0, 900)] private float remainingTime; // Temps restant en secondes

    private void Update()
    {
        // Si le temps restant est supérieur à 0, le décrémente et met à jour l'affichage
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            UpdateCountdownDisplay();
        }
        // Sinon, affiche un panneau indiquant que le temps est écoulé
        else
        {
            GameManager.Instance.ShowTimeOverPanel();
        }
    }

    // Met à jour l'affichage du chronomètre
    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        countdownText.text = $"{minutes:00}:{seconds:00}";
    }
}