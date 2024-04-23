using UnityEngine;
using UnityEngine.UI;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private ItemsData itemsData;
    [SerializeField] private GameObject itemsButton;
    [SerializeField] private Text itemsText;
    private GameObject items;

    private void Start()
    {
        itemsButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            items = gameObject;  // Stocke l'item actuellement accessible
            itemsText.text = itemsData.itemDescription;  // Met à jour le texte selon l'item

            // Active le bouton et configure le OnClickListener
            itemsButton.SetActive(true);
            itemsButton.GetComponent<Button>().onClick.RemoveAllListeners();  // Supprime tous les anciens listeners
            itemsButton.GetComponent<Button>().onClick.AddListener(Pick);  // Ajoute le listener de l'item actuel
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemsButton.SetActive(false);
            itemsButton.GetComponent<Button>().onClick.RemoveAllListeners();  // Nettoyage après la sortie
        }
    }

    public void Pick()
    {
        if (items == null)
            return;

        switch (itemsData.itemName)
        {
            case "Heal":
                GameManager.Instance.health += itemsData.regenPts;
                break;
            case "Shield":
                GameManager.Instance.AddShield(itemsData.shieldPts);
                break;
            case "Rage":
                GameManager.Instance.ActivateRageMode(itemsData);
                break;
        }

        // Détruit l'item et masque le bouton une fois l'item ramassé
        Destroy(items);
        itemsButton.SetActive(false);
    }
}