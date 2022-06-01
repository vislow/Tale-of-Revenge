using TMPro;
using UnityEngine;

namespace Root.Systems.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;

        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;

        private DialogueObject currentDialogue;

        private int maxIndex;
        private int dialogueIndex = -1;
        private bool dialogueActive => dialogueIndex != -1;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);

            dialogueIndex = -1;
            UpdateDialogueText();
        }

        public void StartDialogue(DialogueObject dialogueObject)
        {
            if (dialogueActive || dialogueObject == null) return;

            currentDialogue = dialogueObject;
            maxIndex = currentDialogue.conversation.Count - 1;
            dialogueIndex = 0;

            UpdateDialogueText();
        }

        public void ContinueDialogue()
        {
            if (!dialogueActive || currentDialogue == null)
            {
                CloseDialogue();
                return;
            }

            dialogueIndex++;

            if (dialogueIndex > maxIndex)
            {
                CloseDialogue();
                return;
            }

            UpdateDialogueText();
        }

        public void CloseDialogue()
        {
            dialogueIndex = -1;
            maxIndex = 0;

            UpdateDialogueText();

            currentDialogue = null;
        }

        private void UpdateDialogueText()
        {
            dialogueBox.SetActive(dialogueActive);
            dialogueText.text = !dialogueActive ? "" : currentDialogue.conversation[dialogueIndex];
        }
    }
}