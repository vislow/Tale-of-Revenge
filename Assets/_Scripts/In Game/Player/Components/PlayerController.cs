using System.Collections;
using Root.Systems.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Root.Player.Components
{
    public class PlayerController : PlayerComponent
    {
        [Header("Horizontal Movement")]
        [SerializeField] private float speed = 10f;
        [SerializeField][Range(0, 1)] private float acceleration = 0.4f;
        [SerializeField][Range(0, 1)] private float decceleration = 0.9f;
        [Space]
        [SerializeField] private float runParticleMaxDelay = 0.25f;
        [SerializeField] private float runParticleMinDelay = 0.1f;
        [Header("Spear Hop")]
        [SerializeField] private float spearHopForce = 20f;
        [Header("Jumping")]
        [SerializeField] private float jumpForce = 28f;
        [SerializeField] private float coyoteTime = 0.12f;
        [SerializeField] private float jumpBufferTime = 0.25f;
        [SerializeField][Range(0, 1)] private float variableJumpMultiplier = 0.4f;
        [Header("Dashing")]
        [SerializeField] private float dashSpeed = 3500f;
        [SerializeField] private float dashTime = 0.08f;

        private float runParticleTimer;

        private bool canSpearHop;

        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        private bool jumping;
        private bool jumpPressed;
        private bool jumpHeld;

        private bool dashing;

        internal float facingDirection = 1;

        private void Update()
        {
            FaceDirection();
            JumpHandling();

            if (collision.grounded)
            {
                canSpearHop = true;
            }
        }

        private void FixedUpdate() => HorizontalMovement();

        public void DoSpearHop()
        {
            if (!canSpearHop || collision.grounded || rb.bodyType == RigidbodyType2D.Static) return;

            audioPlayer.Play(soundEffects.jump);
            rb.velocity = new Vector2(rb.velocity.x, spearHopForce);

            canSpearHop = false;
        }

        #region Horizontal Movement
        private void HorizontalMovement()
        {
            if (!GameStateManager.inGameplay) return;

            if (dashing || knockback.inKnockback) return;

            runParticleTimer -= Time.deltaTime;

            if (input.horizontalInput == 0)
            {
                Move(Mathf.Lerp(rb.velocity.x, 0f, decceleration));
                ResetRunParticleTimer();
            }
            else
            {
                if (collision.grounded && runParticleTimer < 0)
                {
                    Instantiate(particleEffects.run, transform.position, Quaternion.identity);
                    ResetRunParticleTimer();
                }

                Move(Mathf.Lerp(rb.velocity.x, input.horizontalInput * speed, acceleration));
            }
        }

        private void Move(float xSpeed) => rb.velocity = new Vector2(xSpeed, rb.velocity.y);

        private void ResetRunParticleTimer() => runParticleTimer = Random.Range(runParticleMinDelay, runParticleMaxDelay);
        #endregion

        #region Jumping
        public void OnJump(InputAction.CallbackContext context)
        {
            if (dashing || combat.attacking || knockback.inKnockback) return;

            if (context.started)
            {
                jumpHeld = true;
            }

            if (context.performed)
            {
                jumpBufferCounter = jumpBufferTime;
                jumpPressed = true;
            }

            if (context.canceled)
            {
                jumpHeld = false;
            }
        }

        private void JumpHandling()
        {
            jumpBufferCounter -= Time.deltaTime;
            coyoteTimeCounter -= Time.deltaTime;

            if (dashing) return;

            HandleFalling();
            HandleActiveInput();
            HandleJumping();

            void HandleFalling()
            {
                if (rb.velocity.y >= Mathf.Epsilon) return;

                jumping = false;

                if (!collision.wasGrounded || collision.grounded) return;

                coyoteTimeCounter = coyoteTime;
            }

            void HandleActiveInput()
            {
                if (!jumping || jumpHeld) return;

                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpMultiplier);
                jumping = false;
            }

            void HandleJumping()
            {
                if (jumping) return;

                if ((jumpPressed || jumpBufferCounter > 0) && (collision.grounded || coyoteTimeCounter > 0))
                {
                    jumping = true;

                    audioPlayer.Play(soundEffects.jump);

                    if (collision.grounded)
                    {
                        var smokeRotation = Quaternion.Euler(new Vector3(0f, facingDirection == 1 ? 180f : 0f, 0f));
                        Instantiate(particleEffects.jump, transform.position, smokeRotation);
                    }

                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }

                jumpPressed = false;
            }
        }
        #endregion

        #region Dashing
        public IEnumerator OnDash(InputAction.CallbackContext context)
        {
            if (!context.performed) yield return null;

            dashing = true;

            float dashTimer = dashTime;

            while (dashTimer > 0)
            {
                rb.velocity = new Vector2(facingDirection * dashSpeed * Time.deltaTime, 0f);
                dashTimer -= Time.deltaTime;

                yield return null;
            }

            dashing = false;
        }
        #endregion

        #region Player Direction
        private void FaceDirection()
        {
            if (input.horizontalInput == 0) return;

            facingDirection = input.horizontalInput;

            if (dashing || combat.attacking) return;

            float yRotation = input.horizontalInput == 1 ? 0f : 180f;
            Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);

            transform.rotation = rotation;
        }
        #endregion
    }
}