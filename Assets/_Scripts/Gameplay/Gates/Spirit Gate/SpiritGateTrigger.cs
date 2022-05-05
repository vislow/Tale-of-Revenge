using UnityEngine;

namespace Root.Gates
{
    public class SpiritGateTrigger : MonoBehaviour
    {
        [SerializeField] private SpiritGate spiritGate;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Spear")) return;

            spiritGate.isOpen = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Spear")) return;

            spiritGate.isOpen = false;
        }
    }
}
