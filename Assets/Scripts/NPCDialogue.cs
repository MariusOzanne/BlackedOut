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
        "C'est Sasha. La fille � qui tu as menti et tu�e !",
        "J'ai cru en toi. J'avais confiance en toi. Mais tu as d�cid� de trahir mes sentiments ! Et je suis s�r que tu as fait la m�me chose avec d'autre filles !",
        "Pr�pare-toi � te confronter � ta r�alit�. Montre moi r�ellement comment t'avais fait pour sauver ma m�re quand des bandits l'avaient attaqu� !"
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
        // Lancer le dialogue du NPC uniquement lorsque le joueur est � proximit� et a interagi avec le bouton
        dialogueManager.dialoguePanel.SetActive(true);
        dialogueManager.SetNPCDialogue(true); 
        dialogueManager.StartDialogue(pnjName, dialogueLines);
        talkButton.SetActive(false);
    }

    // Lorsque le PNJ a termin� le dialogue
    public void EndDialogueForNPC()
    {
        // Activation des portails apr�s la fin du dialogue du PNJ
        foreach (GameObject portal in portalsToActivate)
        {
            portal.SetActive(true);
        }

        talkButton.SetActive(false);
        Destroy(gameObject);
    }
}