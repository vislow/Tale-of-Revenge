using UnityEngine;

namespace Root.Entities.Enemies.Golem
{
    public class RockGolem : MonoBehaviour
    {
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

        private void Update()
        {
            HandleCollision();
            LedgeCheck();
        }

        private void FixedUpdate() => rb.linearVelocity = turning ? Vector2.zero : new Vector2(speed * facingDirection, rb.linearVelocity.y);

        private void HandleCollision()
        {
            wasAtLedge = atLedge;
            atWall = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, groundMask);

            Vector2 ledgeCheckOrigin = (Vector2)transform.position + new Vector2(ledgeCheckOffset.x * facingDirection, ledgeCheckOffset.y);

            atLedge = !Physics2D.Raycast(ledgeCheckOrigin, -Vector2.up, ledgeCheckDistance, groundMask);
        }

        private void LedgeCheck()
        {
            if ((wasAtLedge || !atLedge) && !atWall) return;

            turning = true;

            anim.Play(TurnAnim);
        }

        public void Flip()
        {
            anim.Play(WalkAnim);
            facingDirection *= -1;
            transform.localScale = new Vector3(facingDirection, 1, 1);
            turning = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            object[] args = new object[2];

            args[0] = 1;

            Vector3 direction = (other.transform.position - transform.position).normalized;
            Vector2 newDirection = new Vector2(direction.x < 0 ? -1 : 1, direction.y < 0 ? -1 : 1);

            args[1] = newDirection;

            other.SendMessage("Damage", args, SendMessageOptions.RequireReceiver);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = atLedge ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + (Vector2.right * facingDirection * wallCheckDistance));

            Vector2 ledgeCheckOrigin = (Vector2)transform.position + new Vector2(ledgeCheckOffset.x * facingDirection, ledgeCheckOffset.y);

            Gizmos.DrawLine(ledgeCheckOrigin, new Vector2(ledgeCheckOrigin.x, ledgeCheckOrigin.y - ledgeCheckDistance));
        }
    }
}
