using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Root.Player.Components
{
    public class VitalsHud : MonoBehaviour
    {
        [SerializeField] private bool debug;
        [Header("General")]
        [SerializeField] private Vector2 hudPosition;
        [SerializeField] private float uiSpacing = 0.1935f;
        [Header("Hearts")]
        [SerializeField] private Transform heartContainer;
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private Sprite fullHeart;
        [SerializeField] private Sprite emptyHeart;
        [Space]
        [SerializeField][Range(0, 1)] private float heartBarLerpSpeed;
        [Header("Spear")]
        [SerializeField] private Transform spearDisplayTransform;
        [SerializeField] private SpriteRenderer spearDisplay;
        [SerializeField] private Sprite spearAvailable;
        [SerializeField] private Sprite spearUnavailable;

        private List<Transform> heartList = new List<Transform>();
        private List<SpriteRenderer> heartRendererList = new List<SpriteRenderer>();

        private PlayerManager player => PlayerManager.instance;
        private PlayerHealth playerHealth => player.components.health;
        private Vector3 playerPos => player.components.center.position;

        private void Awake()
        {
            PlayerHealth.OnHealthChanged += UpdateHeartList;
            PlayerSpearManager.OnSpearUpdate += UpdateSpearDiplay;

            Vector3 position = spearDisplayTransform.position;
            spearDisplayTransform.position = new Vector3(position.x + (uiSpacing / 2), position.y - uiSpacing);
        }

        private void OnDestroy()
        {
            PlayerHealth.OnHealthChanged -= UpdateHeartList;
            PlayerSpearManager.OnSpearUpdate -= UpdateSpearDiplay;
        }

        private void Update() => PositionVitalsRelativeToPlayer();

        private void PositionVitalsRelativeToPlayer()
        {
            if (player == null) return;

            transform.position = playerPos + (Vector3)hudPosition;
        }

        private void UpdateSpearDiplay(bool spearActive)
        {
            spearDisplayTransform.gameObject.SetActive(player.components.spearManager.spearUnlocked);
            spearDisplay.sprite = spearActive ? spearUnavailable : spearAvailable;
        }

        #region Heart Display
        [ContextMenu("Update UI")]
        private void UpdateHeartList()
        {
            if (player == null) return;

            if (playerHealth.MaxHealth != heartList.Count)
            {
                ResizeHeartList();
            }

            UpdateHeartSprites();

            void UpdateHeartSprites()
            {
                int currentHealth = playerHealth.CurrentHealth;

                for (int i = 0; i < heartList.Count; i++)
                {
                    heartRendererList[i].sprite = i <= currentHealth ? fullHeart : emptyHeart;
                }
            }

            void ResizeHeartList()
            {
                int maxHealth = playerHealth.MaxHealth;
                int heartListSize = heartList.Count;

                if (maxHealth < heartListSize)
                {
                    heartList.RemoveRange(maxHealth, heartListSize - maxHealth);
                    heartRendererList.RemoveRange(maxHealth, heartListSize - maxHealth);
                }
                else if (maxHealth > heartListSize)
                {
                    heartList.AddRange(Enumerable.Repeat(default(Transform), maxHealth - heartListSize));
                    for (int i = heartListSize; i < maxHealth; i++)
                    {
                        heartList[i] = Instantiate(heartPrefab, Vector3.zero, Quaternion.identity, heartContainer).transform;
                    }

                    heartRendererList.AddRange(Enumerable.Repeat(default(SpriteRenderer), maxHealth - heartListSize));
                    for (int i = heartListSize; i < maxHealth; i++)
                    {
                        heartRendererList[i] = heartList[i].GetComponent<SpriteRenderer>();
                    }
                }
            }

            // For some reason this just deletes and recreates all heart gameobjects everytime health changes
            // heartList.Clear();

            // foreach (Transform child in transform)
            // {
            //     Destroy(child.gameObject);
            // }

            // for (int i = 0; i < playerHealth.MaxHealth; i++)
            // {
            //     GameObject newHeart = Instantiate(heartPrefab, Vector3.zero, Quaternion.identity, transform);
            //     SpriteRenderer heartSprite = newHeart.GetComponent<SpriteRenderer>();

            //     heartList.Add(newHeart.transform);
            //     heartSprite.sprite = playerHealth.CurrentHealth <= i ? emptyHeart : fullHeart;
            // }

            UpdateHeartPositioning();
        }

        private void UpdateHeartPositioning()
        {
            RepositionHearts();
            RepositionHeartBar();

            void RepositionHearts()
            {
                for (int i = 0; i < heartList.Count; i++)
                {
                    float previousChildPos = i == 0 ? 0f : heartList[i - 1].localPosition.x;
                    float relativePos = previousChildPos + uiSpacing;

                    heartList[i].localPosition = new Vector3(relativePos, 0f, 0f);
                }
            }

            void RepositionHeartBar()
            {
                float offset = heartList[heartList.Count - 1].localPosition.x / 2;

                foreach (var child in heartList)
                {
                    child.localPosition -= new Vector3(offset, 0, 0);
                }
            }
        }
        #endregion
    }
}