using UnityEngine;
using UnityEngine.Events;

namespace Entities.Enemies {
    public class EnemyHealthController : MonoBehaviour {
        [SerializeField] private bool invincible;
        [SerializeField] private int health = 3;
        [Space]
        [SerializeField] private bool takeKnockback = true;
        [SerializeField] private float knockbackAmount = 3f;
        [Space]
        [SerializeField] private Rigidbody2D rb;
        [Space]
        [SerializeField] private UnityEvent OnDamaged;
        [SerializeField] private UnityEvent OnDead;

        public void Damage(int damage, Vector2 knockbackDirection) {
            if (!invincible)
                health -= damage;

            OnDamaged?.Invoke();

            if (rb != null && takeKnockback) {
                rb.velocity = knockbackDirection * knockbackAmount;
            }

            if (!invincible && health <= 0) {
                OnDead?.Invoke();
            }
        }
    }
}
