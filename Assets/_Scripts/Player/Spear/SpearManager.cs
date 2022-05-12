using UnityEngine;
using UnityEngine.InputSystem;

namespace Root.Player.Components
{
    public class SpearManager : PlayerComponent
    {
        [SerializeField] private bool debug;
        [Space]
        [SerializeField] private GameObject spearPrefab;
        [SerializeField] private Transform playerCenter;
        [SerializeField] private LayerMask groundLayer;
        [Space]
        [SerializeField] private float maxSpearDistance = 50f;
        [SerializeField] private float spearOffsetDistance = 4f;
        [SerializeField] private float spearThrowCooldown = 0.15f;

        private SpearController spearController;
        private bool isSpearActive;
        private float spearThrowCooldownTimer;

        private void Update() => spearThrowCooldownTimer -= Time.deltaTime;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (spearController == null || !spearController.returningToPlayer || !other.CompareTag("Spear")) return;

            isSpearActive = false;
        }

        public void OnSpear(InputAction.CallbackContext context)
        {
            if (isSpearActive && Vector2.Distance(center.position, spearController.transform.position) > maxSpearDistance)
            {
                RetractSpear();
                return;
            }

            if (!context.performed) return;

            if (isSpearActive)
            {
                RetractSpear();
                return;
            }

            if (spearThrowCooldownTimer > 0) return;

            ThrowSpear();
        }

        private void ThrowSpear()
        {
            isSpearActive = true;
            spearThrowCooldownTimer = spearThrowCooldown;
            controller.SpearHop();

            GameObject spearObject = Instantiate(spearPrefab, GetSpearStartPosition(), GetSpearStartRotation());

            spearController = spearObject.GetComponent<SpearController>();
            spearController.Init(playerCollision: collision, playerObject: gameObject, playerRb: rb);
        }

        private void RetractSpear() => spearController.ReturnToPlayer(gameObject);

        private Quaternion GetSpearStartRotation()
        {
            Vector3 aimDir = Utility.Utils.MouseWorldPosition - playerCenter.position;

            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

            Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);

            return quaternion;
        }

        private Vector3 GetSpearStartPosition()
        {
            Vector3 aimDir = (Utility.Utils.MouseWorldPosition - playerCenter.position).normalized;
            Vector3 centerPos = playerCenter.position;
            Vector3 spearOriginPosition = centerPos + (aimDir * spearOffsetDistance);

            RaycastHit2D hit = Physics2D.Raycast(centerPos, aimDir, spearOffsetDistance, groundLayer);

            if (hit.point != Vector2.zero) return spearOriginPosition;

            float distanceToHitPoint = Vector2.Distance(centerPos, hit.point);
            float distanceToSpearOrigin = Vector2.Distance(centerPos, spearOriginPosition);

            return distanceToHitPoint < distanceToSpearOrigin ? (Vector3)hit.point : spearOriginPosition;
        }

        private void OnDrawGizmos()
        {
            if (!debug) return;

            Gizmos.DrawWireSphere(GetSpearStartPosition(), 0.3f);
        }
    }
}
