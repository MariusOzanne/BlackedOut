using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform player;
    private float attractSpeed = 5f;
    private float rotationSpeed = 100f;

    public AudioClip coinSound;
    private AudioSource CoinAudio;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        CoinAudio = gameObject.AddComponent<AudioSource>();
        CoinAudio.clip = coinSound;
        CoinAudio.playOnAwake = false;
    }

    void Update()
    {
        // Vérifie si la pièce est suffisamment proche pour être attirée vers le joueur
        if (Vector3.Distance(transform.position, player.position) < 5f)  // Distance d'attraction
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
        }

        // Rotation continue pour l'animation
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si la pièce entre en collision avec le joueur
        if (other.gameObject.CompareTag("Player"))
        {
            CoinAudio.Play();

            GameManager.Instance.coins++;  // Incrémenter le nombre de pièces dans le GameManager
            GameManager.Instance.UpdateCoinCount();  // Mettre à jour l'affichage des pièces

            Destroy(gameObject);  // Détruire la pièce pour qu'elle disparaisse
        }
    }
}