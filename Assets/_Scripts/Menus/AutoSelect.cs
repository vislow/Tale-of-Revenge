using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Root.UI
{
    public class AutoSelect : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Selectable selectable;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => selectable.Select();
    }
}