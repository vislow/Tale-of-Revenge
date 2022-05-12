using UnityEngine;
using TMPro;

namespace Root.Utility
{
    public class VersionTextManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;

        private void Awake() => labelText.text = Application.version;
    }
}