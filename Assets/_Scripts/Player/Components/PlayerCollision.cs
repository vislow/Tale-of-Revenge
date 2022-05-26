using UnityEngine;

using Root.Data;

namespace Root.Player.Components
{
    public class PlayerCollision : PlayerComponent
    {
        [SerializeField] private bool debug;
        [Header("Ground Collision")]
        [SerializeField] private float groundCheckRadius = 0.25f;
        [SerializeField] private Vector2 groundCheckOffset = new Vector2(0, 0.1f);
        [SerializeField] internal LayerMask groundLayer = 1 << 6;
        [Header("Spear Collision")]
        [SerializeField] private float spearCheckOffset = 0.1f;
        [SerializeField] private LayerMask spearLayer = 1 << 10;
        [Header("General")]
        [SerializeField] private float timeToSavePosition = 0.5f;
        [SerializeField] private PhysicsMaterial2D playerMaterial;

        internal bool grounded = true;
        internal bool wasGrounded = true;
        internal bool spearUnderPlayer;

        private float originalGravityScale;
        private float safePositionCounter;

        private Vector2 lastSafePosition;

        private void Start()
        {
            PlayerDeathManager.OnDeathStageChanged += DeathEvents;
            originalGravityScale = rb.gravityScale;

            if (!SaveHelper.GetCurrentSaveAvailable()) return;

            playerManager.MovePlayer(SaveHelper.GetCurrentSave().playerData.lastSafePosition);
        }

        private void Update() => Effects();

        private void FixedUpdate() => Collision();

        private void OnDestroy() => PlayerDeathManager.OnDeathStageChanged -= DeathEvents;

        private void OnDrawGizmos()
        {
            if (!debug) return;

            // This draws a gizmo representing the position of the players ground check
            Bounds bounds = collider.bounds;
            Vector3 groundCheckPosition = bounds.center + new Vector3(0, -bounds.extents.y);

            Gizmos.DrawWireSphere(groundCheckPosition, groundCheckRadius);
        }

        private void DeathEvents(DeathStages deathStages)
        {
            // This disables the players collider while dead
            if (deathStages != DeathStages.Dying && deathStages != DeathStages.Done) return;

            components.collider.enabled = false;
        }

        #region Collision
        private void Collision()
        {
            // Update wasGrounded
            wasGrounded = grounded;

            HandleGroundCheck();
            HandleCheckSpearUnderPlayer();
            SaveLastSafePosition();

            rb.gravityScale = grounded && input.horizontalInput == 0 ? 0 : originalGravityScale;
        }

        private void HandleGroundCheck()
        {
            Bounds bounds = collider.bounds;

            Vector3 groundCheckPosition = bounds.center + new Vector3(0, -bounds.extents.y);

            bool nearGround = Physics2D.OverlapCircle(groundCheckPosition, groundCheckRadius, groundLayer);
            bool colliderTouchingGround = components.collider.IsTouchingLayers(groundLayer);

            grounded = nearGround && colliderTouchingGround;
        }

        private void HandleCheckSpearUnderPlayer()
        {
            Vector2 playerPos = transform.position;
            Vector2 leftRayPos = new Vector2(playerPos.x - spearCheckOffset, playerPos.y);
            Vector2 rightRayPos = new Vector2(playerPos.x + spearCheckOffset, playerPos.y);

            bool leftRayHit = Physics2D.Raycast(leftRayPos, Vector2.down, 1f, spearLayer);
            bool rightRayHit = Physics2D.Raycast(rightRayPos, Vector2.down, 1f, spearLayer);

            spearUnderPlayer = leftRayHit || rightRayHit;
        }

        private void SaveLastSafePosition()
        {
            if (!grounded)
            {
                safePositionCounter = timeToSavePosition;
                return;
            }

            if (!spearUnderPlayer)
            {
                safePositionCounter -= Time.deltaTime;

                if (safePositionCounter >= 0) return;

                lastSafePosition = transform.position;
                SaveHelper.GetCurrentSave().playerData.lastSafePosition = lastSafePosition;
            }
        }

        #endregion

        private void Effects()
        {
            if (wasGrounded || !grounded) return;

            components.audioPlayer.Play(soundEffects.land);

            float rotation = components.controller.facingDirection == 1 ? 180f : 0f;

            Quaternion smokeRotation = Quaternion.Euler(new Vector3(0f, rotation, 0f));

            Instantiate(particleEffects.land, transform.position, smokeRotation);
        }
    }
}