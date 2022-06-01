using System.Collections.Generic;
using UnityEngine;

namespace Root.Systems.Popups
{
    [CreateAssetMenu(fileName = "NewPopupData", menuName = "Scriptable Objects/Popups/Popup Data")]
    public class PopupData : ScriptableObject
    {
        public List<Popup> popups = new List<Popup>();

        [System.Serializable]
        public class Popup
        {
            public Sprite icon;
            [TextArea(4, 4)] public string text;
        }
    }
}