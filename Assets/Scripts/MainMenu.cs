using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGameButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void OpenWebsiteButton(string url)
    {
        Application.OpenURL(url);
    }
}