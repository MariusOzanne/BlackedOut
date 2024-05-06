using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform player; // R�f�rence au transform du joueur

    private float attractSpeed = 5f; // Vitesse � laquelle la pi�ce est attir�e vers le joueur
    private float rotationSpeed = 100f; // Vitesse de rotation de la pi�ce

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Trouve le joueur par tag
    }

    private void Update()
    {
        // Si la pi�ce est proche du joueur, elle est attir�e vers lui
        if (Vector3.Distance(transform.position, player.position) < 5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
        }

        // Rotation de la pi�ce autour de son axe Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si la pi�ce entre en collision avec le joueur, elle est collect�e
        if (other.gameObject.CompareTag("Player"))
        {
            // Augmente le nombre de pi�ces et met � jour l'UI des pi�ces
            GameManager.Instance.coins++;
            GameManager.Instance.UpdateCoinsUI();

            // D�truit la pi�ce de la sc�ne
            Destroy(gameObject);
        }
    }
}