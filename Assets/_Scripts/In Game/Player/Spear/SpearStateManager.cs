using Root.Entities;
using Root.Player.Components;
using UnityEngine;

namespace Root.Player.Spear
{
    public class SpearStateManager : StateMachine
    {
        public Rigidbody2D rb;
        [Space]
        public float speed;
        [Range(0, 1)] public float acceleration;
        public float playerDestroyDistance = 3f;
        [Space]
        public GameObject activeObjects;
        public GameObject stuckObjects;
        public GameObject returnObjects;
        [Space]
        public GameObject spearImpactParticles;

        private PlayerManager player;
        internal PlayerCollision PlayerCollision => player.components.collision;
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

        public void Init(PlayerManager playerManager) => this.player = playerManager;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (currentState != activeState || !other.CompareTag("Ground")) return;

            Instantiate(spearImpactParticles, other.ClosestPoint(transform.position), Quaternion.identity);
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
    }
}
