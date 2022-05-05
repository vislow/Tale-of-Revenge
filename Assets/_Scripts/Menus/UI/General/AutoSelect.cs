using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.General {
    public class AutoSelect : MonoBehaviour, IPointerEnterHandler {
        [SerializeField] private Selectable selectable;

        public void OnPointerEnter(PointerEventData eventData) {
            selectable.Select();
        }
    }
}