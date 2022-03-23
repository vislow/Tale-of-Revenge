using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.General {
    public class DamagePlayerOnTrigger : MonoBehaviour {
        [SerializeField] private int damage = 1;

        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                other.GetComponent<Player.Components.PlayerHealth>().Damage(damage, knockbackDirection);
            }
        }
    }
}