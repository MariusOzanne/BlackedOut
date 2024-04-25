using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject talkButton;
    [SerializeField] private GameObject[] portalsToActivate;
    [SerializeField] private string pnjName = "Sasha";

    private string[] dialogueLines = new string[]
    {
        "Salut Boris ! Tu te souviens de moi?",
        "C'est Sasha. La fille à qui tu as menti et tuée !",
        "J'ai cru en toi. J'avais confiance en toi. Mais tu as décidé de trahir mes sentiments ! Et je suis sûr que tu as fait la même chose avec d'autre filles !",
        "Prépare-toi à te confronter à ta réalité. Montre moi réellement comment t'avais fait pour sauver ma mère quand des bandits l'avaient attaqué !"
    };

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("NPC"))
        {
            talkButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.CompareTag("NPC"))
        {
            talkButton.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        // Lancer le dialogue du NPC uniquement lorsque le joueur est à proximité et a interagi avec le bouton
        dialogueManager.dialoguePanel.SetActive(true);
        dialogueManager.SetNPCDialogue(true); 
        dialogueManager.StartDialogue(pnjName, dialogueLines);
        talkButton.SetActive(false);
    }

    // Lorsque le PNJ a terminé le dialogue
    public void EndDialogueForNPC()
    {
        // Activation des portails après la fin du dialogue du PNJ
        foreach (GameObject portal in portalsToActivate)
        {
            portal.SetActive(true);
        }

        talkButton.SetActive(false);
        Destroy(gameObject);
    }
}