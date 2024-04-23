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
    [Range(0, 100)] public int health;
    [Range(0, 100)] public int shield;
    public float speed;
    public int damage;

    [Header("Score du joueur")]
    public int coins;
    public int score;

    // Variables pour sauvegarder les valeurs de début de partie
    private int initialCoins;
    private int initialScore;

    [SerializeField] private Text coinsText;
    [SerializeField] private Text scoreText;

    // Références pour les Texts dans les panels
    [Header("Textes pour le panneau de défaite")]
    [SerializeField] private Text coinsTextDefeat;
    [SerializeField] private Text scoreTextDefeat;

    [Header("Textes pour le panneau du temps écoulé")]
    [SerializeField] private Text coinsTextTimeOver;
    [SerializeField] private Text scoreTextTimeOver;

    [Header("Panels du joueur")]
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject timeOverPanel;

    [Header("Interface utilisateur de l'effet de rage")]
    [SerializeField] private Text rageEffectText;

    private Coroutine rageCoroutine;
    private float defaultSpeed;
    private int defaultDamage;

    // On creee le patern singleton
    // Permettant d'acceder a un script a partir d'un autre script
    private void Awake()
    {
        // Si l'instance n'a pas ete attribue
        if (_Instance == null)
        {
            _Instance = this;   // La variable _Instance = la classe GameManager
            // DontDestroyOnLoad(gameObject);   // Garde le GameManager persistant entre les scènes
        }
        // Sinon
        else
        {
            Destroy(gameObject);      // On la detruit si y'en a trop (car un singleton doit etre unique)
        }

        defaultSpeed = speed;
        defaultDamage = damage;
    }

    private void Start()
    {
        LoadPlayerData(); // Charge les données au démarrage
        UpdateCoinCount();
        UpdateScore();
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        score = PlayerPrefs.GetInt("PlayerScore", 0);
        coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        initialScore = score;
        initialCoins = coins;
    }

    public void ResetPlayerData()
    {
        PlayerPrefs.DeleteKey("PlayerScore");
        PlayerPrefs.DeleteKey("PlayerCoins");

        score = 0;
        coins = 0;

        initialScore = 0;
        initialCoins = 0;

        UpdateCoinCount();
        UpdateScore();

        SavePlayerData();
    }

    public void AddShield(int amount)
    {
        shield += amount;
        shield = Mathf.Min(shield, health);
    }

    public void TakeDamage(int amount)
    {
        if (shield > 0)
        {
            int damageToShield = Mathf.Min(amount, shield);
            shield -= damageToShield;
            amount -= damageToShield;
        }

        if (amount > 0)
        {
            health -= amount;
        }

        CheckPlayerDefeat();
    }

    public void CheckPlayerDefeat()
    {
        if (health <= 0)
        {
            SavePlayerData(); // Sauvegarde les données avant de montrer le panneau de défaite
            ShowDefeatPanel();
        }
    }

    public void UpdateCoinCount()
    {
        coinsText.text = coins.ToString();
    }

    public void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    private void ShowDefeatPanel()
    {
        UpdateFinalPanel(coinsTextDefeat, scoreTextDefeat, defeatPanel);
    }

    public void ShowTimeOverPanel()
    {
        UpdateFinalPanel(coinsTextTimeOver, scoreTextTimeOver, timeOverPanel);
    }

    private void UpdateFinalPanel(Text coinsFinalText, Text scoreFinalText, GameObject panel)
    {
        // Mise à jour du panel de fin avec les scores et pièces cumulés
        coinsFinalText.text = $"Pièces : {initialCoins} (+{coins - initialCoins})";
        scoreFinalText.text = $"Score : {initialScore} (+{score - initialScore})";

        // Sauvegarde les nouvelles valeurs
        SavePlayerData();
        
        panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActivateRageMode(ItemsData itemData)
    {
        if (rageCoroutine != null)
        {
            StopCoroutine(rageCoroutine);
            ResetRageEffects(); // Reset les effets pour éviter de les cumuler
        }

        rageCoroutine = StartCoroutine(RageMode(itemData.effectDuration));
    }

    private IEnumerator RageMode(float duration)
    {
        ApplyRageEffects();

        float timeLeft = duration;
        
        while (timeLeft > 0)
        {
            UpdateRageEffectUI(timeLeft);
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        ResetRageEffects();
        UpdateRageEffectUI(0);
    }

    private void ApplyRageEffects()
    {
        speed = defaultSpeed * 1.5f;
        damage = defaultDamage + 20;
    }

    private void ResetRageEffects()
    {
        speed = defaultSpeed;
        damage = defaultDamage;
    }

    private void UpdateRageEffectUI(float timeLeft)
    {
        if (timeLeft > 0)
        {
            rageEffectText.text = "Rage : " + timeLeft.ToString("F2") + "s";
            rageEffectText.gameObject.SetActive(true);
        }
        else
        {
            rageEffectText.gameObject.SetActive(false);
        }
    }
}