using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        // Pour charger la scène de jeu de façon asynchrone (pour ne pas figer l'application)
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        // Pour quitter le jeu dans Unity
        Application.Quit();
    }

    public void OpenSite(string url)
    {
        Application.OpenURL(url);
    }
}
