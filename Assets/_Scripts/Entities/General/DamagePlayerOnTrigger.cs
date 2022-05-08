using UnityEngine;

namespace Root.Entities.General
{
    public class DamagePlayerOnTrigger : MonoBehaviour
    {
        [SerializeField] private int damage = 1;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                other.GetComponent<Root.Player.Components.PlayerHealth>().Damage(damage, knockbackDirection);
            }
        }
    }
}