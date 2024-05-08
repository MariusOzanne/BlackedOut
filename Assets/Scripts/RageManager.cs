using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageManager : MonoBehaviour
{
    private PlayerController playerController;
    private Coroutine rageModeCoroutine; // Coroutine pour g�rer la dur�e du mode rage
    private ItemData lastAppliedItemData; // Derni�res donn�es de l'item rage appliqu�es
    
    [HideInInspector] public bool isRageActive = false; // �tat actuel du mode rage

    [SerializeField] private Text rageEffectText; // Texte pour afficher la dur�e du mode rage
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
            ResetRageEffects(); // R�initialisation des effets du mode rage pr�c�dent
        }

        isRageActive = true; // Indique que le mode rage est actif
        lastAppliedItemData = itemData; // Sauvegarde les donn�es de l'item pour une r�application future

        ApplyRageEffects(itemData); // Applique les effets de rage imm�diatement

        rageModeCoroutine = StartCoroutine(RageEffectCoroutine(itemData)); // D�marrage d'une nouvelle coroutine pour le mode rage
    }

    private IEnumerator RageEffectCoroutine(ItemData itemData)
    {
        float timeLeft = itemData.durationOfEffect; // Dur�e du mode rage

        while (timeLeft > 0)
        {
            UpdateRageEffectUI(timeLeft); // Mise � jour de l'UI du mode rage
            timeLeft -= Time.deltaTime; // D�compte du temps restant
            yield return null;
        }

        ResetRageEffects(); // R�initialisation des effets une fois le mode rage termin�
        UpdateRageEffectUI(0); // Mise � jour finale de l'UI du mode rage
        isRageActive = false; // Indique que le mode rage n'est plus actif
    }

    private void ApplyRageEffects(ItemData itemData)
    {
        // Applique l'augmentation de vitesse
        playerController.speed = playerController.defaultSpeed * itemData.speedMultiplier;
        // Applique l'augmentation des d�g�ts
        playerController.currentWeapon.damage = playerController.defaultDamage + itemData.additionalDamage;
    }

    public void UpdateRageEffectsOnWeaponChange()
    {
        if (isRageActive) // V�rifie si le mode rage est actif
        {
            ApplyRageEffects(lastAppliedItemData); // R�applique les effets de rage � la nouvelle arme
        }
    }

    public void ResetRageEffects()
    {
        // Restaure la vitesse et les d�g�ts � leurs valeurs par d�faut
        playerController.ResetSpeedAndDamage();
    }

    private void UpdateRageEffectUI(float timeLeft)
    {
        rageEffectText.text = timeLeft.ToString("F0") + " s"; // Affichage du temps restant en secondes
        rageEffectUI.SetActive(timeLeft > 0); // Active l'UI si le mode rage est actif
        rageParticle.SetActive(timeLeft > 0); // Active les particules si le mode rage est actif
    }
}