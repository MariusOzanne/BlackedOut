using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float fixedYPosition; // Position Y fixe pour que la balle se déplace seulement en X et Z

    private void Start()
    {
        fixedYPosition = transform.position.y; // Sauvegarde de la position Y initiale au démarrage
    }

    private void Update()
    {
        // Maintien de la balle à une hauteur constante pendant son déplacement
        transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérification si la balle entre en collision avec un ennemi
        if (other.CompareTag("Enemy"))
        {
            // Tentative de récupération du contrôleur de l'ennemi pour infliger des dégâts
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(GameManager.Instance.damage); // Appel de la méthode de dégât sur l'ennemi
                Destroy(gameObject); // Destruction de la balle après l'impact
            }
        }

        // Vérification si la balle entre en collision avec un portail
        if (other.CompareTag("Portal"))
        {
            // Tentative de récupération du contrôleur du portail pour infliger des dégâts
            PortalController portal = other.GetComponent<PortalController>();
            if (portal != null)
            {
                portal.TakeDamage(GameManager.Instance.damage); // Appel de la méthode de dégât sur le portail
                Destroy(gameObject); // Destruction de la balle après l'impact
            }
        }
    }
}