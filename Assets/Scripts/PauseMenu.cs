using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}