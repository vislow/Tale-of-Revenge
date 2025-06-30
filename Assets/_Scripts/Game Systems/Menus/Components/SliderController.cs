using UnityEngine;
using UnityEngine.UI;

namespace Root.Menus.Components
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