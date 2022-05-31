using System;
using UnityEngine;

namespace Root.Systems.Pages
{
    public class PageChanger : MonoBehaviour
    {
        [SerializeField] private MenuPageController targetPage;

        public static event Action<MenuPageController> OnChangePage;

        public void ChangePage() => OnChangePage?.Invoke(targetPage);
    }
}