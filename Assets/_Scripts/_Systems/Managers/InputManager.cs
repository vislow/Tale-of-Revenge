using Root.Player.Components;
using UnityEngine;

namespace Root.Systems.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        [SerializeField] private RectTransform cursorTransform;

        internal PlayerControls input;
        public float horizontalInput;
        public float verticalInput;

        private Vector2 inputDirection
        {
            set
            {
                horizontalInput = Mathf.RoundToInt(value.x);
                verticalInput = Mathf.RoundToInt(value.y);
            }
        }

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

            input = new PlayerControls();

            input.Gameplay.Move.performed += context => inputDirection = context.ReadValue<Vector2>();
            input.Gameplay.Move.canceled += context => inputDirection = Vector2.zero;

            PlayerDeathManager.OnDeathStageChanged += DeathEvents;
        }

        private void Update()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            cursorTransform.position = UnityEngine.Input.mousePosition;
        }

        private void OnEnable() => input.Enable();

        private void OnDisable() => input.Disable();

        private void OnDestroy() => PlayerDeathManager.OnDeathStageChanged -= DeathEvents;

        private void DeathEvents(DeathStages deathStage)
        {
            switch (deathStage)
            {
                case DeathStages.Dying:
                    horizontalInput = 0;
                    verticalInput = 0;
                    break;
            }
        }
    }
}
