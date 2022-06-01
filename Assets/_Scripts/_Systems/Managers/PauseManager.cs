using Root.Data;
using Root.Systems.Input;
using Root.Systems.Levels;
using Root.Systems.Pages;
using Root.Systems.States;
using Root.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Root.Systems
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager instance;

        [SerializeField] private GameObject pauseContainer;
        [SerializeField] private GameObject onPauseSelectedObject;
        [SerializeField] private MenuPageController mainPageController;
        [SerializeField] private MenuManager menuManager;

        internal bool GamePaused => GameStateManager.CurrentGameState == GameState.Paused;

        private PlayerControls input => InputManager.instance.input;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);

            Initialize();

            void Initialize()
            {
                if (GamePaused)
                {
                    UnPause();
                }

                pauseContainer.SetActive(false);
            }
        }

        private void Start()
        {
            input.UI.Pause.performed += context => OnPauseButtonPressed();
        }

        private void OnApplicationQuit()
        {
            SaveHelper.GetCurrentSave().levelData.currentLevel = Utils.GetActiveSceneIndex();
            SaveHelper.Save();
        }

        private void OnPauseButtonPressed()
        {
            if (!GamePaused)
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
            if (GamePaused)
            {
                GameStateManager.SetState(GameState.Gameplay);
            }

            pauseContainer.SetActive(false);
        }

        public void Pause(bool openPauseUI = true)
        {
            pauseContainer.SetActive(true);

            EventSystem eventSystem = EventSystem.current;

            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(onPauseSelectedObject);

            GameStateManager.SetState(GameState.Paused);
        }

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