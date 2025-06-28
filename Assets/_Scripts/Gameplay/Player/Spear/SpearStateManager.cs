using Root.Entities;
using UnityEngine;
using UnityEngine.UIElements;

namespace Root.Player.Spear
{
    public class SpearStateManager : StateMachine
    {
        public Rigidbody2D rb;
        [Space]
        public float speed;
        [Range(0, 1)] public float acceleration;
        public float playerDestroyDistance = 3f;
        public float spearReturnTime = 3f;
        public float hitDetectDistance = 0.3f;
        [Header("State Objects")]
        public GameObject activeObjects;
        [Space]
        public GameObject stuckObjects;
        public Collider2D stuckObjectCollider;
        public SpriteRenderer stuckObjectRenderer;
        public Color fadedColor;
        public LayerMask groundMask;
        [Space]
        public GameObject returnObjects;
        [Header("Effects")]
        public GameObject spearImpactParticles;

        internal PlayerManager player;
        internal Vector2 PlayerPos => player.components.center.position;

        private Active activeState;
        private Stuck stuckState;
        private Returning returningState;
        internal Inactive inactiveState;

        private void Awake()
        {
            activeState = new Active(this);
            stuckState = new Stuck(this);
            returningState = new Returning(this);
            inactiveState = new Inactive(this);

            ChangeState(activeState);
        }

        /// <summary>
        ///  FIX THIS I JUST WANT IT TO DETECT WHEN A WALL OR THE GROUND HAS ENTERED THE TIP OF THE SPEAR
        /// </summary>
        // void FixedUpdate()
        // {
        //     RaycastHit2D spearRay = Physics2D.Raycast(transform.position + new Vector3(-hitDetectDistance, 0f), transform.right, Mathf.Infinity, groundMask);
        //     if (spearRay.distance < hitDetectDistance)
        //     {
        //         SetSpearStuck(transform.position);
        //     }
        // }

        public void Init(PlayerManager playerManager) => this.player = playerManager;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (currentState != activeState || !other.CompareTag("Ground")) return;
            SetSpearStuck(other.ClosestPoint(transform.position));
        }

        private void SetSpearStuck(Vector2 impactPoint)
        {
            Instantiate(spearImpactParticles, impactPoint, Quaternion.identity);
            ChangeState(stuckState);
        }

        public void SetObjectActivity(bool activeObj, bool stuckObj, bool returnObj)
        {
            activeObjects.SetActive(activeObj);
            stuckObjects.SetActive(stuckObj);
            returnObjects.SetActive(returnObj);
        }

        public void RotateTowardsPlayer()
        {
            var vectorToTarget = (transform.position - (Vector3)PlayerPos).normalized;
            var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = rotation;
        }

        public void DestroySelf(float time = 0f)
        {
            player.components.spearManager.isSpearActive = false;
            Destroy(gameObject, time);
        }

        public void ReturnToPlayer() => ChangeState(returningState);

        /// THIS SEEMS KINDA CORRECT TO BE HONEST IM CONFUSED
        // void OnDrawGizmos()
        // {
        //     RaycastHit2D spearRay = Physics2D.Raycast(transform.position + new Vector3(-hitDetectDistance, 0f), transform.right, Mathf.Infinity, groundMask);
        //     Gizmos.DrawLine(spearRay.transform.position + new Vector3(-hitDetectDistance, 0f), spearRay.point);
        // }
    }
}
