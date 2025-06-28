using UnityEngine;

namespace Root.Player.Components
{
    public class PlayerAnimation : PlayerComponent
    {
        private Animations currentAnimation;

        private void Update() => HandleAnimations();

        private void HandleAnimations()
        {
            if (deathManager.dead)
            {
                DeathAnimations();
            }
            else if (knockback.inKnockback)
            {
                KnockbackAnimations();
            }
            else if (combat.attacking)
            {
                AttackAnimations();
            }
            else if (collision.grounded)
            {
                GroundedAnimations();
            }
            else
            {
                AirAnimations();
            }

            anim.Play(currentAnimation.ToString());
        }

        private void DeathAnimations()
        {
            switch (deathManager.CurrentDeathStage)
            {
                case DeathStages.Dying:
                    currentAnimation = Animations.Dying;
                    break;
                case DeathStages.Respawning:
                    currentAnimation = Animations.Respawning;
                    break;
            }
        }

        private void KnockbackAnimations()
        {
            currentAnimation = Animations.Knockback;
        }

        private void AttackAnimations()
        {
            switch (combat.currentAttackIndex)
            {
                case 0: currentAnimation = Animations.SideAttack; break;
                case 1: currentAnimation = Animations.UpAttack; break;
                case 2: currentAnimation = Animations.DownAttack; break;
            }
        }

        private void GroundedAnimations()
        {
            bool running = Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon && input.horizontalInput != 0;
            currentAnimation = running ? Animations.Running : Animations.Idle;
        }

        private void AirAnimations()
        {
            currentAnimation = rb.linearVelocity.y > 0 ? Animations.Jumping : Animations.Falling;
        }
    }
}