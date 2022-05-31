using System.Collections;
using System.Collections.Generic;
using Root.Systems.Input;
using Root.Systems.States;
using UnityEngine;
using UnityEngine.Events;

namespace Root.Systems.Popups
{
    public class PopupController : MonoBehaviour
    {
        [SerializeField] private bool popupOnAwake;
        [SerializeField] private bool singleUsePopup = true;
        [Space]
        [SerializeField] private List<Popup> popups = new List<Popup>();
        [SerializeField] private UnityEvent OnPopupClosed;

        [System.Serializable]
        public class Popup
        {
            public Sprite icon;
            [TextArea(4, 4)] public string text;
        }

        private bool popupShown;
        private bool submitPressed = false;

        private void Awake()
        {
            if (!popupOnAwake) return;

            InvokePopup();
        }

        private void Start() => InputManager.instance.input.UI.Submit.started += context => SubmitPressed();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            InvokePopup();
        }

        public void InvokePopup()
        {
            if (popupShown) return;

            StartCoroutine(PopupProcess());

            if (!singleUsePopup) return;

            popupShown = true;
        }

        private IEnumerator PopupProcess()
        {
            submitPressed = false;

            for (int i = 0; i < popups.Count; i++)
            {
                Popup popup = popups[i];
                PopupManager.instance.InvokePopup(popup.text, popup.icon);

                while (!submitPressed) yield return null;

                if (i == popups.Count - 1)
                {
                    PopupManager.instance.ClosePopup();
                }

                yield return new WaitForEndOfFrame();

                submitPressed = false;
            }

            OnPopupClosed?.Invoke();
        }

        private void SubmitPressed()
        {
            if (PauseManager.instance.GamePaused) return;

            submitPressed = true;
        }
    }
}
