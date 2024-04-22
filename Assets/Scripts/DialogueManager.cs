using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float letterDelay;
    [Header("Panel Setup")]
    public Text speakerNameText;
    public Text dialogueText;
    public GameObject dialoguePanel;
    public Button continueButton;

    private string[] dialogueLines; // Contiendra toutes les lignes de dialogue
    private int currentLineIndex = 0; // Index de la ligne de dialogue actuelle

    private void Start()
    {
        // Démarrer le dialogue du joueur au lancement de la scène
        string playerName = "Boris";
        string[] dialogueLines = new string[]
        {
            "O- où suis-je? Argh je ne me souviens de rien et j'ai mal partout.",
            "Je me souviens que j'étais en train de draguer une fille puis.. plus rien..",
            "Je vais peut-être essayer d'avancer pour découvrir un peu ce qui se passe ici."
        };
        StartDialogue(playerName, dialogueLines);
    }

    // Coroutine pour afficher progressivement le dialogue
    IEnumerator TypeDialogue(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter; // Ajouter une lettre à la fois
            yield return new WaitForSeconds(letterDelay); // Delai entre chaque lettres
        }
        // Activer le bouton de continuer le dialogue après l'affichage complet du dialogue
        continueButton.gameObject.SetActive(true);
    }

    // Afficher la prochaine ligne de dialogue
    public void ShowNextLine()
    {
        // Vérifier s'il reste des lignes de dialogue à afficher
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++; // Passer à la ligne suivante
            StartCoroutine(TypeDialogue(dialogueLines[currentLineIndex])); // Afficher la ligne suivante de manière progressive
        }
        else
        {
            // S'il n'y a plus de lignes de dialogue, fermer le panneau de dialogue
            dialoguePanel.SetActive(false);
        }
    }

    // Initialiser le dialogue progressif
    public void StartDialogue(string speakerName, string[] dialogueLines)
    {
        speakerNameText.text = speakerName;
        this.dialogueLines = dialogueLines;
        StartCoroutine(TypeDialogue(dialogueLines[currentLineIndex]));
    }
}
