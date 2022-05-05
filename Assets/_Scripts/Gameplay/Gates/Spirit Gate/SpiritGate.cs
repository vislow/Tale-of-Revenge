using UnityEngine;
using System.Collections;

namespace Root.Gates
{
    public class SpiritGate : MonoBehaviour
    {
        [SerializeField] private float gateSpeed;
        [SerializeField] public bool isOpen;

        private Vector3 initialPosition;
        private Vector3 openPosition;

        private void Awake()
        {
            initialPosition = transform.localPosition;
            openPosition = initialPosition + new Vector3(0, 4);
        }

        private void Update()
        {
            var position = transform.localPosition;

            transform.localPosition = Vector3.Lerp(position, isOpen ? openPosition : initialPosition, gateSpeed);
        }
    }
}
