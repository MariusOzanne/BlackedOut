using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;

    // On creee une variable publique Instance pour pouvoir acceder a la variable privee _Instance
    public static GameManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GameManager();
            }
            return _Instance;
        }
    }

    [Header("Caracteristiques du joueur")]
    [Range(0, 100)] public int life;
    [Range(0, 100)] public int shield;
    public int coins;
    public int score;

    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Text coinsText;
    [SerializeField] private Text scoreText;

    // On creee le patern singleton
    // Permettant d'acceder a un script a partir d'un autre script
    private void Awake()
    {
        // Si l'instance n'a pas ete attribue
        if (_Instance == null)
        {
            _Instance = this;   // La variable _Instance = la classe GameManager
        }
        // Sinon
        else
        {
            Destroy(gameObject);      // On la detruit si y'en a trop (car un singleton doit etre unique)
        }
    }

    private void Start()
    {
        UpdateCoinCount();
        UpdateScore();
    }

    public void CheckPlayerDefeat()
    {
        if (life <= 0)
        {
            ShowDefeatPanel();
        }
    }

    private void ShowDefeatPanel()
    {
        defeatPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void UpdateCoinCount()
    {
        coinsText.text = "Coins : " + coins;
    }

    public void UpdateScore()
    {
        scoreText.text = "Score : " + score;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}