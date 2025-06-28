using Root;
using Root.Audio;
using Root.Input;
using Root.Player.Components;
using UnityEngine;
using UnityEngine.InputSystem;

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
        public static bool isPlayerNull => instance == null;

        public PlayerInput playerInput;

        public PlayerClasses.PlayerComponents components;
        public PlayerClasses.PlayerSoundEffects soundEffects;
        public PlayerClasses.PlayerParticleEffects particleEffects;

        internal bool isPlayerDead => instance.components.deathManager.dead;
        internal Vector3 playerPosition => instance.transform.position;

        public void MovePlayer(Vector2 position)
        {
            transform.position = position;
            components.playerCamera.CenterCamera();
        }

        public void UnlockSpear() => components.spearManager.spearUnlocked = true;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);

            GameManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void Start() => PlayerDeathManager.OnDeathStageChanged += DeathEvents;

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Gameplay: UnPauseActions(); break;
                case GameState.Paused: PauseActions(); break;
            }

            void PauseActions() => InputManager.instance.DisablePlayerInput();
            void UnPauseActions() => InputManager.instance.EnablePlayerInput();
        }

        private void DeathEvents(DeathStages deathStage)
        {
            switch (deathStage)
            {
                case DeathStages.Dying:
                    InputManager.instance.DisablePlayerInput();
                    components.rb.bodyType = RigidbodyType2D.Static;
                    break;
                case DeathStages.Done:
                    components.rb.linearVelocity = Vector2.zero;
                    components.rb.bodyType = RigidbodyType2D.Dynamic;
                    InputManager.instance.EnablePlayerInput();
                    break;
            }
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= OnGameStateChanged;
            PlayerDeathManager.OnDeathStageChanged -= DeathEvents;
        }
    }
}