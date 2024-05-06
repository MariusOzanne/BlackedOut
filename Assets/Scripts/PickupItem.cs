using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // Donn�es de l'objet � ramasser
    [SerializeField] private Button pickupButtonPrefab; // Pr�fabriqu� du bouton pour ramasser l'objet

    private Button spawnedButton; // Bouton instanci�

    private void OnTriggerEnter(Collider other)
    {
        // V�rifie si le joueur entre en collision avec l'objet
        if (other.CompareTag("Player"))
        {
            // Instancie le bouton de ramassage si ce n'est pas d�j� fait
            if (pickupButtonPrefab != null && spawnedButton == null)
            {
                // Trouve le Canvas par tag
                Canvas targetCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                if (targetCanvas != null)
                {
                    // Cr�e le bouton de ramassage dans le Canvas sp�cifi�
                    spawnedButton = Instantiate(pickupButtonPrefab, targetCanvas.transform, false);
                    spawnedButton.onClick.AddListener(HandlePickup); // Ajoute un �couteur pour g�rer le ramassage
                    spawnedButton.gameObject.SetActive(true); // Affiche le bouton
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // V�rifie si le joueur quitte la collision avec l'objet
        if (other.CompareTag("Player"))
        {
            // Nettoie le bouton de ramassage si n�cessaire
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
        Destroy(spawnedButton.gameObject); // D�truit le bouton
        Destroy(gameObject); // D�truit l'objet dans la sc�ne
    }

    private void ApplyItemEffect()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        Health health = player.GetComponent<Health>();
        Shield shield = player.GetComponent<Shield>();

        // Applique l'effet bas� sur le nom de l'objet
        switch (itemData.name)
        {
            case "Heal":
                health.Heal(itemData.healthRegeneration); // R�g�n�re la sant�
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