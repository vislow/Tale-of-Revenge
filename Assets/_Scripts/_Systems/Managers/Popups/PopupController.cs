using System.Collections;
using Root.Systems.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Root.Systems.Popups
{
    public class PopupController : MonoBehaviour
    {
        [SerializeField] private bool popupOnAwake;
        [SerializeField] private bool singleUsePopup = true;
        [Space]
        [SerializeField] private PopupData popupData;
        [SerializeField] private UnityEvent OnPopupClosed;

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

            for (int i = 0; i < popupData.popups.Count; i++)
            {
                PopupData.Popup popup = popupData.popups[i];
                PopupManager.instance.InvokePopup(popup.text, popup.icon);

                PopupManager.instance.popupInputTimer = PopupManager.popupInputTime;

                while (PopupManager.instance.popupInputTimer >= 0f)
                {
                    PopupManager.instance.popupInputTimer -= Time.deltaTime;
                    yield return null;
                }

                while (!submitPressed)
                {
                    yield return null;
                }

                if (i == popupData.popups.Count - 1)
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
            if (PauseManager.instance.GamePaused || PopupManager.instance.popupInputTimer > 0f) return;

            submitPressed = true;
        }
    }
}
