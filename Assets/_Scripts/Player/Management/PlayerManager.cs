using Root.Cameras;
using Root.Player.Components;
using Root.Systems.Audio;
using Root.Systems.States;
using UnityEngine;

namespace Root.Player
{
    [RequireComponent(typeof(PlayerHealth))]
    [RequireComponent(typeof(PlayerCombat))]
    [RequireComponent(typeof(PlayerAnimation))]
    [RequireComponent(typeof(PlayerCollision))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerDeathManager))]
    [RequireComponent(typeof(PlayerKnockbackManager))]
    [RequireComponent(typeof(LocalAudioController))]
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        public PlayerComponents components;
        public PlayerSoundEffects soundEffects;
        public PlayerParticleEffects particleEffects;

        [System.Serializable]
        public class PlayerComponents
        {
            public Animator anim;
            public Rigidbody2D rb;
            public Transform center;
            public Collider2D collider;
            [Space]
            public LocalAudioController audioPlayer;
            public PlayerCameraController playerCamera;
            [Space]
            public PlayerHealth health;
            public PlayerCombat combat;
            public PlayerSpearManager spearManager;
            public PlayerController controller;
            public PlayerDeathManager deathManager;
            public PlayerKnockbackManager knockback;
            public PlayerAnimation animation;
            public PlayerCollision collision;
        }

        [System.Serializable]
        public class PlayerParticleEffects
        {
            public GameObject jump, land, damaged, run;
        }

        [System.Serializable]
        public class PlayerSoundEffects
        {
            public AudioObject jump, land;
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

            GameStateManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy() => GameStateManager.OnGameStateChanged -= OnGameStateChanged;

        private void Reset()
        {
            components.health = GetComponent<PlayerHealth>();
            components.combat = GetComponent<PlayerCombat>();
            components.animation = GetComponent<PlayerAnimation>();
            components.collision = GetComponent<PlayerCollision>();
            components.spearManager = GetComponent<PlayerSpearManager>();
            components.controller = GetComponent<PlayerController>();
            components.deathManager = GetComponent<PlayerDeathManager>();
            components.knockback = GetComponent<PlayerKnockbackManager>();
            components.audioPlayer = GetComponent<LocalAudioController>();
        }

        public void OnGameStateChanged(GameState gameState)
        {
            if ((gameState != GameState.Gameplay && gameState != GameState.Paused)) return;

            bool active = gameState == GameState.Gameplay;

            components.anim.enabled = active;
            components.combat.enabled = active;
            components.health.enabled = active;
            components.collider.enabled = active;
            components.knockback.enabled = active;
            components.animation.enabled = active;
            components.controller.enabled = active;
            components.spearManager.enabled = active;
        }

        public void MovePlayer(Vector2 position)
        {
            transform.position = position;

            components.playerCamera.CenterCamera();
        }
    }
}