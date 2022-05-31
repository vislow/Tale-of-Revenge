using UnityEngine;
using UnityEngine.Events;

namespace Root.Entities.Enemies
{
    public class EnemyHealthController : MonoBehaviour
    {
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

        public void Damage(int damage, Vector2 knockbackDirection)
        {
            if (invincible) return;

            health -= damage;
            OnDamaged?.Invoke();

            if (takeKnockback)
            {
                rb.velocity = knockbackDirection * knockbackAmount;
            }

            if (health > 0) return;

            OnDead?.Invoke();
        }
    }
}
