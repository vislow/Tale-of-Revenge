using Root.Input;
using Root.Player;
using UnityEngine;

namespace Root.Levels
{
    public class PassageController : MonoBehaviour
    {
        [HideInInspector] public int id;

        [SerializeField] private float inputExitDuration = 0.5f;
        [SerializeField, Range(-1, 1)] private int inputOverrideDirection;
        [Space]
        [SerializeField] private Collider2D passageTrigger;
        [SerializeField] private Vector2 spawnPositionOffset = new Vector2(0, -2);

        internal Vector3 spawnPosition { get => transform.position + (Vector3)spawnPositionOffset; }

        private bool passageDisabled;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (passageDisabled || !other.CompareTag("Player")) return;

            InputManager.instance.OverrideXInput(inputOverrideDirection);
            PassageManager.instance.LoadLevel(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            passageDisabled = false;
        }

        public void TeleportPlayer()
        {
            passageDisabled = true;

            PlayerManager playerManager = PlayerManager.instance;

            if (PlayerManager.isPlayerNull)
            {
                ConsoleLog("Player is not available, can't teleport to next passage");
                return;
            }

            PlayerManager.instance.MovePlayer(spawnPosition);
            InputManager.instance.OverrideXInput(-inputOverrideDirection, inputExitDuration);
        }

        [ContextMenu("Run Log Test")]
        private void LogTest() => ConsoleLog("Log test");
        private void ConsoleLog(string message) => Utility.Utils.ConsoleLog(this, message);
    }
}