using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton pour acc�der � l'instance de GameManager de partout

    [Header("Player Stats")]
    public float speed; // Vitesse de d�placement du joueur
    public int damage; // D�g�ts inflig�s par le joueur

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

    [Header("Rage Mode UI")]
    [SerializeField] private Text rageEffectText; // Texte pour afficher la dur�e du mode rage
    [SerializeField] private GameObject rageEffectUI; // UI pour le mode rage
    [SerializeField] private GameObject rageParticle; // Effets visuels pour le mode rage

    private Coroutine rageModeCoroutine; // Coroutine pour g�rer la dur�e du mode rage
    private float defaultSpeed; // Vitesse par d�faut pour restaurer apr�s le mode rage
    private int defaultDamage; // D�g�ts par d�faut pour restaurer apr�s le mode rage

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
        defaultDamage = damage; // Sauvegarde des d�g�ts initiaux
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

    public void ActivateRageMode(ItemData itemData)
    {
        if (rageModeCoroutine != null)
        {
            StopCoroutine(rageModeCoroutine);
            ResetRageEffects(); // R�initialisation des effets du mode rage pr�c�dent
        }

        rageModeCoroutine = StartCoroutine(RageEffectCoroutine(itemData)); // D�marrage d'une nouvelle coroutine pour le mode rage
    }

    private IEnumerator RageEffectCoroutine(ItemData itemData)
    {
        ApplyRageEffects(itemData); // Application des effets de rage

        float timeLeft = itemData.durationOfEffect; // Dur�e du mode rage

        while (timeLeft > 0)
        {
            UpdateRageEffectUI(timeLeft); // Mise � jour de l'UI du mode rage
            timeLeft -= Time.deltaTime; // D�compte du temps restant
            yield return null;
        }

        ResetRageEffects(); // R�initialisation des effets une fois le mode rage termin�
        UpdateRageEffectUI(0); // Mise � jour finale de l'UI du mode rage
    }

    private void ApplyRageEffects(ItemData itemData)
    {
        speed = defaultSpeed * itemData.speedMultiplier; // Augmentation de la vitesse
        damage = defaultDamage + itemData.additionalDamage; // Augmentation des d�g�ts
    }

    private void ResetRageEffects()
    {
        speed = defaultSpeed; // Restauration de la vitesse par d�faut
        damage = defaultDamage; // Restauration des d�g�ts par d�faut
    }

    private void UpdateRageEffectUI(float timeLeft)
    {
        rageEffectText.text = timeLeft.ToString("F0") + " s"; // Affichage du temps restant en secondes
        rageEffectUI.SetActive(timeLeft > 0); // Active l'UI si le mode rage est actif
        rageParticle.SetActive(timeLeft > 0); // Active les particules si le mode rage est actif
    }
}