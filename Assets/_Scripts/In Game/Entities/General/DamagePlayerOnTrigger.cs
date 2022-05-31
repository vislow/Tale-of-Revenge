using UnityEngine;
using Root.Player.Components;

namespace Root.Entities.General
{
    public class DamagePlayerOnTrigger : MonoBehaviour
    {
        [SerializeField] private int damage = 1;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

            other.GetComponent<PlayerHealth>().Damage(damage, knockbackDirection);
        }
    }
}