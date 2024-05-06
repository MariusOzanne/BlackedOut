using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float fixedYPosition; // Position Y fixe pour que la balle se d�place seulement en X et Z

    private void Start()
    {
        fixedYPosition = transform.position.y; // Sauvegarde de la position Y initiale au d�marrage
    }

    private void Update()
    {
        // Maintien de la balle � une hauteur constante pendant son d�placement
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // V�rification si la balle entre en collision avec un ennemi
        if (other.CompareTag("Enemy"))
        {
            // Tentative de r�cup�ration du contr�leur de l'ennemi pour infliger des d�g�ts
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(GameManager.Instance.damage); // Appel de la m�thode de d�g�t sur l'ennemi
                Destroy(gameObject); // Destruction de la balle apr�s l'impact
            }
        }

        // V�rification si la balle entre en collision avec un portail
        if (other.CompareTag("Portal"))
        {
            // Tentative de r�cup�ration du contr�leur du portail pour infliger des d�g�ts
            PortalController portal = other.GetComponent<PortalController>();
            if (portal != null)
            {
                portal.TakeDamage(GameManager.Instance.damage); // Appel de la m�thode de d�g�t sur le portail
                Destroy(gameObject); // Destruction de la balle apr�s l'impact
            }
        }
    }
}