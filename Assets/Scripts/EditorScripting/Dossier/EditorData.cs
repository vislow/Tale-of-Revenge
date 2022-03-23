using UnityEngine;

namespace CustomEditors.Dossier
{
    [CreateAssetMenu(fileName = "EditorData", menuName = "Scriptable Objects/Create New Editor Data")]
    public class EditorData : ScriptableObject
    {
        public Texture defaultIcon;
        public int descriptionMarginSize = 1;
    }
}