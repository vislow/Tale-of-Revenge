using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Root.UI.General
{
    public class HorizontalSelector : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private float arrowFlashTime = 0.1f;
        [SerializeField] private Selectable leftArrow;
        [SerializeField] private Selectable rightArrow;
        [Space]
        [SerializeField] private TextMeshProUGUI text;
        public List<string> optionList = new List<string>();

        private PlayerControls controls;
        private bool selected;

        private int currentIndex;
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (value == currentIndex) return;

                if (gameObject.activeInHierarchy)
                {
                    if (currentIndex < value)
                    {
                        StartCoroutine(FlashArrow(rightArrow));
                    }
                    else if (currentIndex > value)
                    {
                        StartCoroutine(FlashArrow(leftArrow));
                    }
                }

                if (value > optionList.Count - 1)
                {
                    value = 0;
                }
                else if (value < 0)
                {
                    value = optionList.Count - 1;
                }

                currentIndex = value;
                text.text = optionList[currentIndex].ToString();
                OnIndexChanged?.Invoke();
            }
        }

        public Action OnIndexChanged;

        private void Awake()
        {
            controls = new PlayerControls();

            controls.UI.Move.performed += context => OnMove(context.ReadValue<Vector2>().x);
        }

        private void Start() => text.text = optionList[CurrentIndex];

        private void OnEnable() => controls.Enable();

        private void OnDisable() => controls.Disable();

        public void OnSelect(BaseEventData eventData) => selected = true;

        public void OnDeselect(BaseEventData eventData) => selected = false;

        public void OnMove(float dir)
        {
            if (!selected) return;

            CurrentIndex += Mathf.RoundToInt(dir);
        }

        private IEnumerator FlashArrow(Selectable selectable)
        {
            selectable.targetGraphic.color = selectable.colors.selectedColor;

            yield return new WaitForSeconds(arrowFlashTime);

            selectable.targetGraphic.color = selectable.colors.normalColor;
        }
    }
}