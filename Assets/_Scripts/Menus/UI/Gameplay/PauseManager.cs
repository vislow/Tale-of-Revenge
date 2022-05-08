using UnityEngine;
using UnityEngine.EventSystems;

using Root.Data;
using Utility;
using UI.Pages;
using Root.LevelLoading;
using Root.GameManagement;

namespace UI.Gameplay
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject onPauseSelectedObject;
        [SerializeField] private MenuPageController mainPageController;
        [SerializeField] private MenuManager menuManager;

        private PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();

            controls.UI.Pause.performed += context => OnCancel();

            if (!IsPaused()) return;

            UnPause();
        }

        private void Start()
        {
            if (!pauseMenu.activeSelf) return;

            pauseMenu.SetActive(false);
        }

        private void OnEnable() => controls.Enable();

        private void OnDisable() => controls.Disable();

        private void OnApplicationQuit()
        {
            SaveHelper.CurrentSave.levelData.currentLevel = Utils.GetActiveSceneIndex();
            SaveHelper.Save();
        }

        private void OnCancel()
        {
            if (!IsPaused())
            {
                Pause();
                return;
            }

            var currentPage = menuManager.currentActivePage;

            if (currentPage != mainPageController)
            {
                currentPage.backButton.ChangePage();
                return;
            }

            UnPause();
        }

        public void UnPause()
        {
            GameStateManager.SetState(GameState.Gameplay);

            pauseMenu.SetActive(false);
        }

        public void Pause()
        {
            pauseMenu.SetActive(true);

            EventSystem eventSystem = EventSystem.current;

            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(onPauseSelectedObject);

            GameStateManager.SetState(GameState.Paused);
        }

        private bool IsPaused() => GameStateManager.CurrentGameState == GameState.Paused;

        public void QuitToMainMenu()
        {
            SaveHelper.CurrentSave.levelData.currentLevel = Utils.GetActiveSceneIndex();
            SaveHelper.Save();

            LevelManager.instance.LoadLevel(0);
        }

        public void QuitToDesktop()
        {
            SaveHelper.CurrentSave.levelData.currentLevel = Utils.GetActiveSceneIndex();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}