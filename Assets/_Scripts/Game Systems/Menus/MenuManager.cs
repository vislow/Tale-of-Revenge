using System;
using System.Collections.Generic;
using Root.Menus;
using Root.Menus.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Root
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private MenuPage startMenu;
        [SerializeField] private MenuPage pauseMenu;
        [SerializeField] private GameObject backgroundOverlay;
        [SerializeField] private List<MenuPage> menuPages = new List<MenuPage>();

        public MenuPage currentActivePage { get; private set; }

        private void Awake()
        {
            SwitchToInitalPage();
        }

        // Add and remove listeners for game state change
        private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
        private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

        // Disable all pages when initially loading into a gameplay scene
        private void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.Gameplay)
            {
                DisableAllPages();
            }
            else if (gameState == GameState.Paused)
            {
                SwitchPage(pauseMenu);
            }
            else if (gameState == GameState.Title)
            {
                SwitchPage(startMenu);
            }
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
            backgroundOverlay.SetActive(true);
            menuPage.gameObject.SetActive(true);
            currentActivePage = menuPage;

            SetSelectedObject();
        }

        private void DisablePage(MenuPage menuPage)
        {
            menuPage.gameObject.SetActive(false);

            if (NoPagesEnabled())
            {
                backgroundOverlay.SetActive(false);
            }
        }

        public void DisableAllPages()
        {
            foreach (var page in menuPages)
            {
                DisablePage(page);
            }
        }

        // Check if all pages are disabled, mainly for checking when to disable the background overlay
        private bool NoPagesEnabled()
        {
            foreach (var page in menuPages)
            {
                if (page.gameObject.activeInHierarchy) return false;
            }

            return true;
        }

        // Sets the currently selected UI item (button, slider, etc.)
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