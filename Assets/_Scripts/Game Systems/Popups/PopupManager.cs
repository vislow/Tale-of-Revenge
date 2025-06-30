using Root.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Root.Popups
{
    public class PopupManager : MonoBehaviour
    {
        public static PopupManager instance;

        [SerializeField] private GameObject popupContainer;
        [Space]
        [SerializeField] private Sprite defaultIcon;
        [SerializeField] private Image popupIcon;
        [SerializeField] private TextMeshProUGUI popupText;

        internal static float popupInputTime = 0.2f;
        internal float popupInputTimer;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);
        }

        public void InvokePopup(string text, Sprite icon = null)
        {
            InputManager.instance.DisablePlayerInput();

            popupContainer.SetActive(true);

            popupText.text = text;

            popupIcon.sprite = icon == null ? defaultIcon : icon;
            popupIcon.SetNativeSize();
        }

        public void ClosePopup()
        {
            popupContainer.SetActive(false);

            InputManager.instance.EnablePlayerInput();
        }
    }
}
