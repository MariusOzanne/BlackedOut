using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Statistiques de l'ennemi")]
    public EnemyData enemyData;
    [Header("Zone de d�tection de l'ennemi")]
    [SerializeField] private GameObject detectionZone; // Zone de d�tection de d�placement
    [SerializeField] private GameObject attackZone; // Zone de d�tection d'attaque

    [Header("Prefab de la pi�ce")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Variables unique du slime")]
    [SerializeField] private float duplicationRadius = 0.25f;
    [SerializeField] private int numberOfDuplicates = 2;
    [SerializeField] private float sizeReductionFactor = 2f;
    private bool hasDuplicated = false;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private float nextAttackTime;
    private bool playerInAttackRange;
    private Animation anim;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = enemyData.speed;
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        if (isDead)
            return;

        // Si le joueur est dans la zone de d�tection de d�placement
        if (detectionZone.GetComponent<Collider>().bounds.Contains(player.position))
        {
            if (!playerInAttackRange)
                // Jouer l'animation de d�placement
                anim.CrossFade("move_forward");

            // Si le joueur est dans la zone d'attaque
            if (attackZone.GetComponent<Collider>().bounds.Contains(player.position))
            {
                // Arr�te le mouvement de l'ennemi
                navMeshAgent.isStopped = true;
                if (!playerInAttackRange)
                {
                    // Jouer l'animation d'attaque
                    anim.CrossFade("attack_short_001");
                    playerInAttackRange = true;
                }

                // Attaque
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + enemyData.attackCooldown;
                }
            }
            else
            {
                // Reprend le mouvement vers le joueur
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(player.position);
                playerInAttackRange = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si le joueur entre dans la zone de d�tection d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si le joueur quitte la zone de d�tection d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = false;
        }
    }

    void Attack()
    {
        // Appliquer les d�g�ts au joueur
        GameManager.Instance.TakeDamage(enemyData.damage);
    }

    public void SetEnemyData(EnemyData data)
    {
        enemyData = data;
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
            else
            {
                Debug.LogError("NavMeshAgent non trouv� sur l'ennemi");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        enemyData.health -= damage;
        if (enemyData.health <= 0 && !isDead)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        navMeshAgent.enabled = false;
        // Jouer l'animation de mort
        anim.Play("dead");

        // Gestion des pi�ces � l�cher
        int coinsToDrop = Random.Range(enemyData.minCoins, enemyData.maxCoins + 1);
        for (int i = 0; i < coinsToDrop; i++)
        {
            // Position al�atoire autour de l'ennemi
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 2;
            spawnPosition.y = 1;

            // Appliquer une rotation initiale de -90 degr�s autour de l'axe X et cr�er une rotation initiale autour de l'axe Y
            Quaternion spawnRotation = Quaternion.Euler(-90, Random.Range(0, 360), 0);
            Instantiate(coinPrefab, spawnPosition, spawnRotation);
        }

        // Incr�mentation du score
        GameManager.Instance.score += enemyData.scoreValue;
        GameManager.Instance.UpdateScore();

        // Si c'est un slime original
        if (!hasDuplicated)
        {
            // Duplication en Slime
            if (gameObject.CompareTag("Enemy") && (enemyData.enemyName == "Slime" || enemyData.enemyName == "KingSlime"))
            {
                DuplicateSlime();
            }

            // Marquer que le slime a �t� dupliqu�
            hasDuplicated = true;
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void DuplicateSlime()
    {
        // Ne duplique que si ce n'est pas d�j� fait
        if (!hasDuplicated)
        {
            float angleIncrement = 360f / numberOfDuplicates;
            float initialAngleOffset = Random.Range(0f, 360f); // Offset al�atoire pour l'angle initial

            for (int i = 0; i < numberOfDuplicates; i++)
            {
                float angle = (i * angleIncrement + initialAngleOffset) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angle) * duplicationRadius, 0f, Mathf.Sin(angle) * duplicationRadius);
                GameObject duplicate = Instantiate(enemyData.enemyPrefab, transform.position + offset, Quaternion.identity);
                duplicate.transform.localScale /= sizeReductionFactor;

                // Ajustement de l'intensit� de la lumi�re
                Light duplicateLight = duplicate.GetComponentInChildren<Light>();
                if (duplicateLight != null)
                {
                    duplicateLight.intensity = 0.5f; // Modifier l'intensit� de la lumi�re, vous pouvez ajuster selon vos besoins
                }

                // Appliquer les donn�es du Slime au duplicate
                EnemyController enemyController = duplicate.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    // Cloner l'EnemyData avant de l'appliquer au duplicate
                    EnemyData duplicateEnemyData = enemyData.Clone();
                    enemyController.SetEnemyData(duplicateEnemyData);
                    enemyController.hasDuplicated = true; // Marquer que le duplicate ne peut pas dupliquer davantage
                }
                else
                {
                    Debug.LogError("EnemyController non trouv� sur le duplicate du Slime");
                }
            }

            // Marquer que le slime a �t� dupliqu�
            hasDuplicated = true;
        }
    }
}
