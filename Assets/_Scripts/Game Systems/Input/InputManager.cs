using Root.Player;
using UnityEngine;

namespace Root.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        internal PlayerControls input;
        internal float verticalInput => overrideInput ? overriddenInputDirection.y : Mathf.RoundToInt(inputDirection.y);
        internal float horizontalInput => overrideInput ? overriddenInputDirection.x : Mathf.RoundToInt(inputDirection.x);

        private Vector2 inputDirection;
        private Vector2 overriddenInputDirection;
        private bool overrideInput;

        #region Initalization
        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);

            InitalizeInput();
        }

        private void InitalizeInput()
        {
            input = new PlayerControls();

            input.Gameplay.Move.performed += context => inputDirection = context.ReadValue<Vector2>();
            input.Gameplay.Move.canceled += context => inputDirection = Vector2.zero;
            input.UI.Pause.performed += context => OnPausePressed();
        }

        private void OnEnable() => input.Enable();
        private void OnDisable() => input.Disable();

        private void OnPausePressed()
        {
            if (GameManager.gameState == GameState.Gameplay)
            {

                GameManager.instance.PauseGame();
            }
            else
            {
                GameManager.instance.ResumeGame();
            }
        }
        #endregion

        #region Helper Functions
        public void OverrideXInput(int direction) => Override(direction);
        public void OverrideXInput(int direction, float time = 0)
        {
            Override(direction);
            Invoke(nameof(StopOverrideInput), time);
        }

        private void Override(int direction)
        {
            overrideInput = true;
            overriddenInputDirection.x = direction;
        }

        public void StopOverrideInput() => overrideInput = false;

        public void DisablePlayerInput()
        {
            if (PlayerManager.isPlayerNull) return;

            input.Gameplay.Disable();
            PlayerManager.instance.playerInput.DeactivateInput();
        }

        public void EnablePlayerInput()
        {
            if (PlayerManager.isPlayerNull) return;

            input.Gameplay.Enable();
            PlayerManager.instance.playerInput.ActivateInput();


        }
        #endregion
    }
}
