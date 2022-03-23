using System.Collections;
using System.Collections.Generic;
using GameManagement;
using UnityEngine;

namespace Gameplay.Objects {
    public class FixRigidbodyOnPause : MonoBehaviour {
        public Rigidbody2D rb;

        private Vector2 velocity;
        private float angularVelocity;

        private void Awake() => GameStateManager.OnGameStateChanged += OnStateChange;

        private void OnDestroy() {
            GameStateManager.OnGameStateChanged -= OnStateChange;
        }

        private void OnStateChange(GameState gameState) {
            if (rb == null) return;

            if (gameState == GameState.Paused) {
                velocity = rb.velocity;
                angularVelocity = rb.angularVelocity;

                rb.bodyType = RigidbodyType2D.Static;
            } else if(gameState == GameState.Gameplay) {
                rb.bodyType = RigidbodyType2D.Dynamic;

                rb.velocity = velocity;
                rb.angularVelocity = angularVelocity;
            }
        }
    }
}