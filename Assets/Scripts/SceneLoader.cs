using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen; // �cran de chargement � afficher pendant le chargement de la sc�ne
    [SerializeField] private Slider loadingProgressBar; // Barre de progression pour montrer l'�tat de chargement
    [SerializeField] private Text loadingProgressText; // Texte pour afficher le pourcentage de chargement

    public void LoadSceneAsync(int sceneIndex)
    {
        StartCoroutine(AsyncLoadScene(sceneIndex)); // D�marre la coroutine pour le chargement asynchrone
    }

    private IEnumerator AsyncLoadScene(int sceneIndex)
    {
        Time.timeScale = 1; // Assure que le temps n'est pas ralenti ou acc�l�r�

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex); // D�marre le chargement asynchrone de la sc�ne
        loadingScreen.SetActive(true); // Active l'�cran de chargement

        while (!loadOperation.isDone) // Tant que le chargement n'est pas termin�
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f); // Calcule le pourcentage de progression
            loadingProgressBar.value = progress; // Met � jour la barre de progression
            loadingProgressText.text = $"{progress * 100:0}%"; // Affiche le pourcentage de progression

            yield return null; // Attend un frame avant de continuer
        }
    }
}