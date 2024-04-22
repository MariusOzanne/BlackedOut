/*

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            items = gameObject;  // Stocke l'item actuellement accessible
            itemsText.GetComponent<Text>().text = itemsData.itemDescription;  // Met à jour le texte selon l'item

            // Active le bouton et configure le OnClickListener
            itemsButton.SetActive(true);
            itemsButton.GetComponent<Button>().onClick.RemoveAllListeners();  // Supprime tous les anciens listeners
            itemsButton.GetComponent<Button>().onClick.AddListener(Pick);  // Ajoute le listener de l'item actuel
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            itemsButton.SetActive(false);
            itemsButton.GetComponent<Button>().onClick.RemoveAllListeners();  // Il est important de nettoyer après la sortie
        }
    }

    public void Pick()
    {
        if (items == null)
            return;

        // S'il y'a collision avec l'objet nommé
        switch (items.name)
        {
            #region Consommables

            case "SmallHeal":
                GameManager.Instance.life += itemsData.regenPts;
                break;

            case "SmallShield":
                GameManager.Instance.AddShield(itemsData.shieldPts);
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

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private ItemsData itemsDataTemplate; // Modèle des données de l'item
    private ItemsData itemsData; // Données de l'item utilisées par cette instance
    [SerializeField] private GameObject itemsButtonPrefab;
    [SerializeField] private Transform canvasTransform;

    private GameObject itemsButton;
    private Text itemDescriptionText;

    private void Awake()
    {
        // Cloner les données de l'item pour cette instance
        itemsData = itemsDataTemplate.Clone();

        // Trouver le CanvasHUD par le nom s'il n'est pas déjà défini
        if (canvasTransform == null)
        {
            var canvasObj = GameObject.Find("CanvasHUD");
            if (canvasObj != null)
            {
                canvasTransform = canvasObj.transform;
            }
        }

        if (canvasTransform != null)
        {
            // Instancier le bouton du prefab et le mettre comme enfant du canvas
            itemsButton = Instantiate(itemsButtonPrefab, canvasTransform);
            itemsButton.SetActive(false);

            // Trouver le composant Text enfant du bouton et l'initialiser
            itemDescriptionText = itemsButton.GetComponentInChildren<Text>();
            if (itemDescriptionText == null)
            {
                Debug.LogError("No Text component found on button prefab's children");
            }

            // Positionner le bouton
            itemsButton.transform.localPosition = new Vector3(169f, -139.9f, 0f);
            itemsButton.transform.localScale = Vector3.one;
        }
        else
        {
            // Si le canvas n'est toujours pas trouvé, log une erreur
            Debug.LogError("CanvasTransform is not set or is persistent. Button will be instantiated without a parent.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Configurer le bouton et le texte
            itemDescriptionText.text = itemsData.itemDescription;
            itemsButton.SetActive(true);

            // Configurer le OnClickListener
            itemsButton.GetComponent<Button>().onClick.RemoveAllListeners();
            itemsButton.GetComponent<Button>().onClick.AddListener(() => Pick(gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            itemsButton.SetActive(false);
            itemsButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public void Pick(GameObject item)
    {
        if (item == null)
            return;

        switch (item.name)
        {
            case "SmallHeal":
                GameManager.Instance.life += itemsData.regenPts;
                break;
            case "SmallShield":
                GameManager.Instance.AddShield(itemsData.shieldPts);
                break;
        }

        item.SetActive(false);
        itemsButton.SetActive(false);
    }
}