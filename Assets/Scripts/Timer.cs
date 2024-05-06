using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text countdownText; // R�f�rence au texte d'affichage du chronom�tre
    [SerializeField][Range(0, 900)] private float remainingTime; // Temps restant en secondes

    private void Update()
    {
        // Si le temps restant est sup�rieur � 0, le d�cr�mente et met � jour l'affichage
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            UpdateCountdownDisplay();
        }
        // Sinon, affiche un panneau indiquant que le temps est �coul�
        else
        {
            GameManager.Instance.ShowTimeOverPanel();
        }
    }

    // Met � jour l'affichage du chronom�tre
    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        countdownText.text = $"{minutes:00}:{seconds:00}";
    }
}