using System.Collections;
using UnityEngine;

namespace Entities.General {
    public class HitFlash : MonoBehaviour {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float hitFlashDuration = 0.2f;
        [Space]
        [SerializeField] private ParticleSystem hitParticles;

        public void StartFlash() {
            StartCoroutine(FlashTimer());
        }

        private IEnumerator FlashTimer() {
            if (spriteRenderer == null) {
                spriteRenderer = GetComponent<SpriteRenderer>();
                Debug.Log($"Please manually set reference to sprite renderer in the inspector ({gameObject.name})");
            }

            var material = spriteRenderer.material;

            if (hitParticles != null) {
                var emission = hitParticles.emission;
                hitParticles.Play();
            }

            material.SetFloat("_IsHit", 1.0f);

            yield return new WaitForSeconds(hitFlashDuration);

            material.SetFloat("_IsHit", 0.0f);
        }
    }
}
