using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    public EnemyData enemyData; // Données spécifiques à l'ennemi comme la santé, la vitesse, etc.

    [Header("Detection Zones")]
    [SerializeField] private GameObject detectionZone; // Zone de détection de présence du joueur
    [SerializeField] private GameObject attackZone; // Zone d'attaque où l'ennemi peut effectuer une attaque

    [Header("Loot")]
    [SerializeField] private GameObject coinPrefab; // Préfabriqué pour la pièce de monnaie à larguer à la mort

    [Header("Slime Specific")]
    [SerializeField] private float duplicationRadius; // Rayon pour la duplication des slimes
    [SerializeField] private int numberOfDuplicates; // Nombre de duplications
    [SerializeField] private float sizeReductionFactor; // Facteur de réduction de taille pour chaque duplication
    private bool hasDuplicated = false; // Contrôle si la duplication a déjà eu lieu

    private Transform player;
    private PlayerController playerController;
    private NavMeshAgent navMeshAgent;
    private float nextAttackTime;
    private bool playerInAttackRange;
    private Animation animator;
    private Health health;
    private bool isDead = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Récupération de la référence du joueur
        playerController = player.GetComponent<PlayerController>();
        navMeshAgent = GetComponent<NavMeshAgent>(); // Accès au composant de navigation mesh
        navMeshAgent.speed = enemyData.speed; // Définition de la vitesse de l'agent
        animator = GetComponent<Animation>(); // Accès au composant d'animation
        health = GetComponent<Health>();

        // Initialise la santé maximale et actuelle de l'ennemi en fonction des données de l'ennemi
        health.maxHealth = enemyData.maxHealth;
        health.currentHealth = enemyData.health;
        health.healthBar.SetMaxHealth(enemyData.maxHealth);
    }

    private void Update()
    {
        if (isDead) // Arrête la mise à jour si l'ennemi est mort
        {
            return;
        }

        MoveAndDetectPlayer(); // Fonction de déplacement et de détection du joueur
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le joueur entre dans la zone d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Vérifie si le joueur sort de la zone d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = false;
        }
    }

    private void MoveAndDetectPlayer()
    {
        // Vérifie si le joueur est dans la zone de détection
        if (detectionZone.GetComponent<Collider>().bounds.Contains(player.position))
        {
            // Lance l'animation de mouvement si le joueur n'est pas en portée d'attaque
            if (!playerInAttackRange)
            {
                animator.CrossFade("move_forward");
            }

            // Vérifie si le joueur est dans la zone d'attaque
            if (attackZone.GetComponent<Collider>().bounds.Contains(player.position))
            {
                StopAndAttack(); // Fonction pour stopper et attaquer
            }
            else
            {
                navMeshAgent.isStopped = false; // L'agent de navigation continue de se déplacer
                navMeshAgent.SetDestination(player.position); // Définit la destination de l'agent vers le joueur
                playerInAttackRange = false; // Le joueur n'est plus considéré en portée d'attaque
            }
        }
    }

    private void StopAndAttack()
    {
        navMeshAgent.isStopped = true; // Arrête le mouvement de l'agent
        
        if (!playerInAttackRange)
        {
            animator.CrossFade("attack_short_001"); // Lance l'animation d'attaque
            playerInAttackRange = true; // Le joueur est maintenant en portée d'attaque
        }

        if (Time.time >= nextAttackTime) // Vérifie si le temps est écoulé pour attaquer à nouveau
        {
            Attack(); // Exécute une attaque
            nextAttackTime = Time.time + enemyData.attackCooldownTime; // Réinitialise le temps pour la prochaine attaque
        }
    }

    private void Attack()
    {
        if (playerController != null)
        {
            playerController.TakeDamage(enemyData.damage); // Applique des dégâts au joueur
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        navMeshAgent.enabled = false;
        animator.Play("dead"); // Joue l'animation de mort

        // Désactive tous les colliders sur l'ennemi pour éviter des interactions inutiles
        foreach (var collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        DropLoot(); // Largue des objets
        GameManager.Instance.score += enemyData.scoreValue; // Augmente le score du joueur
        GameManager.Instance.UpdateScoreUI(); // Met à jour l'interface utilisateur du score

        // Duplique l'ennemi s'il s'agit d'un slime et n'a pas encore été dupliqué
        if (!hasDuplicated && (enemyData.name == "Slime" || enemyData.name == "KingSlime"))
        {
            DuplicateSlime();
            hasDuplicated = true;
        }

        yield return new WaitForSeconds(2f); // Attend 2 secondes
        Destroy(gameObject); // Détruit l'objet ennemi
    }

    private void DropLoot()
    {
        // Détermine le nombre de pièces à larguer
        int coinsToDrop = Random.Range(enemyData.minCoins, enemyData.maxCoins + 1);

        // Crée chaque pièce à une position aléatoire autour de l'ennemi
        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 2;
            spawnPosition.y = 1;

            Quaternion spawnRotation = Quaternion.Euler(-90, Random.Range(0, 360), 0);
            Instantiate(coinPrefab, spawnPosition, spawnRotation);
        }
    }

    private void DuplicateSlime()
    {
        // Calcule l'angle et la position pour chaque duplication
        float angleIncrement = 360f / numberOfDuplicates;
        float initialAngleOffset = Random.Range(0f, 360f);

        for (int i = 0; i < numberOfDuplicates; i++)
        {
            float angle = (i * angleIncrement + initialAngleOffset) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle) * duplicationRadius, 0f, Mathf.Sin(angle) * duplicationRadius);
            GameObject duplicate = Instantiate(enemyData.prefab, transform.position + offset, Quaternion.identity);
            duplicate.transform.localScale /= sizeReductionFactor; // Réduit la taille du duplicata

            // Configurer les données de l'ennemi duplicata et marquer comme déjà dupliqué
            EnemyController enemyController = duplicate.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                EnemyData duplicateEnemyData = enemyData.Clone();
                enemyController.SetEnemyData(duplicateEnemyData);
                enemyController.hasDuplicated = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) // Ignore les dégâts si l'ennemi est déjà mort
        {
            return;
        }

        health.TakeDamage(damage); // Réduit la santé de l'ennemi

        if (health.currentHealth <= 0 && !isDead) // Vérifie si l'ennemi doit mourir
        {
            StartCoroutine(Die()); // Déclenche la coroutine de mort
        }
    }

    public void SetEnemyData(EnemyData data)
    {
        enemyData = data; // Configure les données de l'ennemi

        // S'assure que l'agent de navigation est configuré correctement
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = enemyData.speed;
        }
        else
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = enemyData.speed;
            }
        }
    }
}