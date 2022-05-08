using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Root.Player.Components
{
    public class PlayerController : PlayerComponent
    {
        [Header("Movement")]
        [SerializeField] private float speed = 10f;
        [SerializeField][Range(0, 1)] private float acceleration = 0.4f;
        [SerializeField][Range(0, 1)] private float decceleration = 0.9f;
        [Space]
        [SerializeField] private float spearHopForce = 20f;
        [SerializeField] private float spearHopMinVelocity = -20f;
        [Space]
        [SerializeField] private float jumpForce = 28f;
        [SerializeField] private float coyoteTime = 0.12f;
        [SerializeField] private float jumpBufferTime = 0.25f;
        [SerializeField][Range(0, 1)] private float variableJumpMultiplier = 0.4f;
        [Space]
        [SerializeField] private float runParticleMaxDelay = 0.25f;
        [SerializeField] private float runParticleMinDelay = 0.1f;
        [Space]
        [SerializeField] private float dashSpeed = 3500f;
        [SerializeField] private float dashTime = 0.08f;

        internal float horizontalInput;
        internal float verticalInput;
        internal float facingDirection = 1;
        internal bool dashing;

        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        private float runParticleTimer;
        private bool jumping;

        private bool jumpPressed;
        private bool jumpHeld;

        private void Start()
            => PlayerDeathManager.OnDeathStageChanged += DeathEvents;

        private void OnDestroy()
            => PlayerDeathManager.OnDeathStageChanged -= DeathEvents;

        private void Update()
        {
            if (deathManager.dead)
            {
                horizontalInput = 0;
                return;
            }

            if (horizontalInput != 0)
            {
                facingDirection = horizontalInput;
            }

            FaceDirection();
            JumpHandling();
        }

        private void FixedUpdate()
        {
            if (deathManager.dead)
                return;

            HorizontalMovement();
        }

        private void DeathEvents(DeathStages deathStage)
        {
            switch (deathStage)
            {
                case DeathStages.Dying:
                    horizontalInput = 0;
                    verticalInput = 0;
                    rb.bodyType = RigidbodyType2D.Static;
                    break;
                case DeathStages.Done:
                    rb.velocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    break;
            }
        }

        #region Jump
        public void OnJump(InputAction.CallbackContext context)
        {
            if (dashing || combat.attacking || knockback.inKnockback) return;

            if (context.canceled)
            {
                jumpHeld = false;
            }

            if (context.performed)
            {
                jumpBufferCounter = jumpBufferTime;

                jumpPressed = true;
            }

            if (context.started)
            {
                jumpHeld = true;
            }
        }

        private void JumpHandling()
        {
            jumpBufferCounter -= Time.deltaTime;
            coyoteTimeCounter -= Time.deltaTime;

            if (dashing) return;

            if (rb.velocity.y < Mathf.Epsilon)
            {
                jumping = false;

                if (collision.wasGrounded && !collision.grounded)
                {
                    coyoteTimeCounter = coyoteTime;
                }
            }

            if (jumping && !jumpHeld)
            {
                StopJump();
            }

            if (jumping) return;

            if ((jumpPressed || jumpBufferCounter > 0)
            && (collision.grounded || coyoteTimeCounter > 0))
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

        public void SpearHop()
        {
            if (collision.grounded || rb.velocity.y < spearHopMinVelocity) return;

            audioPlayer.Play(soundEffects.jump);

            rb.velocity = new Vector2(rb.velocity.x, spearHopForce);
        }

        private void StopJump()
        {
            Vector2 velocity = rb.velocity;
            rb.velocity = new Vector2(velocity.x, velocity.y * variableJumpMultiplier);

            jumping = false;
        }
        #endregion

        #region Dash
        public void OnDash(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            StartCoroutine(Dash());
        }

        private IEnumerator Dash()
        {
            dashing = true;

            var dashTimer = dashTime;

            while (dashTimer > 0)
            {
                var velocity = new Vector2(facingDirection * dashSpeed * Time.deltaTime, 0f);

                rb.velocity = velocity;

                dashTimer -= Time.deltaTime;

                yield return null;
            }

            dashing = false;
        }
        #endregion

        #region Movement
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 dir = context.ReadValue<Vector2>();

            horizontalInput = dir.x;
            verticalInput = dir.y;
        }

        private void HorizontalMovement()
        {
            if (dashing || knockback.inKnockback)
                return;

            runParticleTimer -= Time.deltaTime;

            if (horizontalInput == 0)
            {
                ResetRunParticleTimer();

                var velocity = Mathf.Lerp(rb.velocity.x, 0f, decceleration);

                Move(velocity);
            }
            else
            {
                if (collision.grounded && runParticleTimer < 0)
                {
                    Instantiate(particleEffects.run, transform.position, Quaternion.identity);
                    ResetRunParticleTimer();
                }

                var velocity = Mathf.Lerp(rb.velocity.x, horizontalInput * speed, acceleration);

                Move(velocity);
            }
        }

        private void Move(float xSpeed)
            => rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        #endregion

        private void FaceDirection()
        {
            if (dashing || combat.attacking)
                return;

            if (horizontalInput != 0)
            {
                var yRotation = horizontalInput == 1 ? 0f : 180f;
                var rotation = Quaternion.Euler(0f, yRotation, 0f);

                transform.rotation = rotation;
            }
        }

        private void ResetRunParticleTimer()
            => runParticleTimer = Random.Range(runParticleMinDelay, runParticleMaxDelay);
    }
}