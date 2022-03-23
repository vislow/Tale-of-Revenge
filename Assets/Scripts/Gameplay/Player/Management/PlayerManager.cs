using UnityEngine;
using Audio;
using Cameras;
using GameManagement;
using Player.Components;

namespace Player
{
    [RequireComponent(typeof(Components.PlayerHealth))]
    [RequireComponent(typeof(Components.PlayerCombat))]
    [RequireComponent(typeof(Components.PlayerAnimation))]
    [RequireComponent(typeof(Components.PlayerCollision))]
    [RequireComponent(typeof(Components.PlayerController))]
    [RequireComponent(typeof(Components.PlayerSpearManager))]
    [RequireComponent(typeof(Components.PlayerDeathManager))]
    [RequireComponent(typeof(Components.PlayerKnockbackManager))]
    [RequireComponent(typeof(Audio.LocalAudioPlayer))]
    public class PlayerManager : MonoBehaviour
    {
        /// <Description> Variables </Description>

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
            public PlayerSpearManager spearManager;
            public PlayerKnockbackManager knockback;
            public Components.PlayerAnimation animation;
            public Components.PlayerCollision collision;
            [Space]
            public PlayerCamera playerCamera;
            [Space]
            public LocalAudioPlayer audioPlayer;
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

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

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
            components.spearManager = GetComponent<PlayerSpearManager>();
            components.knockback = GetComponent<PlayerKnockbackManager>();
            components.animation = GetComponent<Components.PlayerAnimation>();
            components.collision = GetComponent<Components.PlayerCollision>();
            components.audioPlayer = GetComponent<Audio.LocalAudioPlayer>();
        }

        /// <Description> Custom Methods </Description>

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
            components.spearManager.enabled = active;
        }

        public void MovePlayer(Vector2 position)
        {
            transform.position = position;

            components.playerCamera.CenterCamera();
        }
    }
}