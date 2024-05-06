using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    public EnemyData enemyData; // Donn�es sp�cifiques � l'ennemi comme la sant�, la vitesse, etc.

    [Header("Detection Zones")]
    [SerializeField] private GameObject detectionZone; // Zone de d�tection de pr�sence du joueur
    [SerializeField] private GameObject attackZone; // Zone d'attaque o� l'ennemi peut effectuer une attaque

    [Header("Loot")]
    [SerializeField] private GameObject coinPrefab; // Pr�fabriqu� pour la pi�ce de monnaie � larguer � la mort

    [Header("Slime Specific")]
    [SerializeField] private float duplicationRadius; // Rayon pour la duplication des slimes
    [SerializeField] private int numberOfDuplicates; // Nombre de duplications
    [SerializeField] private float sizeReductionFactor; // Facteur de r�duction de taille pour chaque duplication
    private bool hasDuplicated = false; // Contr�le si la duplication a d�j� eu lieu

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
        player = GameObject.FindGameObjectWithTag("Player").transform; // R�cup�ration de la r�f�rence du joueur
        playerController = player.GetComponent<PlayerController>();
        navMeshAgent = GetComponent<NavMeshAgent>(); // Acc�s au composant de navigation mesh
        navMeshAgent.speed = enemyData.speed; // D�finition de la vitesse de l'agent
        animator = GetComponent<Animation>(); // Acc�s au composant d'animation
        health = GetComponent<Health>();

        // Initialise la sant� maximale et actuelle de l'ennemi en fonction des donn�es de l'ennemi
        health.maxHealth = enemyData.maxHealth;
        health.currentHealth = enemyData.health;
        health.healthBar.SetMaxHealth(enemyData.maxHealth);
    }

    private void Update()
    {
        if (isDead) // Arr�te la mise � jour si l'ennemi est mort
        {
            return;
        }

        MoveAndDetectPlayer(); // Fonction de d�placement et de d�tection du joueur
    }

    private void OnTriggerEnter(Collider other)
    {
        // V�rifie si le joueur entre dans la zone d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // V�rifie si le joueur sort de la zone d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = false;
        }
    }

    private void MoveAndDetectPlayer()
    {
        // V�rifie si le joueur est dans la zone de d�tection
        if (detectionZone.GetComponent<Collider>().bounds.Contains(player.position))
        {
            // Lance l'animation de mouvement si le joueur n'est pas en port�e d'attaque
            if (!playerInAttackRange)
            {
                animator.CrossFade("move_forward");
            }

            // V�rifie si le joueur est dans la zone d'attaque
            if (attackZone.GetComponent<Collider>().bounds.Contains(player.position))
            {
                StopAndAttack(); // Fonction pour stopper et attaquer
            }
            else
            {
                navMeshAgent.isStopped = false; // L'agent de navigation continue de se d�placer
                navMeshAgent.SetDestination(player.position); // D�finit la destination de l'agent vers le joueur
                playerInAttackRange = false; // Le joueur n'est plus consid�r� en port�e d'attaque
            }
        }
    }

    private void StopAndAttack()
    {
        navMeshAgent.isStopped = true; // Arr�te le mouvement de l'agent
        
        if (!playerInAttackRange)
        {
            animator.CrossFade("attack_short_001"); // Lance l'animation d'attaque
            playerInAttackRange = true; // Le joueur est maintenant en port�e d'attaque
        }

        if (Time.time >= nextAttackTime) // V�rifie si le temps est �coul� pour attaquer � nouveau
        {
            Attack(); // Ex�cute une attaque
            nextAttackTime = Time.time + enemyData.attackCooldownTime; // R�initialise le temps pour la prochaine attaque
        }
    }

    private void Attack()
    {
        if (playerController != null)
        {
            playerController.TakeDamage(enemyData.damage); // Applique des d�g�ts au joueur
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        navMeshAgent.enabled = false;
        animator.Play("dead"); // Joue l'animation de mort

        // D�sactive tous les colliders sur l'ennemi pour �viter des interactions inutiles
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
        GameManager.Instance.UpdateScoreUI(); // Met � jour l'interface utilisateur du score

        // Duplique l'ennemi s'il s'agit d'un slime et n'a pas encore �t� dupliqu�
        if (!hasDuplicated && (enemyData.name == "Slime" || enemyData.name == "KingSlime"))
        {
            DuplicateSlime();
            hasDuplicated = true;
        }

        yield return new WaitForSeconds(2f); // Attend 2 secondes
        Destroy(gameObject); // D�truit l'objet ennemi
    }

    private void DropLoot()
    {
        // D�termine le nombre de pi�ces � larguer
        int coinsToDrop = Random.Range(enemyData.minCoins, enemyData.maxCoins + 1);

        // Cr�e chaque pi�ce � une position al�atoire autour de l'ennemi
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
            duplicate.transform.localScale /= sizeReductionFactor; // R�duit la taille du duplicata

            // Configurer les donn�es de l'ennemi duplicata et marquer comme d�j� dupliqu�
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
        if (isDead) // Ignore les d�g�ts si l'ennemi est d�j� mort
        {
            return;
        }

        health.TakeDamage(damage); // R�duit la sant� de l'ennemi

        if (health.currentHealth <= 0 && !isDead) // V�rifie si l'ennemi doit mourir
        {
            StartCoroutine(Die()); // D�clenche la coroutine de mort
        }
    }

    public void SetEnemyData(EnemyData data)
    {
        enemyData = data; // Configure les donn�es de l'ennemi

        // S'assure que l'agent de navigation est configur� correctement
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