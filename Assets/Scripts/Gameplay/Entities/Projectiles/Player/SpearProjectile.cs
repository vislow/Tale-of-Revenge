using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Entities.Projectiles.Player
{
    public class SpearProjectile : BaseProjectile
    {
        /// <Description> Variables </Description>

        [SerializeField] private Collider2D solidCollider;
        [SerializeField] private LayerMask groundLayer = 1<<6;
        [Space]
        [SerializeField] private GameObject impactParticles;
        [SerializeField] private float fallThroughDisableTime = 0.1f;

        internal Action OnSpearHitObject;
        internal Action OnSpearRetracted;
        internal Action OnSpearDestroyed;
        internal Transform target;

        private float fallThroughDisableCounter;
        private bool spearRetracting;
        private bool inObject;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Awake()
        {
            OnSpearHitObject += OnHitObject;
            OnSpearRetracted += OnRetracted;
        }

        private void OnDestroy()
        {
            OnSpearHitObject -= OnHitObject;
            OnSpearRetracted -= OnRetracted;
        }


        private void Update()
        {
            var player = PlayerManager.instance;

            if (!inObject || player == null)
                return;

            fallThroughDisableCounter -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.S))
            {
                solidCollider.enabled = false;
                fallThroughDisableCounter = fallThroughDisableTime;

                return;
            }

            if (fallThroughDisableCounter > 0 && player.components.rb.velocity.y <= 0)
                return;

            solidCollider.enabled = PlayerManager.instance.components.collision.spearUnderPlayer;
        }

        public void FixedUpdate()
        {
            if (inObject) return;

            if (target != null)
            {
                Vector3 vectorToTarget = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = q;

                rb.velocity = transform.right * speed;
            }
            else
            {
                rb.velocity = transform.right * speed;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (spearRetracting && other.CompareTag("Player"))
            {
                OnSpearDestroyed?.Invoke();
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!spearRetracting && other.CompareTag("Ground"))
            {
                Instantiate(impactParticles, other.ClosestPoint(transform.position), Quaternion.identity);
                OnSpearHitObject?.Invoke();
            }
        }

        /// <Description> Custom Methods </Description>

        private void OnHitObject()
        {
            inObject = true;
            rb.bodyType = RigidbodyType2D.Static;

            FixRotation();
        }

        private void OnRetracted()
        {
            rb.bodyType = RigidbodyType2D.Kinematic;

            spearRetracting = true;

            inObject = false;

            solidCollider.enabled = false;
        }

        private void FixRotation()
        {
            RaycastHit2D checkRight = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, groundLayer);
            RaycastHit2D checkLeft = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, groundLayer);
            RaycastHit2D checkUp = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, groundLayer);
            RaycastHit2D checkDown = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);

            RaycastHit2D closestHorizontal = checkRight.distance < checkLeft.distance ? checkRight : checkLeft;
            RaycastHit2D closestVertical = checkUp.distance < checkDown.distance ? checkUp : checkDown;

            if (closestHorizontal.distance < closestVertical.distance)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, closestHorizontal.point.x - transform.position.x > 0 ? 180f : 0f);
            }

            /*var frontPos = transform.position;

            RaycastHit2D[] casts = new RaycastHit2D[4];

            casts[0] = Physics2D.Raycast(frontPos, Vector2.right, 4f, groundLayer);
            casts[1] = Physics2D.Raycast(frontPos, Vector2.left, 4f, groundLayer);
            casts[2] = Physics2D.Raycast(frontPos, Vector2.up, 4f, groundLayer);
            casts[3] = Physics2D.Raycast(frontPos, Vector2.down, 4f, groundLayer);

            Vector2 closestPoint = new Vector2(Mathf.Infinity, Mathf.Infinity);

            foreach (RaycastHit2D cast in casts) {
                if (Vector2.Distance(frontPos, cast.point) < Vector2.Distance(frontPos, closestHitPoint)) {
                    Debug.Log("Closest distance is " + cast.distance);
                    closestPoint = cast.point;
                    closestHitPoint = cast.point;
                }
            }

            Vector2 directionVector = (Vector3)closestPoint - frontPos;

            if (directionVector.x == 0) { // Spear is vertical
                if (directionVector.y > 0) { // Spear is facing down
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                } else { // Spear is facing up
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                }
            } else { // Spear is horizontal
                if (directionVector.x > 0) { // Spear is facing left
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                } else { // Spear is facing right
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }

                inSideWall = true;
            }*/
        }

        /*private Vector2 closestHitPoint;

        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(closestHitPoint, 0.4f);
            Gizmos.DrawWireSphere(transform.position, 0.4f);
        }*/
    }
}