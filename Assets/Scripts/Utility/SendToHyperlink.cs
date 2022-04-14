using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc {
    public class SendToHyperlink : MonoBehaviour {
        [SerializeField] private string url;

        public void OpenHyperlink() {
            Application.OpenURL(url);
        }
    }
}