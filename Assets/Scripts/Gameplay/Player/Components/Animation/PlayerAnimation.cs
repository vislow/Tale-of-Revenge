using UnityEngine;

namespace Player.Components
{
    public class PlayerAnimation : PlayerComponent
    {
        /// <Description> Variables </Description>

        private Animations currentAnimation;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Update()
        {
            if (deathManager.dead)
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
            else if (knockback.inKnockback)
            {
                currentAnimation = Animations.Knockback;
            }
            else if (combat.attacking)
            {
                switch (combat.currentAttackIndex)
                {
                    case 0:
                        currentAnimation = Animations.SideAttack;
                        break;
                    case 1:
                        currentAnimation = Animations.UpAttack;
                        break;
                    case 2:
                        currentAnimation = Animations.DownAttack;
                        break;
                }
            }
            else if (collision.grounded)
            {
                currentAnimation =
                    Mathf.Abs(rb.velocity.x) > Mathf.Epsilon && controller.horizontalInput != 0
                    ? Animations.Running : currentAnimation = Animations.Idle;
            }
            else
            {
                currentAnimation =
                    rb.velocity.y > 0
                    ? Animations.Jumping : Animations.Falling;
            }

            anim.Play(currentAnimation.ToString());
        }

        /// <Description> Custom Methods </Description>
    }
}