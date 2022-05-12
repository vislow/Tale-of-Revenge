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
            input = new PlayerControls();

            input.Gameplay.Move.performed += context => inputDirection = context.ReadValue<Vector2>();

            PlayerDeathManager.OnDeathStageChanged += DeathEvents;
        }

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
