using Root.Player;
using UnityEngine;

namespace Root.Entities.General
{
    public class DamagePlayerOnTrigger : MonoBehaviour
    {
        [SerializeField] private int damage = 1;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

            var playerHealth = other.GetComponent<PlayerManager>().components.health;
            playerHealth.Damage(damage, knockbackDirection);
        }
    }
}