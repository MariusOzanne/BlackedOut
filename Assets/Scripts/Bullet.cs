using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage;

    private float fixedYPosition;

    private void Start()
    {
        fixedYPosition = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Portal"))
        {
            PortalController portal = other.GetComponent<PortalController>();
            if (portal != null)
            {
                portal.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}