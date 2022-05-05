using UnityEngine;

namespace Root
{
    public class StatsVisibility : MonoBehaviour
    {
        private PlayerControls controls;
        private CanvasGroup canvasGroup;

        private bool visible;

        private void Awake()
        {
            controls = new PlayerControls();

            controls.UI.Menu.performed += context => OnMenu();

            canvasGroup = GetComponent<CanvasGroup>();
            visible = canvasGroup.alpha == 1;
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void OnMenu()
        {
            visible = !visible;

            canvasGroup.alpha = visible ? 1 : 0;
        }
    }
}