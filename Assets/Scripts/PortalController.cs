using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [Header("Enemy Types")]
    [SerializeField] private List<EnemyData> enemyTypes; // Liste des types d'ennemis que le portail peut générer

    [Header("Wave Settings")]
    [SerializeField] private int enemiesPerWave; // Nombre d'ennemis par vague
    [SerializeField] private float spawnInterval; // Intervalle entre les apparitions des ennemis
    [SerializeField] private float waveInterval; // Intervalle entre les vagues

    private float nextWaveTime; // Temps pour la prochaine vague
    private int enemiesSpawned; // Nombre d'ennemis déjà apparus
    private Health health; // Composant pour gérer la santé du portail

    private void Start()
    {
        nextWaveTime = Time.time + waveInterval; // Initialisation du temps pour la prochaine vague
        health = GetComponent<Health>(); // Récupération du composant Health
    }

    private void Update()
    {
        if (health.currentHealth <= 0)
        {
            return;
        }

        // Vérifie si le temps pour la prochaine vague est atteint
        if (Time.time >= nextWaveTime)
        {
            StartCoroutine(SpawnWave()); // Démarre la coroutine pour générer une vague
            nextWaveTime = Time.time + waveInterval; // Réinitialise le temps pour la prochaine vague
        }
    }

    private IEnumerator SpawnWave()
    {
        enemiesSpawned = 0;

        // Continue de générer des ennemis jusqu'à atteindre le nombre requis par vague
        while (enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // Attend entre chaque apparition
        }
    }

    private void SpawnEnemy()
    {
        // Sélection aléatoire d'un type d'ennemi à générer
        int randomIndex = Random.Range(0, enemyTypes.Count);
        EnemyData originalData = enemyTypes[randomIndex];

        EnemyData clonedData = originalData.Clone(); // Clone les données pour éviter les références partagées

        GameObject enemyPrefab = originalData.prefab;
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity); // Création de l'ennemi

        // Configuration de l'ennemi généré
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetEnemyData(clonedData);
        }

        enemiesSpawned++; // Incrémente le compteur d'ennemis générés
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
        
        if (health.currentHealth <= 0)
        {
            DestroyPortal(); // Détruit le portail si sa santé tombe à zéro
        }
    }

    private void DestroyPortal()
    {
        GameManager.Instance.score += 500; // Augmente le score du joueur
        GameManager.Instance.UpdateScoreUI(); // Met à jour l'UI du score
        Destroy(gameObject); // Détruit le portail
    }
}