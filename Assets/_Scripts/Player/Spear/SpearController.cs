using UnityEngine;
using Root.Player.Components;
using Root.Entities.Projectiles;
using Root.Systems.Input;

namespace Root.Player
{
    public class SpearController : BaseProjectile
    {
        [SerializeField] private GameObject solidCollider;
        [SerializeField] private LayerMask spearLayer;
        [Space]
        [SerializeField] private float spearCheckDistance;

        private PlayerController playerController;
        private PlayerCollision playerCollision;
        private GameObject playerObject;
        private Rigidbody2D playerRb;

        internal bool returningToPlayer { get; private set; }
        private bool inWall;

        public void Init(PlayerCollision playerCollision, GameObject playerObject, Rigidbody2D playerRb)
        {
            this.playerCollision = playerCollision;
            this.playerObject = playerObject;
            this.playerRb = playerRb;
        }

        private void Update() => HandlePlatformBehaviour();

        private void FixedUpdate() => HandleMovement();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!returningToPlayer && other.CompareTag("Ground"))
            {
                inWall = true;
                rb.velocity = Vector3.zero;
            }
            else if (returningToPlayer && other.CompareTag("Player"))
            {
                Destroy(this.gameObject);
            }
        }

        private void HandleMovement()
        {
            if (inWall || rb.bodyType == RigidbodyType2D.Static) return;

            Vector2 speed = transform.right * base.speed * Time.deltaTime;
            rb.velocity = returningToPlayer ? -speed : speed;

            if (!returningToPlayer) return;

            RotateTowardsPlayer();
        }

        private void HandlePlatformBehaviour()
        {
            if (!inWall)
            {
                solidCollider.SetActive(false);
                return;
            }

            if (InputManager.instance.verticalInput == -1)
            {
                solidCollider.SetActive(false);
                return;
            }

            solidCollider.SetActive(playerCollision.spearUnderPlayer);
        }

        public void ReturnToPlayer(GameObject playerObject)
        {
            returningToPlayer = true;
            inWall = false;
        }

        private void RotateTowardsPlayer()
        {
            var vectorToTarget = (transform.position - playerObject.transform.position).normalized;
            var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = rotation;
        }

        private void OnDrawGizmos()
        {
            if (!debug) return;

            Gizmos.DrawWireSphere(transform.position, spearCheckDistance);

            if (playerObject == null) return;

            Gizmos.DrawLine(playerObject.transform.position, playerObject.transform.position + new Vector3(0, -10));
        }
    }
}
