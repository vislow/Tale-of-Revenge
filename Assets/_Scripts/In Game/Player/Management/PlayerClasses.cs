using Root.Player.Camera;
using Root.Player.Components;
using Root.Systems.Audio;
using UnityEngine;

namespace Root.Player
{
    public class PlayerClasses
    {
        [System.Serializable]
        public class PlayerComponents
        {
            public Animator anim;
            public Rigidbody2D rb;
            public Transform center;
            public Collider2D collider;
            [Space]
            public LocalAudioController audioPlayer;
            public CameraController playerCamera;
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
    }
}