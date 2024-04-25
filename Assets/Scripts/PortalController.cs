using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    #region Variable setup

    [Header("Choix des ennemis apparaissant du portail")]
    [SerializeField] private List<EnemyData> enemyTypes;
    [Header("Vie du portail")]
    public float portalHealth;
    [Header("Setup des vagues")]
    [SerializeField] private int enemiesPerWave; // Nombre d'ennemis par vague
    [SerializeField] private float spawnInterval; // Intervalle entre les apparitions d'ennemis
    [SerializeField] private float waveInterval; // Intervalle entre les vagues
    
    #endregion
    
    public float currentHealth;
    
    private float nextWaveTime;
    private int enemiesSpawned;

    public AudioClip portalAudio;
    private AudioSource PortalSource;

    void Start()
    {
        currentHealth = portalHealth;
        nextWaveTime = Time.time + waveInterval;

        PortalSource = gameObject.AddComponent<AudioSource>();
        PortalSource.loop = true;
        PortalSource.clip = portalAudio;
        PortalSource.playOnAwake = true;
        PortalSource.volume = 0.3f;
        PortalSource.Play();
    }

    void Update()
    {
        if (Time.time >= nextWaveTime)
        {
            StartCoroutine(SpawnWave());
            nextWaveTime = Time.time + waveInterval;
        }
    }

    IEnumerator SpawnWave()
    {
        enemiesSpawned = 0;
        while (enemiesSpawned < enemiesPerWave)
        {
            // Choix aléatoire de type d'ennemi dans la liste enemyTypes
            int randomIndex = Random.Range(0, enemyTypes.Count);
            EnemyData originalData = enemyTypes[randomIndex];

            // Cloner les données d'ennemi pour chaque nouveau spawn
            EnemyData clonedData = originalData.Clone();

            // Faire apparaître l'ennemi correspondant au type choisi
            GameObject enemyPrefab = originalData.enemyPrefab;
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // Assigner les données clonées à l'ennemi
            EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetEnemyData(clonedData);
            }

            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            DestroyPortal();
        }
    }

    private void DestroyPortal()
    {
        GameManager.Instance.score += 500; // Incrémenter le score
        GameManager.Instance.UpdateScore(); // Mettre à jour le score dans l'UI

        PortalSource.Stop();
        Destroy(gameObject); // Détruire le portail
    }
}
