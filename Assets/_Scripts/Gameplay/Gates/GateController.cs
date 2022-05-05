using System.Collections;
using UnityEngine;

namespace Root
{
    public class GateController : MonoBehaviour
    {
        [SerializeField] private float gateSpeed = 0.3f;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Vector3 originalPos;

        private void Start()
        {
            originalPos = transform.position;
        }

        public void Open(bool open)
        {
            float targetYPos = open ? spriteRenderer.bounds.size.y + originalPos.y : originalPos.y;

            Vector2 targetPos = new Vector3(originalPos.x, targetYPos);

            StartCoroutine(MoveGate(targetPos));
        }

        private IEnumerator MoveGate(Vector2 targetPos)
        {
            while (Vector2.Distance(transform.position, targetPos) > 0.0001f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, gateSpeed);

                yield return null;
            }

            transform.position = targetPos;
        }
    }
}