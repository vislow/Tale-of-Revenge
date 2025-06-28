using Root;
using UnityEngine;

namespace Root
{
    public class FixRigidbodyOnPause : MonoBehaviour
    {
        public Rigidbody2D rb;

        private RigidbodyType2D bodyType;
        private Vector2 velocity;
        private float angularVelocity;

        private void Awake()
        {
            GameManager.OnGameStateChanged += OnGameStateChange;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= OnGameStateChange;
        }

        private void OnGameStateChange(GameState gameState)
        {
            // When the game is paused, capture the rigidbodies velocities, then make the object static
            // When the game is unpaused, make the rigidbody dynamic then set the velocities back to how they were pre-pause.
            if (rb == null) return;

            if (gameState == GameState.Paused)
            {
                bodyType = rb.bodyType;
                velocity = rb.linearVelocity;
                angularVelocity = rb.angularVelocity;

                rb.bodyType = RigidbodyType2D.Static;
            }
            else if (gameState == GameState.Gameplay)
            {
                rb.bodyType = bodyType;
                rb.linearVelocity = velocity;
                rb.angularVelocity = angularVelocity;
            }
        }
    }
}