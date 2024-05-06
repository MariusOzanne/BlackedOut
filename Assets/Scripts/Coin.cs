using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform player; // Référence au transform du joueur

    private float attractSpeed = 5f; // Vitesse à laquelle la pièce est attirée vers le joueur
    private float rotationSpeed = 100f; // Vitesse de rotation de la pièce

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Trouve le joueur par tag
    }

    private void Update()
    {
        // Si la pièce est proche du joueur, elle est attirée vers lui
        if (Vector3.Distance(transform.position, player.position) < 5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
        }

        // Rotation de la pièce autour de son axe Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si la pièce entre en collision avec le joueur, elle est collectée
        if (other.gameObject.CompareTag("Player"))
        {
            // Augmente le nombre de pièces et met à jour l'UI des pièces
            GameManager.Instance.coins++;
            GameManager.Instance.UpdateCoinsUI();

            // Détruit la pièce de la scène
            Destroy(gameObject);
        }
    }
}