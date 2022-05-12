using UnityEngine;

namespace Root
{
    public class LeverController : MonoBehaviour
    {
        [SerializeField] private GateController gate;

        private bool isOpen;

        private void Damage()
        {
            isOpen = !isOpen;
            gate.Open(isOpen);
        }
    }
}