using UnityEngine;

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
        [SerializeField] private float spearOffsetDistance;

        private bool isSpearActive;
        private SpearController spearController;

        private void Update()
        {
            if (!Input.GetMouseButtonDown(2)) return;

            if (isSpearActive)
            {
                RetractSpear();
            }
            else
            {
                SpawnSpear();
            }
        }

        private void SpawnSpear()
        {
            isSpearActive = true;

            controller.SpearHop();

            GameObject spearObject = Instantiate(spearPrefab, GetSpearStartPosition(), GetSpearStartRotation());

            spearController = spearObject.GetComponent<SpearController>();
            spearController.playerObject = gameObject;
        }

        private void RetractSpear()
        {
            spearController.ReturnToPlayer(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!spearController.returningToPlayer || !other.CompareTag("Spear")) return;

            isSpearActive = false;
        }

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

            float distanceToHitPoint = Vector2.Distance(centerPos, hit.point);
            float distanceToSpearOrigin = Vector2.Distance(centerPos, spearOriginPosition);

            if (hit.point != Vector2.zero && distanceToHitPoint < distanceToSpearOrigin)
            {
                spearOriginPosition = hit.point;
            }

            return spearOriginPosition;
        }

        private void OnDrawGizmos()
        {
            if (!debug) return;

            Gizmos.DrawWireSphere(GetSpearStartPosition(), 0.3f);
        }
    }
}
