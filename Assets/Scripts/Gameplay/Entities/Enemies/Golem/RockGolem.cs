using GameManagement;
using UnityEngine;

namespace Entities.Enemies.Golem {
    public class RockGolem : MonoBehaviour {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator anim;
        [Space]
        [SerializeField] private float speed;
        [Space]
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private float ledgeCheckDistance;
        [SerializeField] private Vector2 ledgeCheckOffset;
        [SerializeField] private LayerMask groundMask;

        private int facingDirection = 1;
        private bool atWall;
        private bool atLedge;
        private bool wasAtLedge;
        private bool turning;

        private const string TurnAnim = "RockGolem_Turn";
        private const string WalkAnim = "RockGolem_Walk";

        private void Update() {
            //if (GameStateManager.CurrentGameState != GameState.Gameplay) return;

            HandleCollision();
            LedgeCheck();
        }

        private void FixedUpdate() {
            //if (GameStateManager.CurrentGameState != GameState.Gameplay) return;

            if (!turning)
                rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
            else
                rb.velocity = Vector2.zero;
        }

        private void HandleCollision() {
            wasAtLedge = atLedge;

            atWall = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, groundMask);

            var ledgeCheckOrigin = (Vector2)transform.position + new Vector2(ledgeCheckOffset.x * facingDirection, ledgeCheckOffset.y);
            atLedge = !Physics2D.Raycast(ledgeCheckOrigin, -Vector2.up, ledgeCheckDistance, groundMask);
        }

        private void LedgeCheck() {
            if ((!wasAtLedge && atLedge) || atWall) {
                turning = true;

                anim.Play(TurnAnim);
            }
        }

        public void Flip() {
            anim.Play(WalkAnim);
            facingDirection *= -1;
            transform.localScale = new Vector3(facingDirection, 1, 1);

            turning = false;
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                object[] args = new object[2];
                args[0] = 1;

                var direction = (other.transform.position - transform.position).normalized;
                var newDirection = new Vector2(direction.x < 0 ? -1 : 1, direction.y < 0 ? -1 : 1);
                args[1] = newDirection;

                other.SendMessage("Damage", args, SendMessageOptions.RequireReceiver);
            }
        }

        private void OnDrawGizmos() {
            if (atLedge)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;

            Gizmos.DrawLine(transform.position, (Vector2)transform.position + (Vector2.right * facingDirection * wallCheckDistance));

            var ledgeCheckOrigin = (Vector2)transform.position + new Vector2(ledgeCheckOffset.x * facingDirection, ledgeCheckOffset.y);
            Gizmos.DrawLine(ledgeCheckOrigin, new Vector2(ledgeCheckOrigin.x, ledgeCheckOrigin.y - ledgeCheckDistance));
        }
    }
}
