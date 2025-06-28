using UnityEngine;

namespace Root.Dialogue
{
    public class DialogueSource : MonoBehaviour
    {
        [SerializeField] private DialogueObject dialogueObject;

        public void StartDialogue()
        {
            if (dialogueObject == null) return;

            DialogueManager.instance.StartDialogue(dialogueObject);
        }

        public void ContinueDialogue() => DialogueManager.instance.ContinueDialogue();

        public void CloseDialogue() => DialogueManager.instance.CloseDialogue();

        private void Update()
        {
            if (!UnityEngine.Input.GetButtonDown("Submit")) return;

            StartDialogue();
        }
    }
}