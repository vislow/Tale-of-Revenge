using UnityEngine;
using Cameras;
using Audio;

namespace Player.Components
{
    public class PlayerComponent : MonoBehaviour
    {
        internal PlayerManager playerManager { get => PlayerManager.instance; }

        public PlayerManager.PlayerComponents components { get => playerManager.components; }

        public new Collider2D collider { get => components.collider; }

        public Rigidbody2D rb { get => components.rb; }
        public Animator anim { get => components.anim; }
        public Transform center { get => components.center; }

        public new PlayerAnimation animation { get => components.animation; }

        public PlayerCombat combat { get => components.combat; }
        public PlayerHealth health { get => components.health; }
        public PlayerCollision collision { get => components.collision; }
        public PlayerController controller { get => components.controller; }
        public PlayerCamera playerCamera { get => components.playerCamera; }
        public PlayerKnockbackManager knockback { get => components.knockback; }
        public PlayerSpearManager spearManager { get => components.spearManager; }
        public PlayerDeathManager deathManager { get => components.deathManager; }

        public LocalAudioPlayer audioPlayer { get => components.audioPlayer; }

        public PlayerManager.PlayerSoundEffects soundEffects { get => playerManager.soundEffects; }
        public PlayerManager.PlayerParticleEffects particleEffects { get => playerManager.particleEffects; }
    }
}