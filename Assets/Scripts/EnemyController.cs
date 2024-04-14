using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Stats de l'ennemi")]
    [SerializeField] private EnemyData enemyData;
    [Header("Zone de detection de l'ennemi")]
    [SerializeField] private GameObject detectionZone; // Zone de détection de déplacement
    [SerializeField] private GameObject attackZone; // Zone de détection d'attaque

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private float nextAttackTime;
    private bool playerInAttackRange;
    private Animation anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = enemyData.speed;
        playerInAttackRange = false;
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        // Si le joueur est dans la zone de détection de déplacement
        if (detectionZone.GetComponent<Collider>().bounds.Contains(player.position))
        {
            // Jouer l'animation de déplacement
            anim.CrossFade("move_forward");

            // Si le joueur est dans la zone d'attaque
            if (attackZone.GetComponent<Collider>().bounds.Contains(player.position))
            {
                // Arrête le mouvement de l'ennemi
                navMeshAgent.isStopped = true;
                // Jouer l'animation d'attaque
                anim.CrossFade("attack_short_001");
                anim.Stop("move_forward");

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

                // Jouer l'animation de déplacement
                anim.CrossFade("move_forward");
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
    }

    public void TakeDamage()
    {
        // Degats des balles a faire ici
        //enemyData.health -= ;
        if (enemyData.health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        // Jouer l'animation de mort
        anim.Stop("attack_short_001");
        anim.Stop("move_forward");
        anim.CrossFade("dead");

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
