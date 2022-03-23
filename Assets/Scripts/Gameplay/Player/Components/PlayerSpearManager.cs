using System;
using UnityEngine;
using UnityEngine.Events;
using Entities.Projectiles.Player;
using Cameras;
using UnityEngine.InputSystem;
using Data;

namespace Player.Components
{
    public class PlayerSpearManager : PlayerComponent
    {
        /// <Description> Variables </Description>

        [SerializeField] private bool debug;
        [Space]
        [SerializeField] private float spearOffsetDistance = 4f;
        [SerializeField] private Vector2 spearSpawnOffset = new Vector2(0f, 0.15f);
        [SerializeField] private GameObject spearPrefab;
        [Space]
        [SerializeField] private float spearVelocity = 150f;
        [SerializeField] private float maxSpearRange = 30f;
        [Space]
        [SerializeField] private float spearThrowDelay = 0.3f;
        [SerializeField] private float spearRetractDelay = 0.1f;
        [Space]
        [SerializeField] private UnityEvent OnSpearThrown;

        private SpearProjectile currentSpear;
        private GameObject currentSpearObject;
        private Vector2 closestAimPoint;
        private float spearThrowDelayCounter;
        private float spearRetractDelayCounter;
        private bool spearActive;

        /// <Description> Methods </Description>

        private void Update()
        {
            if (!spearActive)
            {
                spearThrowDelayCounter -= Time.deltaTime;
            }
            else if (spearActive)
            {
                spearRetractDelayCounter -= Time.deltaTime;
            }
        }

        public void OnSpear(InputAction.CallbackContext context)
        {
            if (spearActive)
            {
                float distanceToSpear = Vector2.Distance(transform.position, currentSpearObject.transform.position);

                if (distanceToSpear > maxSpearRange || (context.performed && spearRetractDelayCounter <= 0))
                {
                    RetractSpear();
                }
            }
            else
            {
                if (context.performed && spearThrowDelayCounter <= 0)
                {
                    spearActive = true;

                    OnSpearThrown?.Invoke();

                    currentSpearObject = Instantiate(spearPrefab, GetSpearStartPosition(), GetSpearStartRotation());

                    currentSpear = currentSpearObject.GetComponent<SpearProjectile>();

                    currentSpear.speed = spearVelocity;

                    currentSpear.OnSpearDestroyed += OnSpearDestroyed;
                    currentSpear.OnSpearDestroyed += OnSpearHitObject;
                }
            }
        }

        private void RetractSpear()
        {
            currentSpear.target = center;
            currentSpear.OnSpearRetracted?.Invoke();
        }

        private Quaternion GetSpearStartRotation()
        {
            Vector3 aimDir = MouseWorldPosition.instance.GetMouseWorldPosition() - center.position;

            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

            Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);

            return quaternion;
        }

        private Vector3 GetSpearStartPosition()
        {
            Vector3 aimDir = (MouseWorldPosition.instance.GetMouseWorldPosition() - center.position).normalized;
            Vector3 centerPos = center.position;
            Vector3 spearOriginPosition = centerPos + (aimDir * spearOffsetDistance);

            RaycastHit2D hit = Physics2D.Raycast(centerPos, aimDir, spearOffsetDistance, collision.groundLayer);

            float distanceToHitPoint = Vector2.Distance(centerPos, hit.point);
            float distanceToSpearOrigin = Vector2.Distance(centerPos, spearOriginPosition);

            if (hit.point != Vector2.zero && distanceToHitPoint < distanceToSpearOrigin)
            {
                spearOriginPosition = hit.point;
            }

            return spearOriginPosition;
        }

        private void OnSpearDestroyed()
        {
            spearActive = false;
            currentSpear = null;
            currentSpearObject = null;

            spearThrowDelayCounter = spearThrowDelay;
        }

        private void OnSpearHitObject()
            => spearRetractDelayCounter = spearRetractDelay;

        private void OnDrawGizmos()
        {
            if (!debug || MouseWorldPosition.instance == null) return;

            Vector3 centerPos = center.position;
            Vector3 aimDir = (MouseWorldPosition.instance.GetMouseWorldPosition() - centerPos).normalized;

            Vector3 spearOriginPosition = centerPos + (aimDir * spearOffsetDistance);

            RaycastHit2D hit = Physics2D.Raycast(centerPos, aimDir, Mathf.Infinity, collision.groundLayer);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetSpearStartPosition(), 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center.position, 0.2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(MouseWorldPosition.instance.GetMouseWorldPosition(), 0.2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hit.point, 0.2f);
        }
    }
}