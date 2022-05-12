using UnityEngine;

namespace Root.Utility
{
    public class SendToHyperlink : MonoBehaviour
    {
        [SerializeField] private string url;

        public void OpenHyperlink() => Application.OpenURL(url);
    }
}