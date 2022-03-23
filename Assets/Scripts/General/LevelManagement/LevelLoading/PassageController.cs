using UnityEngine;

using Player;

namespace LevelLoading
{
    public class PassageController : MonoBehaviour
    {
        /// <Description> Variables </Description>

        [HideInInspector] public int id;

        [SerializeField] private Collider2D passageTrigger;
        [Space]
        [SerializeField] private Vector2 spawnPositionOffset = new Vector2(0, -2);

        internal Vector3 spawnPosition { get => (Vector3)spawnPositionOffset + transform.position; }

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private bool passageDisabled;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (passageDisabled) return;

            PassageManager.instance.LoadLevel(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                passageDisabled = false;
            }
        }

        /// <Description> Custom Methods </Description>

        public void TeleportPlayer()
        {
            passageDisabled = true;

            PlayerManager playerManager = PlayerManager.instance;

            if (playerManager == null)
            {
                Debug.Log("There is no available instance of the player");

                return;
            }

            playerManager.MovePlayer(spawnPosition);
        }
    }
}