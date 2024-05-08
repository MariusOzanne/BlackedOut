using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private SceneLoader sceneLoader;

    public void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseMenuPanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        RageManager rageManager = FindObjectOfType<RageManager>();
        if (rageManager != null)
        {
            rageManager.ResetRageEffects();
        }

        FindObjectOfType<SaveSystem>().SaveData();
        sceneLoader.LoadSceneAsync(0);
    }
}