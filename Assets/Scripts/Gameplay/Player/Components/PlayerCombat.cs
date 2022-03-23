using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Entities.Enemies;

namespace Player.Components
{
    public class PlayerCombat : PlayerComponent
    {
        /// <Description> Variables </Description>

        [SerializeField] private bool debug;
        [Space]
        [SerializeField] private int attackDamage = 1;
        [SerializeField] private float timeBetweenAttacks = 0.1f;
        [Space]
        [SerializeField] private AttackDetails[] attacks;

        [System.Serializable]
        public class AttackDetails
        {
            public bool debug;
            public Vector2 position;
            public Vector2 hitboxSize;
            public CapsuleDirection2D hitboxDirection;
            public AttackDirections attackDirection;
            public float selfKnockback;
        }

        public enum AttackDirections
        {
            Side, Up, Down,
        }

        private List<GameObject> enemiesInAttack = new List<GameObject>();
        private float attackCooldownTimer;
        private float capturedInputDirection;
        private bool hasAppliedSelfKnockback;

        internal bool attacking;
        internal int currentAttackIndex;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Update()
            => attackCooldownTimer -= Time.deltaTime;

        /// <Description> Custom Methods </Description>

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (knockback.inKnockback) return;

            if (context.performed && attackCooldownTimer <= 0)
            {
                if (controller.verticalInput == -1 && !collision.grounded)
                {
                    Attack(2);
                }
                else if (controller.verticalInput == 1)
                {
                    Attack(1);
                }
                else
                {
                    Attack(0);
                }
            }
        }

        private void Attack(int attackIndex)
        {
            currentAttackIndex = attackIndex;
            AttackDetails attack = attacks[attackIndex];
            Attack(attack.position, attack.hitboxSize, attack.hitboxDirection, attack.attackDirection);
        }

        private void Attack(Vector2 position, Vector2 hitboxSize, CapsuleDirection2D hitboxDirection, AttackDirections attackDirection)
        {
            attacking = true;

            enemiesInAttack.Clear();

            capturedInputDirection = controller.facingDirection;
            attackCooldownTimer = timeBetweenAttacks;
        }

        private void DamageEnemies()
        {
            AttackDetails attack = attacks[currentAttackIndex];

            Vector2 attackPos = transform.position + new Vector3(attack.position.x * capturedInputDirection, attack.position.y, 0f);

            Collider2D[] hit = Physics2D.OverlapCapsuleAll(attackPos, attack.hitboxSize, attack.hitboxDirection, 0f);

            foreach (Collider2D enemy in hit)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    EnemyHealthController enemyHealthController = enemy.GetComponent<EnemyHealthController>();


                    if (!hasAppliedSelfKnockback)
                    {
                        hasAppliedSelfKnockback = true;
                        knockback.SelfKnockback(-GetAttackDirectionVector(attack.attackDirection), attack.selfKnockback);
                    }

                    if (!enemyHealthController) continue;

                    if (enemiesInAttack.Contains(enemy.gameObject))
                    {
                        continue;
                    }

                    enemiesInAttack.Add(enemy.gameObject);

                    Vector2 knockbackDirection = GetAttackDirectionVector(attack.attackDirection);

                    enemyHealthController.Damage(attackDamage, knockbackDirection);
                }
            }
        }

        private Vector2 GetAttackDirectionVector(AttackDirections direction)
        {
            Vector2 directionVector = new Vector2();

            switch (direction)
            {
                case AttackDirections.Side: directionVector = new Vector2(controller.facingDirection, 0f); break;
                case AttackDirections.Up: directionVector = new Vector2(0, 1); break;
                case AttackDirections.Down: directionVector = new Vector2(0, -1); break;
            }

            return directionVector;
        }

        public void SetAttackingFalse()
        {
            attacking = false;
            hasAppliedSelfKnockback = false;
            enemiesInAttack.Clear();
        }

        private void OnDrawGizmos()
        {
            if (!debug) return;

            Gizmos.color = Color.red;

            foreach (var attack in attacks)
            {
                if (!attack.debug) continue;

                Vector2 newAttackPoint = attack.position;

                if (playerManager != null)
                    newAttackPoint.x *= controller.facingDirection;

                Vector2 pos = transform.position + (Vector3)newAttackPoint;

                Vector2 leftPos = pos;
                Vector2 rightPos = pos;

                if (attack.hitboxDirection == CapsuleDirection2D.Vertical)
                {
                    leftPos.y -= (attack.hitboxSize.y / 2) - attack.hitboxSize.x / 2;
                    rightPos.y += (attack.hitboxSize.y / 2) - attack.hitboxSize.x / 2;

                    Gizmos.DrawWireSphere(leftPos, attack.hitboxSize.x / 2);
                    Gizmos.DrawWireSphere(rightPos, attack.hitboxSize.x / 2);
                }
                else
                {
                    leftPos.x -= (attack.hitboxSize.x / 2) - attack.hitboxSize.y / 2;
                    rightPos.x += (attack.hitboxSize.x / 2) - attack.hitboxSize.y / 2;

                    Gizmos.DrawWireSphere(leftPos, attack.hitboxSize.y / 2);
                    Gizmos.DrawWireSphere(rightPos, attack.hitboxSize.y / 2);
                }
            }
        }
    }
}