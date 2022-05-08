using System;
using UnityEngine;
using Root.Player;

namespace Root.Entities.Projectiles.Player
{
    public class SpearProjectile : BaseProjectile
    {

        [SerializeField] private Collider2D solidCollider;
        [SerializeField] private LayerMask groundLayer = 1 << 6;
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

            if (!inObject || player == null) return;

            fallThroughDisableCounter -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.S))
            {
                solidCollider.enabled = false;
                fallThroughDisableCounter = fallThroughDisableTime;

                return;
            }

            if (fallThroughDisableCounter > 0 && player.components.rb.velocity.y <= 0) return;

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
            }

            rb.velocity = transform.right * speed;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!spearRetracting || !other.CompareTag("Player")) return;

            OnSpearDestroyed?.Invoke();
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (spearRetracting || !other.CompareTag("Ground")) return;

            Instantiate(impactParticles, other.ClosestPoint(transform.position), Quaternion.identity);
            OnSpearHitObject?.Invoke();
        }

        private void OnHitObject()
        {
            inObject = true;
            rb.bodyType = RigidbodyType2D.Static;

            //FixRotation();
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
        }
    }
}