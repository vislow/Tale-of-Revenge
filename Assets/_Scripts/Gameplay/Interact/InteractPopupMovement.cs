using UnityEngine;

namespace Root.Interact
{
    public class InteractPopupMovement : MonoBehaviour
    {
        [SerializeField] private float sinMagnitude = 1f;
        [SerializeField] private float sinSpeed = 1f;

        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.localPosition;
        }

        private void FixedUpdate()
        {
            transform.localPosition = initialPosition + new Vector3(0f, Mathf.Sin(Time.time * sinSpeed) * sinMagnitude, 0f);
        }
    }
}
