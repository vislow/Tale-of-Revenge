using System.Collections.Generic;
using UnityEngine;

namespace Player.Components
{
    public class VitalsHud : MonoBehaviour
    {
        /// <Description> Variables </Description>

        [SerializeField] private bool debug;
        [Space]
        [SerializeField] private float spacing = 0.1935f;
        [SerializeField][Range(0, 1)] private float heartLerpSpeed;
        [Space]
        [SerializeField] private Sprite fullHeart;
        [SerializeField] private Sprite emptyHeart;
        [SerializeField] private GameObject heartPrefab;
        [Space]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Vector3 positionOffset;

        private List<Transform> childList = new List<Transform>();

        /// <Description> Methods </Description>

        private void Awake() => PlayerHealth.OnHealthChanged += UpdateChildList;

        private void OnDestroy() => PlayerHealth.OnHealthChanged -= UpdateChildList;

        private void Update()
        {
            if (playerTransform == null) return;

            transform.position = playerTransform.position + positionOffset;
        }

        private void OnValidate()
        {
            if (!debug || playerTransform == null) return;

            transform.position = playerTransform.position + positionOffset;
        }

        [ContextMenu("Update UI")]
        private void UpdateChildList()
        {
            if (PlayerManager.instance == null) return;

            PlayerHealth playerHealth = PlayerManager.instance.components.health;

            if (playerHealth == null) return;

            childList.Clear();

            if (this == null) return;

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < playerHealth.MaxHealth; i++)
            {
                GameObject newHeart = Instantiate(heartPrefab, Vector3.zero, Quaternion.identity, transform);

                childList.Add(newHeart.transform);

                SpriteRenderer heartSprite = newHeart.GetComponent<SpriteRenderer>();

                heartSprite.sprite = playerHealth.CurrentHealth <= i ? emptyHeart : fullHeart;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            for (int i = 0; i < childList.Count; i++)
            {
                var previousChildPos = i == 0 ? 0f : childList[i - 1].localPosition.x;
                float relativePos = previousChildPos + spacing;

                childList[i].localPosition = new Vector3(relativePos, 0f, 0f);
            }

            float offset = childList[childList.Count - 1].localPosition.x / 2;

            foreach (var child in childList)
            {
                child.localPosition -= new Vector3(offset, 0, 0);
            }
        }
    }
}