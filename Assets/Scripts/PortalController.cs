using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [Header("Enemy Types")]
    [SerializeField] private List<EnemyData> enemyTypes; // Liste des types d'ennemis que le portail peut g�n�rer

    [Header("Wave Settings")]
    [SerializeField] private int enemiesPerWave; // Nombre d'ennemis par vague
    [SerializeField] private float spawnInterval; // Intervalle entre les apparitions des ennemis
    [SerializeField] private float waveInterval; // Intervalle entre les vagues

    private float nextWaveTime; // Temps pour la prochaine vague
    private int enemiesSpawned; // Nombre d'ennemis d�j� apparus
    private Health health; // Composant pour g�rer la sant� du portail

    private void Start()
    {
        nextWaveTime = Time.time + waveInterval; // Initialisation du temps pour la prochaine vague
        health = GetComponent<Health>(); // R�cup�ration du composant Health
    }

    private void Update()
    {
        if (health.currentHealth <= 0)
        {
            return;
        }

        // V�rifie si le temps pour la prochaine vague est atteint
        if (Time.time >= nextWaveTime)
        {
            StartCoroutine(SpawnWave()); // D�marre la coroutine pour g�n�rer une vague
            nextWaveTime = Time.time + waveInterval; // R�initialise le temps pour la prochaine vague
        }
    }

    private IEnumerator SpawnWave()
    {
        enemiesSpawned = 0;

        // Continue de g�n�rer des ennemis jusqu'� atteindre le nombre requis par vague
        while (enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Attend entre chaque apparition
        }
    }

    private void SpawnEnemy()
    {
        // S�lection al�atoire d'un type d'ennemi � g�n�rer
        int randomIndex = Random.Range(0, enemyTypes.Count);
        EnemyData originalData = enemyTypes[randomIndex];

        EnemyData clonedData = originalData.Clone(); // Clone les donn�es pour �viter les r�f�rences partag�es

        GameObject enemyPrefab = originalData.prefab;
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity); // Cr�ation de l'ennemi

        // Configuration de l'ennemi g�n�r�
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetEnemyData(clonedData);
        }

        enemiesSpawned++; // Incr�mente le compteur d'ennemis g�n�r�s
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
        
        if (health.currentHealth <= 0)
        {
            DestroyPortal(); // D�truit le portail si sa sant� tombe � z�ro
        }
    }

    private void DestroyPortal()
    {
        GameManager.Instance.score += 500; // Augmente le score du joueur
        GameManager.Instance.UpdateScoreUI(); // Met � jour l'UI du score
        Destroy(gameObject); // D�truit le portail
    }
}