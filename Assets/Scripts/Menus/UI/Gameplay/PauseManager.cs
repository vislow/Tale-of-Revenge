using UnityEngine;
using UnityEngine.EventSystems;

using Data;
using Utils;
using UI.Pages;
using LevelLoading;
using GameManagement;

namespace UI.Gameplay
{
    public class PauseManager : MonoBehaviour
    {
        /// <Description> Variables </Description>

        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject onPauseSelectedObject;
        [SerializeField] private MenuPageController mainPageController;
        [SerializeField] private MenuManager menuManager;

        private PlayerControls controls;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

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

        private void OnEnable()
            => controls.Enable();

        private void OnDisable()
            => controls.Disable();

        private void OnApplicationQuit()
        {
            SaveHelper.CurrentSave.levelData.currentLevel = GlobalFunctions.GetActiveSceneIndex();
            SaveHelper.Save();
        }

        /// <Description> Custom Methods </Description>

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

        private bool IsPaused()
            => GameStateManager.CurrentGameState == GameState.Paused;

        public void QuitToMainMenu()
        {
            SaveHelper.CurrentSave.levelData.currentLevel = GlobalFunctions.GetActiveSceneIndex();
            SaveHelper.Save();

            LevelLoader.instance.LoadLevel(1);
        }

        public void QuitToDesktop()
        {
            SaveHelper.CurrentSave.levelData.currentLevel = GlobalFunctions.GetActiveSceneIndex();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}