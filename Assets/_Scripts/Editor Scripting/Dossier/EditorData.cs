using UnityEngine;

namespace CustomEditors.Dossier
{
    [CreateAssetMenu(fileName = "NewEditorData", menuName = "Scriptable Objects/Editor/Dossier/Editor Data")]
    public class EditorData : ScriptableObject
    {
        public Texture defaultIcon;
        public int descriptionMarginSize = 1;
    }
}