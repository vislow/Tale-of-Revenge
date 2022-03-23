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

            playerManager.MovePlayer(SaveManager.instance.currentSave.playerData.lastSafePosition);
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
            if (!drawGizmos)
                return;

            Gizmos.DrawWireSphere(transform.position + (Vector3)groundCheckOffset, groundCheckRadius);
        }

        /// <Description> Custom Methods </Description>

        private void DeathEvents(DeathStages deathStages)
        {
            switch (deathStages)
            {
                case DeathStages.Dying:
                    components.collider.enabled = false;
                    break;
                case DeathStages.Done:
                    components.collider.enabled = false;
                    break;
            }
        }

        private void Collision()
        {
            wasGrounded = grounded;

            Vector3 groundCheckPosition = transform.position + (Vector3)groundCheckOffset;

            grounded =
                Physics2D.OverlapCircle(groundCheckPosition, groundCheckRadius, groundLayer)
                && components.collider.IsTouchingLayers(groundLayer);

            rb.gravityScale =
                grounded && components.controller.horizontalInput == 0 ? 0 : originalGravityScale;

            /// <Description> This saves the players last safe position </Description>

            Vector2 playerPos = transform.position;

            bool rayLeft = Physics2D.Raycast(new Vector2(playerPos.x - spearCheckOffset, playerPos.y), Vector2.down, 2f, spearLayer);
            bool rayRight = Physics2D.Raycast(new Vector2(playerPos.x + spearCheckOffset, playerPos.y), Vector2.down, 2f, spearLayer);

            spearUnderPlayer = rayLeft || rayRight;

            /// <Description> </Description>

            if (grounded && !spearUnderPlayer)
            {
                safePositionCounter -= Time.deltaTime;

                if (safePositionCounter < 0)
                {
                    lastSafePosition = transform.position;
                    SaveManager.instance.currentSave.playerData.lastSafePosition = lastSafePosition;
                }
            }
            else if (!grounded)
            {
                safePositionCounter = timeToSavePosition;
            }
        }

        private void Effects()
        {
            if (!wasGrounded && grounded)
            {
                components.audioPlayer.Play(soundEffects.land);

                float rotation = components.controller.facingDirection == 1 ? 180f : 0f;

                Quaternion smokeRotation = Quaternion.Euler(new Vector3(0f, rotation, 0f));

                Instantiate(particleEffects.land, transform.position, smokeRotation);
            }
        }
    }
}