using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Stats de l'ennemi")]
    public EnemyData enemyData;
    [Header("Zone de detection de l'ennemi")]
    [SerializeField] private GameObject detectionZone; // Zone de d�tection de d�placement
    [SerializeField] private GameObject attackZone; // Zone de d�tection d'attaque

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
        GameManager.Instance.life -= enemyData.damage;

        // V�rifier si le joueur a perdu toute sa vie
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

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
