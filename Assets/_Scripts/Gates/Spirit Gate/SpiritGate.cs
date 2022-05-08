using UnityEngine;
using System.Collections;

namespace Root.Gates
{
    public class SpiritGate : MonoBehaviour
    {
        [SerializeField] private float gateLiftSpeed;
        [SerializeField] private float gateLiftDistance = 4f;
        [SerializeField] private Rigidbody2D rb;

        private Vector3 openPosition;
        internal bool isOpen;

        private void Awake()
        {
            openPosition = transform.localPosition + new Vector3(0, gateLiftDistance);
        }

        private void FixedUpdate()
        {
            rb.bodyType = isOpen ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;

            if (!isOpen) return;

            var position = transform.localPosition;

            transform.localPosition = Vector3.MoveTowards(position, openPosition, gateLiftSpeed);
        }
    }
}
