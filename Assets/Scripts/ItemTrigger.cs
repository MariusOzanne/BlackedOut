using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private ItemsData itemsData;
    [SerializeField] private GameObject itemsButton;
    [SerializeField] private GameObject itemsText;
    private GameObject items;

    // Lorsque le joueur rentre en collision avec la potion
    private void OnTriggerEnter(Collider other)
    {
        // Si on touche le tag item
        if (gameObject.CompareTag("Item"))
        {
            // Récupérer le bon item
            items = gameObject;
            itemsButton.SetActive(true);
            itemsText.GetComponent<Text>().text = "Récupérer l'objet";   // Affichage du texte
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si on sors de la collision avec l'item
        if (gameObject.CompareTag("Item"))
        {
            // On libère la variable
            itemsButton.SetActive(false);
        }
    }

    public void Pick()
    {

        // S'il y'a collision avec l'objet nommé
        switch (items.name)
        {
            #region Consommables

            case "SmallHeal":
                GameManager.Instance.life += itemsData.regenPts; 
                break;

            case "SmallShield":
                GameManager.Instance.shield += itemsData.shieldPts;   // On récupère 5 points de faim
                break;

            #endregion

            default:
                // Si l'objet n'a pas de nom défini, on sort de la fonction
                return;
        }

        // On masque l'item et le dialogue lorsque l'objet a été ramassé
        items.SetActive(false);
        itemsButton.SetActive(false);
        
    }
}