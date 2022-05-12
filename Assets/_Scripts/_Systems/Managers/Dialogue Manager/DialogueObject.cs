using System.Collections.Generic;
using UnityEngine;

namespace Root.Systems.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogueObject", menuName = "Scriptable Objects/Dialogue/Dialogue Object")]
    public class DialogueObject : ScriptableObject
    {
        public List<string> conversation = new List<string>();
    }
}