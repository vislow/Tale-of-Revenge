using UnityEngine;

namespace Entities.Enemies.Testing {
    public class CombatBall : MonoBehaviour {
        [SerializeField] private Animator anim;
        [SerializeField] private Rigidbody2D rb;
        [Space]
        [SerializeField] private float knockbackForce;

        public void Damage(object[] args) {
            anim.SetTrigger("Hit");

            Vector2 knockbackDirection = (Vector2)args[1];
            Vector2 knockback = knockbackDirection * knockbackForce;

            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }
}