using UnityEngine;
using Root.Audio;
using Root.Cameras;
using Root.GameManagement;
using Root.Player.Components;

namespace Root.Player
{
    [RequireComponent(typeof(Components.PlayerHealth))]
    [RequireComponent(typeof(Components.PlayerCombat))]
    [RequireComponent(typeof(Components.PlayerAnimation))]
    [RequireComponent(typeof(Components.PlayerCollision))]
    [RequireComponent(typeof(Components.PlayerController))]
    [RequireComponent(typeof(Components.PlayerDeathManager))]
    [RequireComponent(typeof(Components.PlayerKnockbackManager))]
    [RequireComponent(typeof(Root.Audio.LocalAudioController))]
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
            public PlayerHealth health;
            public PlayerCombat combat;
            public PlayerController controller;
            public PlayerDeathManager deathManager;
            public PlayerKnockbackManager knockback;
            public Components.PlayerAnimation animation;
            public Components.PlayerCollision collision;
            [Space]
            public PlayerCameraController playerCamera;
            [Space]
            public LocalAudioController audioPlayer;
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

        private void OnDestroy()
            => GameStateManager.OnGameStateChanged -= OnGameStateChanged;

        private void Reset()
        {
            components.health = GetComponent<PlayerHealth>();
            components.combat = GetComponent<PlayerCombat>();
            components.controller = GetComponent<PlayerController>();
            components.deathManager = GetComponent<PlayerDeathManager>();
            components.knockback = GetComponent<PlayerKnockbackManager>();
            components.animation = GetComponent<Components.PlayerAnimation>();
            components.collision = GetComponent<Components.PlayerCollision>();
            components.audioPlayer = GetComponent<Root.Audio.LocalAudioController>();
        }

        public void OnGameStateChanged(GameState gameState)
        {
            if ((gameState != GameState.Gameplay && gameState != GameState.Paused))
                return;

            bool active = gameState == GameState.Gameplay;

            if (components == null) return;

            components.rb.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;

            components.anim.enabled = active;
            components.combat.enabled = active;
            components.health.enabled = active;
            components.collider.enabled = active;
            components.knockback.enabled = active;
            components.animation.enabled = active;
            components.controller.enabled = active;
        }

        public void MovePlayer(Vector2 position)
        {
            transform.position = position;

            components.playerCamera.CenterCamera();
        }
    }
}