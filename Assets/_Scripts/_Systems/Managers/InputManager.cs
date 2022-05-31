using Root.Player;
using Root.Systems.States;
using UnityEngine;

namespace Root.Systems.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        [SerializeField] private RectTransform cursorTransform;

        internal PlayerControls input;

        private Vector2 inputDirection;
        private Vector2 overriddenInputDirection;
        public float verticalInput => overrideInput ? overriddenInputDirection.y : Mathf.RoundToInt(inputDirection.y);
        public float horizontalInput => overrideInput ? overriddenInputDirection.x : Mathf.RoundToInt(inputDirection.x);
        private bool overrideInput;

        private PlayerManager player => PlayerManager.instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            Initialize();

            void Initialize()
            {
                input = new PlayerControls();

                input.Gameplay.Move.performed += context => inputDirection = context.ReadValue<Vector2>();
                input.Gameplay.Move.canceled += context => inputDirection = Vector2.zero;

                GameStateManager.OnGameStateChanged += OnGameStateChanged;
            }
        }

        private void OnGameStateChanged(GameState state) => cursorTransform.gameObject.SetActive(state == GameState.Gameplay);

        private void Update()
        {
            HandleCursor();
        }

        private void HandleCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            if (GameStateManager.inGame)
            {
                cursorTransform.position = UnityEngine.Input.mousePosition;
            }
        }

        #region Input Overriding
        public void OverrideInput(int direction) => Override(direction);
        public void OverrideInput(int direction, float time = 0)
        {
            Override(direction);
            Invoke(nameof(ReturnInput), time);
        }

        private void Override(int direction)
        {
            overrideInput = true;
            overriddenInputDirection.x = direction;
        }

        public void ReturnInput() => overrideInput = false;
        #endregion

        #region Input Activity
        [ContextMenu("Disable Player Input")]
        public void DisablePlayerInput()
        {
            if (!CheckPlayerAvailable()) return;
            input.Gameplay.Disable();
            PlayerManager.instance.playerInputComponent.DeactivateInput();
        }

        [ContextMenu("Enable Player Input")]
        public void EnablePlayerInput()
        {
            if (!CheckPlayerAvailable()) return;
            input.Gameplay.Enable();
            PlayerManager.instance.playerInputComponent.ActivateInput();
        }

        private bool CheckPlayerAvailable()
        {
            if (player == null || player.playerInputComponent == null)
            {
                ConsoleLog("Player not available, can't access player input component");
                return false;
            }

            return true;
        }
        #endregion

        private void OnEnable() => input.Enable();
        private void OnDisable() => input.Disable();
        private void OnDestroy() => GameStateManager.OnGameStateChanged -= OnGameStateChanged;

        [ContextMenu("Run Log Test")]
        private void LogTest() => ConsoleLog("Log test");
        private void ConsoleLog(string message) => Utility.Utils.ConsoleLog(this, message);
    }
}
