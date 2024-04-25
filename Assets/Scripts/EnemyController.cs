using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Stats de l'ennemi")]
    public EnemyData enemyData;
    [Header("Zone de detection de l'ennemi")]
    [SerializeField] private GameObject detectionZone; // Zone de détection de déplacement
    [SerializeField] private GameObject attackZone; // Zone de détection d'attaque

    [Header("Prefab de la pièce")]
    [SerializeField] private GameObject coinPrefab;

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

        // Si le joueur est dans la zone de détection de déplacement
        if (detectionZone.GetComponent<Collider>().bounds.Contains(player.position))
        {
            if (!playerInAttackRange)
                // Jouer l'animation de déplacement
                anim.CrossFade("move_forward");

            // Si le joueur est dans la zone d'attaque
            if (attackZone.GetComponent<Collider>().bounds.Contains(player.position))
            {
                // Arrête le mouvement de l'ennemi
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
        // Si le joueur entre dans la zone de détection d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si le joueur quitte la zone de détection d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = false;
        }
    }

    void Attack()
    {
        // Appliquer les dégâts au joueur
        GameManager.Instance.life -= enemyData.damage;

        // Vérifier si le joueur a perdu toute sa vie
        GameManager.Instance.CheckPlayerDefeat();
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
                Debug.LogError("NavMeshAgent non trouvé sur l'ennemi");
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

        // Gestion des pièces à lâcher
        int coinsToDrop = Random.Range(enemyData.minCoins, enemyData.maxCoins + 1);
        for (int i = 0; i < coinsToDrop; i++)
        {
            // Position aléatoire autour de l'ennemi
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 2;
            spawnPosition.y = 1;

            // Appliquer une rotation initiale de -90 degrés autour de l'axe X et créer une rotation initiale autour de l'axe Y
            Quaternion spawnRotation = Quaternion.Euler(-90, Random.Range(0, 360), 0);
            Instantiate(coinPrefab, spawnPosition, spawnRotation);
        }

        // Incrémentation du score
        GameManager.Instance.score += enemyData.scoreValue;
        GameManager.Instance.UpdateScore();

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}