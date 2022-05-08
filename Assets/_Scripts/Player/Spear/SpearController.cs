using UnityEngine;
using Root.Entities.Projectiles;

namespace Root.Player
{
    public class SpearController : BaseProjectile
    {
        [SerializeField] private float platformPlayerDistanceThreshold;
        [SerializeField] private GameObject solidCollider;
        [SerializeField] private LayerMask spearLayer;

        internal GameObject playerObject;
        internal bool returningToPlayer;
        private bool inWall;

        private void Update() => HandlePlatformBehaviour();

        private void FixedUpdate() => HandleMovement();

        private void HandleMovement()
        {
            if (inWall) return;

            var speed = transform.right * base.speed * Time.deltaTime;

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

            if (Vector2.Distance(playerObject.transform.position, transform.position) > platformPlayerDistanceThreshold) return;

            var rayOrigin = playerObject.transform.position + new Vector3(0, 1f);

            bool playerAboveSpear = Physics2D.Raycast(rayOrigin, Vector2.down, platformPlayerDistanceThreshold, spearLayer);

            solidCollider.SetActive(playerAboveSpear);
        }

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

            Gizmos.DrawWireSphere(transform.position, platformPlayerDistanceThreshold);

            if (playerObject == null) return;

            Gizmos.DrawLine(playerObject.transform.position, playerObject.transform.position + new Vector3(0, -10));
        }
    }
}
