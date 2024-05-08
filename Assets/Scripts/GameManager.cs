using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton pour accéder à l'instance de GameManager de partout

    [Header("Player Scores")]
    public int coins; // Nombre de pièces collectées par le joueur
    public int score; // Score total du joueur

    private int initialCoins; // Initialisation du compteur de pièces pour les comparaisons
    private int initialScore; // Initialisation du compteur de score pour les comparaisons

    [SerializeField] private Text coinsText; // UI pour afficher les pièces
    [SerializeField] private Text scoreText; // UI pour afficher le score

    [Header("Defeat UI")]
    [SerializeField] private Text defeatCoinsText; // Texte pour les pièces sur l'écran de défaite
    [SerializeField] private Text defeatScoreText; // Texte pour le score sur l'écran de défaite
    [SerializeField] private GameObject defeatPanel; // Panneau UI montré lors de la défaite

    [Header("Time Over UI")]
    [SerializeField] private Text timeOverCoinsText; // Texte pour les pièces sur l'écran de temps écoulé
    [SerializeField] private Text timeOverScoreText; // Texte pour le score sur l'écran de temps écoulé
    [SerializeField] private GameObject timeOverPanel; // Panneau UI montré quand le temps est écoulé

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
        FindObjectOfType<SaveSystem>().LoadData(); // Chargement des données du joueur
        initialCoins = coins; // Définit les pièces initiales après le chargement
        initialScore = score; // Définit le score initial après le chargement
        UpdateCoinsUI(); // Mise à jour de l'affichage des pièces
        UpdateScoreUI(); // Mise à jour de l'affichage du score
    }

    public void UpdateCoinsUI()
    {
        coinsText.text = coins.ToString(); // Mise à jour de l'UI des pièces
    }

    public void UpdateScoreUI()
    {
        scoreText.text = score.ToString(); // Mise à jour de l'UI du score
    }

    public void ShowDefeatPanel()
    {
        UpdateFinalPanel(defeatCoinsText, defeatScoreText, defeatPanel); // Mise à jour du panneau de défaite
    }

    public void ShowTimeOverPanel()
    {
        UpdateFinalPanel(timeOverCoinsText, timeOverScoreText, timeOverPanel); // Mise à jour du panneau de temps écoulé
    }

    private void UpdateFinalPanel(Text coinsText, Text scoreText, GameObject panel)
    {
        // Réinitialise les effets de rage
        RageManager rageManager = FindObjectOfType<RageManager>();
        if (rageManager != null)
        {
            rageManager.ResetRageEffects();
        }

        coinsText.text = $"{initialCoins} (+{coins - initialCoins})"; // Format de l'affichage des pièces
        scoreText.text = $"{initialScore} (+{score - initialScore})"; // Format de l'affichage du score

        FindObjectOfType<SaveSystem>().SaveData();

        initialCoins = coins; // Met à jour les pièces initiales après la sauvegarde
        initialScore = score; // Met à jour le score initial après la sauvegarde

        panel.SetActive(true);
        Time.timeScale = 0; // Arrête le temps pour afficher le panneau
    }

    public void ResetScoresToDefault()
    {
        coins = 0; // Réinitialise les pièces à 0
        score = 0; // Réinitialise le score à 0
        initialCoins = 0; // Réinitialise les pièces initiales à 0
        initialScore = 0; // Réinitialise le score initial à 0

        UpdateCoinsUI(); // Mise à jour de l'UI des pièces
        UpdateScoreUI(); // Mise à jour de l'UI du score
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Restaure le temps normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharge la scène actuelle
    }
}