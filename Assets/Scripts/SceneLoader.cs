using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen; // Écran de chargement à afficher pendant le chargement de la scène
    [SerializeField] private Slider loadingProgressBar; // Barre de progression pour montrer l'état de chargement
    [SerializeField] private Text loadingProgressText; // Texte pour afficher le pourcentage de chargement

    public void LoadSceneAsync(int sceneIndex)
    {
        StartCoroutine(AsyncLoadScene(sceneIndex)); // Démarre la coroutine pour le chargement asynchrone
    }

    private IEnumerator AsyncLoadScene(int sceneIndex)
    {
        Time.timeScale = 1; // Assure que le temps n'est pas ralenti ou accéléré

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex); // Démarre le chargement asynchrone de la scène
        loadingScreen.SetActive(true); // Active l'écran de chargement

        while (!loadOperation.isDone) // Tant que le chargement n'est pas terminé
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f); // Calcule le pourcentage de progression
            loadingProgressBar.value = progress; // Met à jour la barre de progression
            loadingProgressText.text = $"{progress * 100:0}%"; // Affiche le pourcentage de progression

            yield return null; // Attend un frame avant de continuer
        }
    }
}