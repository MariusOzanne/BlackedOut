using System.Collections;
using System.Collections.Generic;
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
    //private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float nextAttackTime;
    private bool playerInAttackRange;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        navMeshAgent.speed = enemyData.speed;
        playerInAttackRange = false;
    }

    void Update()
    {
        // Si le joueur est dans la zone de détection de déplacement, se dirige vers le joueur

        if (detectionZone.GetComponent<Collider>().bounds.Contains(player.position))
        {
            navMeshAgent.SetDestination(player.position);
            //animator.SetBool("isWalking", true);
        }
        //else
        //{
        //    animator.SetBool("isWalking", false); 
        //}

        // Si le joueur est dans la zone d'attaque et l'ennemi peut attaquer
        if (attackZone.GetComponent<Collider>().bounds.Contains(player.position) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + enemyData.attackCooldown;
        }

        // Si le joueur n'est pas dans la zone d'attaque mais dans la zone de déplacement, se rapproche du joueur
        if (!playerInAttackRange && detectionZone.GetComponent<Collider>().bounds.Contains(player.position))
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si le joueur entre dans la zone de détection d'attaque
        if (other.CompareTag("Player") && other.gameObject == attackZone)
        {
            playerInAttackRange = true;
            //animator.SetTrigger("Attack");
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
        GameManager.Instance.life -= enemyData.damage;
    }

    public void TakeDamage()
    {
        
        //enemyData.health -= ;
        if (enemyData.health <= 0)
        {
            //animator.SetTrigger("Die");
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
