using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // Données de l'objet à ramasser
    [SerializeField] private Button pickupButtonPrefab; // Préfabriqué du bouton pour ramasser l'objet

    private Button spawnedButton; // Bouton instancié

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le joueur entre en collision avec l'objet
        if (other.CompareTag("Player"))
        {
            // Instancie le bouton de ramassage si ce n'est pas déjà fait
            if (pickupButtonPrefab != null && spawnedButton == null)
            {
                // Trouve le Canvas par tag
                Canvas targetCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                if (targetCanvas != null)
                {
                    // Crée le bouton de ramassage dans le Canvas spécifié
                    spawnedButton = Instantiate(pickupButtonPrefab, targetCanvas.transform, false);
                    spawnedButton.onClick.AddListener(HandlePickup); // Ajoute un écouteur pour gérer le ramassage
                    spawnedButton.gameObject.SetActive(true); // Affiche le bouton
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Vérifie si le joueur quitte la collision avec l'objet
        if (other.CompareTag("Player"))
        {
            // Nettoie le bouton de ramassage si nécessaire
            if (spawnedButton != null)
            {
                spawnedButton.onClick.RemoveListener(HandlePickup);
                Destroy(spawnedButton.gameObject);
            }
        }
    }

    private void HandlePickup()
    {
        ApplyItemEffect(); // Applique l'effet de l'objet
        Destroy(spawnedButton.gameObject); // Détruit le bouton
        Destroy(gameObject); // Détruit l'objet dans la scène
    }

    private void ApplyItemEffect()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        Health health = player.GetComponent<Health>();
        Shield shield = player.GetComponent<Shield>();

        // Applique l'effet basé sur le nom de l'objet
        switch (itemData.name)
        {
            case "Heal":
                health.Heal(itemData.healthRegeneration); // Régénère la santé
                break;
            case "Shield":
                shield.AddShield(itemData.shieldAmount); // Ajoute un bouclier
                break;
            case "Rage":
                GameManager.Instance.ActivateRageMode(itemData); // Active le mode rage
                break;
        }
    }
}