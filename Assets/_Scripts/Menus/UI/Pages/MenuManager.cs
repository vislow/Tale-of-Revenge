using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Pages {
    public class MenuManager : MonoBehaviour {
        [SerializeField] private bool debug;
        [Space]
        [SerializeField] private MenuPageController initialPage;
        [SerializeField] private List<MenuPageController> pageControllers = new List<MenuPageController>();

        private PlayerControls controls;
        internal MenuPageController currentActivePage;

        [ContextMenu("Populate Page Controller List")]
        protected void GetPageControllers() {
            pageControllers.Clear();

            MenuPageController[] controllers = FindObjectsOfType<MenuPageController>();

            foreach (MenuPageController controller in controllers) {
                pageControllers.Add(controller);
            }
        }

        private void Awake() {
            InitializeControls();
            InitializeMenus();
        }

        private void InitializeMenus() {
            PageChanger.OnChangePage += ChangePage;

            foreach (MenuPageController controller in pageControllers) {
                controller.gameObject.SetActive(false);
            }

            if (initialPage == null) {
                initialPage = pageControllers[0];

                if (initialPage == null) {
                    Debug.LogError("There are no available menu pages, please populate the menu manager's page list");
                    return;
                }
            }

            ActivatePage(initialPage);
        }

        private void InitializeControls() {
            controls = new PlayerControls();
            controls.UI.Cancel.performed += context => OnCancelPressed();
        }

        private void OnEnable() {
            controls.Enable();
        }

        private void OnDisable() {
            controls.Disable();
        }

        public void ChangePage(MenuPageController targetPage) {
            if (targetPage == null) {
                Debug.Log("Target page was null");
                return;
            }

            if (currentActivePage != null)
                currentActivePage.gameObject.SetActive(false);
            targetPage.gameObject.SetActive(true);

            currentActivePage = targetPage;

            if (targetPage.firstSelectedObject == null) return;

            StartCoroutine(SetSelectedObject());
        }

        private void ActivatePage(MenuPageController controller) {
            controller.gameObject.SetActive(true);

            currentActivePage = controller;

            StartCoroutine(SetSelectedObject());
        }

        private IEnumerator SetSelectedObject() {
            while (EventSystem.current == null) {
                yield return new WaitForEndOfFrame();
                yield return null;
            }

            EventSystem.current.SetSelectedGameObject(null);

            yield return new WaitForEndOfFrame();

            EventSystem.current.SetSelectedGameObject(currentActivePage.firstSelectedObject);
        }

        private void OnDestroy() {
            PageChanger.OnChangePage -= ChangePage;
        }

        private void OnCancelPressed() {
            if (currentActivePage.backButton == null) return;

            currentActivePage.backButton.ChangePage();
        }
    }
}