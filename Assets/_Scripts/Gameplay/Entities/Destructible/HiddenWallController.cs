using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Root.Entities.Destructible
{
    public class HiddenWallController : MonoBehaviour
    {
        [SerializeField][Range(0, 1)] private float fadeTime = 0.5f;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Tilemap regionTilemap;
        [Space]
        [SerializeField] private UnityEvent OnFadeComplete;

        [ContextMenu("Uncover Region")]
        public void UncoverRegion()
        {
            if (spriteRenderer == null) return;

            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }

        [ContextMenu("Cover Region")]
        public void CoverRegion()
        {
            if (spriteRenderer == null) return;

            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeOut()
        {
            while (spriteRenderer.color.a != 0)
            {
                if (spriteRenderer.color.a < 0.01)
                {
                    spriteRenderer.color = Color.clear;
                    regionTilemap.color = Color.clear;
                }

                spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 0, fadeTime));

                regionTilemap.color = new Color(1, 1, 1, Mathf.Lerp(regionTilemap.color.a, 0, fadeTime));

                yield return null;
            }

            OnFadeComplete?.Invoke();
        }

        private IEnumerator FadeIn()
        {
            while (spriteRenderer.color.a != 1)
            {
                if (spriteRenderer.color.a > 0.99)
                {
                    spriteRenderer.color = Color.white;
                    regionTilemap.color = Color.white;
                }

                spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 1, fadeTime));

                regionTilemap.color = new Color(1, 1, 1, Mathf.Lerp(regionTilemap.color.a, 1, fadeTime));

                yield return null;
            }
        }
    }
}