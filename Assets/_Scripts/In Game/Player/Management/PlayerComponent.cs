using Root.Player.Camera;
using Root.Systems.Audio;
using Root.Systems.Input;
using UnityEngine;

namespace Root.Player.Components
{
    public class PlayerComponent : MonoBehaviour
    {
        public PlayerManager playerManager;

        public InputManager input { get => InputManager.instance; }

        public PlayerClasses.PlayerComponents components { get => playerManager.components; }

        public new Collider2D collider { get => components.collider; }
        public new PlayerAnimation animation { get => components.animation; }

        public Rigidbody2D rb { get => components.rb; }
        public Animator anim { get => components.anim; }
        public Transform center { get => components.center; }
        public LocalAudioController audioPlayer { get => components.audioPlayer; }

        public PlayerCombat combat { get => components.combat; }
        public PlayerHealth health { get => components.health; }
        public PlayerCollision collision { get => components.collision; }
        public PlayerController controller { get => components.controller; }
        public PlayerSpearManager spearManager { get => components.spearManager; }
        public PlayerKnockbackManager knockback { get => components.knockback; }
        public PlayerDeathManager deathManager { get => components.deathManager; }
        public CameraController playerCamera { get => components.playerCamera; }

        public PlayerClasses.PlayerSoundEffects soundEffects { get => playerManager.soundEffects; }
        public PlayerClasses.PlayerParticleEffects particleEffects { get => playerManager.particleEffects; }
    }
}