using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton pour acc�der � l'instance de GameManager de partout

    [Header("Player Scores")]
    public int coins; // Nombre de pi�ces collect�es par le joueur
    public int score; // Score total du joueur

    private int initialCoins; // Initialisation du compteur de pi�ces pour les comparaisons
    private int initialScore; // Initialisation du compteur de score pour les comparaisons

    [SerializeField] private Text coinsText; // UI pour afficher les pi�ces
    [SerializeField] private Text scoreText; // UI pour afficher le score

    [Header("Defeat UI")]
    [SerializeField] private Text defeatCoinsText; // Texte pour les pi�ces sur l'�cran de d�faite
    [SerializeField] private Text defeatScoreText; // Texte pour le score sur l'�cran de d�faite
    [SerializeField] private GameObject defeatPanel; // Panneau UI montr� lors de la d�faite

    [Header("Time Over UI")]
    [SerializeField] private Text timeOverCoinsText; // Texte pour les pi�ces sur l'�cran de temps �coul�
    [SerializeField] private Text timeOverScoreText; // Texte pour le score sur l'�cran de temps �coul�
    [SerializeField] private GameObject timeOverPanel; // Panneau UI montr� quand le temps est �coul�

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindObjectOfType<SaveSystem>().LoadData(); // Chargement des donn�es du joueur
        initialCoins = coins; // D�finit les pi�ces initiales apr�s le chargement
        initialScore = score; // D�finit le score initial apr�s le chargement
        UpdateCoinsUI(); // Mise � jour de l'affichage des pi�ces
        UpdateScoreUI(); // Mise � jour de l'affichage du score
    }

    public void UpdateCoinsUI()
    {
        coinsText.text = coins.ToString(); // Mise � jour de l'UI des pi�ces
    }

    public void UpdateScoreUI()
    {
        scoreText.text = score.ToString(); // Mise � jour de l'UI du score
    }

    public void ShowDefeatPanel()
    {
        UpdateFinalPanel(defeatCoinsText, defeatScoreText, defeatPanel); // Mise � jour du panneau de d�faite
    }

    public void ShowTimeOverPanel()
    {
        UpdateFinalPanel(timeOverCoinsText, timeOverScoreText, timeOverPanel); // Mise � jour du panneau de temps �coul�
    }

    private void UpdateFinalPanel(Text coinsText, Text scoreText, GameObject panel)
    {
        // R�initialise les effets de rage
        RageManager rageManager = FindObjectOfType<RageManager>();
        if (rageManager != null)
        {
            rageManager.ResetRageEffects();
        }

        coinsText.text = $"{initialCoins} (+{coins - initialCoins})"; // Format de l'affichage des pi�ces
        scoreText.text = $"{initialScore} (+{score - initialScore})"; // Format de l'affichage du score

        FindObjectOfType<SaveSystem>().SaveData();

        initialCoins = coins; // Met � jour les pi�ces initiales apr�s la sauvegarde
        initialScore = score; // Met � jour le score initial apr�s la sauvegarde

        panel.SetActive(true);
        Time.timeScale = 0; // Arr�te le temps pour afficher le panneau
    }

    public void ResetScoresToDefault()
    {
        coins = 0; // R�initialise les pi�ces � 0
        score = 0; // R�initialise le score � 0
        initialCoins = 0; // R�initialise les pi�ces initiales � 0
        initialScore = 0; // R�initialise le score initial � 0

        UpdateCoinsUI(); // Mise � jour de l'UI des pi�ces
        UpdateScoreUI(); // Mise � jour de l'UI du score
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Restaure le temps normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharge la sc�ne actuelle
    }
}