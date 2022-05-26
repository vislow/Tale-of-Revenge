using Root.Systems.States;
using UnityEngine;

namespace Root.ObjectManagement
{
    public class FixRigidbodyOnPause : MonoBehaviour
    {
        public Rigidbody2D rb;

        private Vector2 velocity;
        private float angularVelocity;
        private RigidbodyType2D bodyType;

        private void Awake() => GameStateManager.OnGameStateChanged += OnStateChange;

        private void OnDestroy() => GameStateManager.OnGameStateChanged -= OnStateChange;

        private void OnStateChange(GameState gameState)
        {
            if (rb == null) return;

            if (gameState == GameState.Paused)
            {
                velocity = rb.velocity;
                bodyType = rb.bodyType;
                angularVelocity = rb.angularVelocity;

                rb.bodyType = RigidbodyType2D.Static;
            }
            else if (gameState == GameState.Gameplay)
            {
                rb.bodyType = bodyType;

                if (rb.bodyType != RigidbodyType2D.Static)
                {
                    rb.velocity = velocity;
                    rb.angularVelocity = angularVelocity;
                }
            }
        }
    }
}