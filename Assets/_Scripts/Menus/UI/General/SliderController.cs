using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.General {
    public class SliderController : MonoBehaviour {
        public Slider mainSlider;

        //Reference to new "RectTransform"(Child of FillArea).
        public RectTransform newFillRect;

        //Deactivates the old FillRect and assigns a new one.
        void Start() {
            mainSlider.fillRect.gameObject.SetActive(false);
            mainSlider.fillRect = newFillRect;
        }
    }
}