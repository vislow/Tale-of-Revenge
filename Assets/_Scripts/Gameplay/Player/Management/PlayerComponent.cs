using Root.Audio;
using Root.Input;
using Root.Player.Camera;
using UnityEngine;

namespace Root.Player.Components
{
    public class PlayerComponent : MonoBehaviour
    {
        public PlayerManager playerManager;

        public InputManager input => InputManager.instance;

        public PlayerClasses.PlayerComponents components => playerManager.components;

        public new Collider2D collider => components.collider;
        public new PlayerAnimation animation => components.animation;

        public Rigidbody2D rb => components.rb;
        public Animator anim => components.anim;
        public Transform center => components.center;
        public LocalAudioController audioPlayer => components.audioPlayer;

        public PlayerCombat combat => components.combat;
        public PlayerHealth health => components.health;
        public PlayerCollision collision => components.collision;
        public PlayerController controller => components.controller;
        public PlayerSpearManager spearManager => components.spearManager;
        public PlayerKnockbackManager knockback => components.knockback;
        public PlayerDeathManager deathManager => components.deathManager;
        public CameraController playerCamera => components.playerCamera;

        public PlayerClasses.PlayerSoundEffects soundEffects => playerManager.soundEffects;
        public PlayerClasses.PlayerParticleEffects particleEffects => playerManager.particleEffects;
    }
}