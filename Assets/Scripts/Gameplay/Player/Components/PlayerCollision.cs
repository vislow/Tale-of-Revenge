using UnityEngine;

using Data;

namespace Player.Components
{
    public class PlayerCollision : PlayerComponent
    {
        /// <Description> Variables </Description>

        [SerializeField] private bool drawGizmos;
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

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Start()
        {
            PlayerDeathManager.OnDeathStageChanged += DeathEvents;

            originalGravityScale = rb.gravityScale;

            if (!SaveHelper.CurrentSaveAvailable) return;

            playerManager.MovePlayer(SaveHelper.CurrentSave.playerData.lastSafePosition);
        }

        private void Update()
        {
            Collision();
            Effects();
        }

        private void OnDestroy()
            => PlayerDeathManager.OnDeathStageChanged -= DeathEvents;

        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;

            // This draws a gizmo representing the position of the players ground check
            Vector3 groundCheckCenter = transform.position + (Vector3)groundCheckOffset;

            Gizmos.DrawWireSphere(groundCheckCenter, groundCheckRadius);
        }

        /// <Description> Custom Methods </Description>

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

            //
            Vector3 groundCheckPosition = transform.position + (Vector3)groundCheckOffset;

            grounded =
                Physics2D.OverlapCircle(groundCheckPosition, groundCheckRadius, groundLayer)
                && components.collider.IsTouchingLayers(groundLayer);

            rb.gravityScale =
                grounded && components.controller.horizontalInput == 0 ? 0 : originalGravityScale;

            CheckSpearUnderPlayer();
            SaveLastSafePosition();
        }

        private void CheckSpearUnderPlayer()
        {
            Vector2 playerPos = transform.position;

            Vector2 leftRayPos = new Vector2(playerPos.x - spearCheckOffset, playerPos.y);
            bool rayLeft = Physics2D.Raycast(leftRayPos, Vector2.down, 2f, spearLayer);

            Vector2 rightRayPos = new Vector2(playerPos.x - spearCheckOffset, playerPos.y);
            bool rayRight = Physics2D.Raycast(rightRayPos, Vector2.down, 2f, spearLayer);

            spearUnderPlayer = rayLeft || rayRight;
        }

        private void SaveLastSafePosition()
        {
            if (!grounded)
            {
                safePositionCounter = timeToSavePosition;
            }
            else if (!spearUnderPlayer)
            {
                safePositionCounter -= Time.deltaTime;

                if (safePositionCounter >= 0) return;

                lastSafePosition = transform.position;
                SaveHelper.CurrentSave.playerData.lastSafePosition = lastSafePosition;
            }
        }

        #endregion

        private void Effects()
        {
            // Landing Effects
            if (wasGrounded || !grounded) return;

            // Play landing sound
            components.audioPlayer.Play(soundEffects.land);

            // Spawn landing smoke
            var rotation = components.controller.facingDirection == 1 ? 180f : 0f;
            var smokeRotation = Quaternion.Euler(
                new Vector3(0f, rotation, 0f));

            Instantiate(particleEffects.land, transform.position, smokeRotation);
        }
    }
}