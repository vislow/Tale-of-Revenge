using UnityEngine;
using UnityEngine.EventSystems;
using Root.Data;
using Root.Utility;
using Root.UI.Pages;
using Root.Systems.Levels;
using Root.Systems.States;

namespace Root.UI.Gameplay
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
            SaveHelper.GetCurrentSave().levelData.currentLevel = Utils.GetActiveSceneIndex();
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
            SaveHelper.GetCurrentSave().levelData.currentLevel = Utils.GetActiveSceneIndex();
            SaveHelper.Save();

            LevelManager.instance.LoadLevel(0);
        }

        public void QuitToDesktop()
        {
            SaveHelper.GetCurrentSave().levelData.currentLevel = Utils.GetActiveSceneIndex();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}