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

    [SerializeField] private Text coinsText;
    [SerializeField] private Text scoreText;
    
    [Header("Panel du joueur")]
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject timeOverPanel;

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

    public void AddShield(int amount)
    {
        shield += amount;
        shield = Mathf.Min(shield, health);
    }

    public void ActivateRageMode()
    {
        speed *= 1.5f;
        damage += 20;

        StartCoroutine(ResetRageMode());
    }

    private IEnumerator ResetRageMode()
    {
        yield return new WaitForSeconds(10);
        speed /= 1.5f;
        damage -= 20;
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
        defeatPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowTimeOverPanel()
    {
        timeOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}