using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageManager : MonoBehaviour
{
    private PlayerController playerController;
    private Coroutine rageModeCoroutine; // Coroutine pour gérer la durée du mode rage
    private ItemData lastAppliedItemData; // Dernières données de l'item rage appliquées
    
    [HideInInspector] public bool isRageActive = false; // État actuel du mode rage

    [SerializeField] private Text rageEffectText; // Texte pour afficher la durée du mode rage
    [SerializeField] private GameObject rageEffectUI; // UI pour le mode rage
    [SerializeField] private GameObject rageParticle; // Effets visuels pour le mode rage

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void ActivateRageMode(ItemData itemData)
    {
        if (rageModeCoroutine != null)
        {
            StopCoroutine(rageModeCoroutine);
            ResetRageEffects(); // Réinitialisation des effets du mode rage précédent
        }

        isRageActive = true; // Indique que le mode rage est actif
        lastAppliedItemData = itemData; // Sauvegarde les données de l'item pour une réapplication future

        ApplyRageEffects(itemData); // Applique les effets de rage immédiatement

        rageModeCoroutine = StartCoroutine(RageEffectCoroutine(itemData)); // Démarrage d'une nouvelle coroutine pour le mode rage
    }

    private IEnumerator RageEffectCoroutine(ItemData itemData)
    {
        float timeLeft = itemData.durationOfEffect; // Durée du mode rage

        while (timeLeft > 0)
        {
            UpdateRageEffectUI(timeLeft); // Mise à jour de l'UI du mode rage
            timeLeft -= Time.deltaTime; // Décompte du temps restant
            yield return null;
        }

        ResetRageEffects(); // Réinitialisation des effets une fois le mode rage terminé
        UpdateRageEffectUI(0); // Mise à jour finale de l'UI du mode rage
        isRageActive = false; // Indique que le mode rage n'est plus actif
    }

    private void ApplyRageEffects(ItemData itemData)
    {
        // Applique l'augmentation de vitesse
        playerController.speed = playerController.defaultSpeed * itemData.speedMultiplier;
        // Applique l'augmentation des dégâts
        playerController.currentWeapon.damage = playerController.defaultDamage + itemData.additionalDamage;
    }

    public void UpdateRageEffectsOnWeaponChange()
    {
        if (isRageActive) // Vérifie si le mode rage est actif
        {
            ApplyRageEffects(lastAppliedItemData); // Réapplique les effets de rage à la nouvelle arme
        }
    }

    public void ResetRageEffects()
    {
        // Restaure la vitesse et les dégâts à leurs valeurs par défaut
        playerController.ResetSpeedAndDamage();
    }

    private void UpdateRageEffectUI(float timeLeft)
    {
        rageEffectText.text = timeLeft.ToString("F0") + " s"; // Affichage du temps restant en secondes
        rageEffectUI.SetActive(timeLeft > 0); // Active l'UI si le mode rage est actif
        rageParticle.SetActive(timeLeft > 0); // Active les particules si le mode rage est actif
    }
}