using Root;
using UnityEngine;

namespace Root.Gates
{
    public class SpiritGateTrigger : MonoBehaviour
    {
        [SerializeField] private SpiritGate spiritGate;
        [SerializeField] private Animator anim;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (GameManager.gameState != GameState.Gameplay || !other.CompareTag("Spear")) return;

            spiritGate.IsOpen = true;
            anim.SetTrigger("Activate");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (GameManager.gameState != GameState.Gameplay || !other.CompareTag("Spear")) return;

            spiritGate.IsOpen = false;
            anim.SetTrigger("Deactivate");
        }
    }
}
