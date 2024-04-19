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

    // Fonction de chargement du niveau (argument, index de la sc�ne � charger)
    public void LoadLevel(int sceneIndex)
    {
        Time.timeScale = 1;
        // D�marage de la coroutine (IEnumerator) 
        StartCoroutine(LoadAsync(sceneIndex)); //Permet d'ex�cuter du code en parall�le, sans bloquer l'ex�cution du thread principal
    }

    IEnumerator LoadAsync(int sceneIndex)
    {

        // Stockage de l'avancement de la sc�ne
        // Chargement de la sc�ne en arri�re plan (continue de jouer la sc�ne en cours)
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // Activer l'�cran de chargement lorsque le chargement de la sc�ne se lance
        loadingScreen.SetActive(true);


        // V�rifier l'�tat de l'avancement
        while (!operation.isDone) // Tant que l'op�ration n'est pas termin�e
        {
            // On stocke la progression du chargement et de l'activation dans une variable progress
            // Chargement de la sc�ne entre 0 et 0.9, et activation de la sc�ne entre 0.9 et 1
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Permet de bloquer la valeur entre 0 et 1
            slider.value = progress;                                   // Afficher la progression du chargement dans le slider
            progressText.text = progress * 100 + "%";                  // Le texte se met � jour en fonction du % de chargement du niveau
            yield return null;                                         // Permet d'attendre un frame entre chaque lecture
        }
    }
}
