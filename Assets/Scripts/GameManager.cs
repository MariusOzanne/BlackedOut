using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton pour accéder à l'instance de GameManager de partout

    [Header("Player Stats")]
    public float speed; // Vitesse de déplacement du joueur
    public int damage; // Dégâts infligés par le joueur

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

    [Header("Rage Mode UI")]
    [SerializeField] private Text rageEffectText; // Texte pour afficher la durée du mode rage
    [SerializeField] private GameObject rageEffectUI; // UI pour le mode rage
    [SerializeField] private GameObject rageParticle; // Effets visuels pour le mode rage

    private Coroutine rageModeCoroutine; // Coroutine pour gérer la durée du mode rage
    private float defaultSpeed; // Vitesse par défaut pour restaurer après le mode rage
    private int defaultDamage; // Dégâts par défaut pour restaurer après le mode rage

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

        defaultSpeed = speed; // Sauvegarde de la vitesse initiale
        defaultDamage = damage; // Sauvegarde des dégâts initiaux
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

    public void ActivateRageMode(ItemData itemData)
    {
        if (rageModeCoroutine != null)
        {
            StopCoroutine(rageModeCoroutine);
            ResetRageEffects(); // Réinitialisation des effets du mode rage précédent
        }

        rageModeCoroutine = StartCoroutine(RageEffectCoroutine(itemData)); // Démarrage d'une nouvelle coroutine pour le mode rage
    }

    private IEnumerator RageEffectCoroutine(ItemData itemData)
    {
        ApplyRageEffects(itemData); // Application des effets de rage

        float timeLeft = itemData.durationOfEffect; // Durée du mode rage

        while (timeLeft > 0)
        {
            UpdateRageEffectUI(timeLeft); // Mise à jour de l'UI du mode rage
            timeLeft -= Time.deltaTime; // Décompte du temps restant
            yield return null;
        }

        ResetRageEffects(); // Réinitialisation des effets une fois le mode rage terminé
        UpdateRageEffectUI(0); // Mise à jour finale de l'UI du mode rage
    }

    private void ApplyRageEffects(ItemData itemData)
    {
        speed = defaultSpeed * itemData.speedMultiplier; // Augmentation de la vitesse
        damage = defaultDamage + itemData.additionalDamage; // Augmentation des dégâts
    }

    private void ResetRageEffects()
    {
        speed = defaultSpeed; // Restauration de la vitesse par défaut
        damage = defaultDamage; // Restauration des dégâts par défaut
    }

    private void UpdateRageEffectUI(float timeLeft)
    {
        rageEffectText.text = timeLeft.ToString("F0") + " s"; // Affichage du temps restant en secondes
        rageEffectUI.SetActive(timeLeft > 0); // Active l'UI si le mode rage est actif
        rageParticle.SetActive(timeLeft > 0); // Active les particules si le mode rage est actif
    }
}