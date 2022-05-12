using System.Collections;
using UnityEngine;

namespace Root.Player.Components
{
    public class PlayerKnockbackManager : PlayerComponent
    {
        [SerializeField] private Vector2 defaultKnockbackDirection;
        [Space]
        [SerializeField] private float defaultKnockbackTime;
        [SerializeField] private float defaultKnockbackForce;

        internal bool inKnockback;

        private void Update()
        {
            if (deathManager.dead)
            {
                StopAllCoroutines();
                inKnockback = false;
            }
        }

        [ContextMenu("Knockback player")]
        protected void Knockback() => StartCoroutine(ApplyKnockback(defaultKnockbackDirection, defaultKnockbackForce, defaultKnockbackTime));

        public void Knockback(Vector2 direction) => StartCoroutine(ApplyKnockback(direction, defaultKnockbackForce, defaultKnockbackTime));

        public void Knockback(Vector2 direction, float force, float time) => StartCoroutine(ApplyKnockback(direction, force, time));

        private IEnumerator ApplyKnockback(Vector2 direction, float force, float time)
        {
            if (deathManager.dead) yield return null;

            inKnockback = true;

            while (time > 0)
            {
                time -= Time.deltaTime;

                rb.velocity = direction * force;

                yield return null;
            }

            rb.velocity = direction;
            inKnockback = false;
        }

        public void SelfKnockback(Vector2 direction, float force)
        {
            if (deathManager.dead) return;

            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}