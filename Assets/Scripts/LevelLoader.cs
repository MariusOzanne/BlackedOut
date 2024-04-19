using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    // Composants
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

    // Fonction de chargement du niveau (argument, index de la scène à charger)
    public void LoadLevel(int sceneIndex)
    {
        Time.timeScale = 1;
        // Démarage de la coroutine (IEnumerator) 
        StartCoroutine(LoadAsync(sceneIndex)); //Permet d'exécuter du code en parallèle, sans bloquer l'exécution du thread principal
    }

    IEnumerator LoadAsync(int sceneIndex)
    {

        // Stockage de l'avancement de la scène
        // Chargement de la scène en arrière plan (continue de jouer la scène en cours)
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // Activer l'écran de chargement lorsque le chargement de la scène se lance
        loadingScreen.SetActive(true);


        // Vérifier l'état de l'avancement
        while (!operation.isDone) // Tant que l'opération n'est pas terminée
        {
            // On stocke la progression du chargement et de l'activation dans une variable progress
            // Chargement de la scène entre 0 et 0.9, et activation de la scène entre 0.9 et 1
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Permet de bloquer la valeur entre 0 et 1
            slider.value = progress;                                   // Afficher la progression du chargement dans le slider
            progressText.text = progress * 100 + "%";                  // Le texte se met à jour en fonction du % de chargement du niveau
            yield return null;                                         // Permet d'attendre un frame entre chaque lecture
        }
    }
}
