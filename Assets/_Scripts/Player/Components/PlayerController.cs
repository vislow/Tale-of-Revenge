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

        internal float facingDirection = 1;
        internal bool dashing;

        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        private float runParticleTimer;
        private bool jumping;

        private Vector2 slopeNormal;
        private float slopeAngle;

        private bool jumpPressed;
        private bool jumpHeld;

        private void Start() => PlayerDeathManager.OnDeathStageChanged += DeathEvents;

        private void OnDestroy() => PlayerDeathManager.OnDeathStageChanged -= DeathEvents;

        private void Update()
        {
            FaceDirection();
            JumpHandling();
        }

        private void FixedUpdate() => HorizontalMovement();

        private void DeathEvents(DeathStages deathStage)
        {
            switch (deathStage)
            {
                case DeathStages.Dying:
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

        public void SpearHop()
        {
            if (collision.grounded || rb.velocity.y < spearHopMinVelocity || rb.bodyType == RigidbodyType2D.Static) return;

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
        private void HorizontalMovement()
        {
            if (deathManager.dead || dashing || knockback.inKnockback) return;

            runParticleTimer -= Time.deltaTime;

            if (input.horizontalInput == 0)
            {
                ResetRunParticleTimer();
                Move(Mathf.Lerp(rb.velocity.x, 0f, decceleration));
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

        private void Move(float xSpeed)
        {
            if (!collision.grounded)
            {
                rb.velocity = new Vector2(xSpeed, rb.velocity.y);
                return;
            }

            if (jumping) return;

            RaycastHit2D hit = Physics2D.Raycast(center.position, Vector2.down, 1f, collision.groundLayer);

            if (hit)
            {
                slopeNormal = Vector2.Perpendicular(hit.normal).normalized;
                slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            }

            rb.velocity = new Vector2(-xSpeed * slopeNormal.x, -xSpeed * slopeNormal.y);
        }
        #endregion

        private void FaceDirection()
        {
            if (input.horizontalInput == 0) return;

            facingDirection = input.horizontalInput;

            if (dashing || combat.attacking) return;

            float yRotation = input.horizontalInput == 1 ? 0f : 180f;
            Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);

            transform.rotation = rotation;
        }

        private void ResetRunParticleTimer() => runParticleTimer = Random.Range(runParticleMinDelay, runParticleMaxDelay);
    }
}