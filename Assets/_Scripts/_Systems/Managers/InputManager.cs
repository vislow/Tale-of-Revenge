using UnityEngine;
using Root.Player.Components;

namespace Root.Systems.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        internal PlayerControls input;
        internal float horizontalInput;
        internal float verticalInput;

        private Vector2 inputDirection
        {
            set
            {
                horizontalInput = value.x;
                verticalInput = value.y;
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

            input.Gameplay.Move.performed += context => inputDirection = context.ReadValue<Vector2>().normalized;
            input.Gameplay.Move.canceled += context => inputDirection = Vector2.zero;

            PlayerDeathManager.OnDeathStageChanged += DeathEvents;
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
