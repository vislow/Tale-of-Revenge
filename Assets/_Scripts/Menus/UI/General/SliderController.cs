using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Root.UI.General
{
    public class SliderController : MonoBehaviour
    {
        public Slider mainSlider;
        public RectTransform newFillRect;

        void Start()
        {
            mainSlider.fillRect.gameObject.SetActive(false);
            mainSlider.fillRect = newFillRect;
        }
    }
}