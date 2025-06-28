using System.Collections.Generic;
using Root.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Root
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private MenuPage startMenu;
        [SerializeField] private MenuPage pauseMenu;
        [SerializeField] private List<MenuPage> menuPages = new List<MenuPage>();

        public MenuPage currentActivePage { get; private set; }

        private void Awake()
        {
            SwitchToInitalPage();
        }

        public void SwitchPage(MenuPage targetMenu)
        {
            // Disable all pages, enable the one you want
            if (targetMenu == null) return;

            if (targetMenu == startMenu && GameManager.gameState == GameState.Paused)
            {
                targetMenu = pauseMenu;
            }
            else if (targetMenu == pauseMenu && GameManager.gameState == GameState.Title)
            {
                targetMenu = startMenu;
            }

            DisableAllPages();
            EnablePage(targetMenu);

            if (targetMenu.firstSelectedObject == null) return;

            SetSelectedObject();
        }

        private void EnablePage(MenuPage menuPage)
        {
            menuPage.gameObject.SetActive(true);
            currentActivePage = menuPage;

            SetSelectedObject();
        }

        private void DisablePage(MenuPage menuPage)
        {
            menuPage.gameObject.SetActive(false);
        }

        private void DisableAllPages()
        {
            foreach (var page in menuPages)
            {
                DisablePage(page);
            }
        }

        private void SetSelectedObject()
        {
            EventSystem.current.SetSelectedGameObject(currentActivePage.firstSelectedObject);
        }

        [ContextMenu("Get menu pages")]
        [SerializeField]
        private void GetMenuPages()
        {
            menuPages.Clear();
            MenuPage[] newMenuPages = GetComponentsInChildren<MenuPage>(true);

            foreach (MenuPage pages in newMenuPages)
            {
                menuPages.Add(pages);
            }
        }

        [ContextMenu("Switch to initial page")]
        [SerializeField]
        private void SwitchToInitalPage()
        {
            SwitchPage(startMenu);
        }
    }
}