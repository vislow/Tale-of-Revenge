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
        private SaveManager saveManager { get => SaveManager.instance; }

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

        private void Awake()
        {
            controls = new PlayerControls();

            controls.UI.Pause.performed += context => OnCancel();

            if (IsPaused())
            {
                UnPause();
            }
        }

        private void Start()
        {
            if (pauseMenu.activeSelf)
                pauseMenu.SetActive(false);
        }

        private void OnEnable()
            => controls.Enable();

        private void OnDisable()
            => controls.Disable();

        private void OnApplicationQuit()
        {
            saveManager.currentSave.levelData.currentLevel = GlobalFunctions.GetActiveSceneIndex();
            saveManager.Save();
        }

        /// <Description> Custom Methods </Description>

        private void OnCancel()
        {
            if (IsPaused())
            {
                if (menuManager.currentActivePage == mainPageController)
                {
                    UnPause();
                }
                else
                {
                    menuManager.currentActivePage.backButton.ChangePage();
                }
            }
            else
            {
                Pause();
            }
        }

        public void UnPause()
        {
            GameStateManager.SetState(GameState.Gameplay);
            pauseMenu.SetActive(false);
        }

        public void Pause()
        {
            pauseMenu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(onPauseSelectedObject);

            GameStateManager.SetState(GameState.Paused);
        }

        private bool IsPaused()
            => GameStateManager.CurrentGameState == GameState.Paused;

        public void QuitToMainMenu()
        {
            saveManager.currentSave.levelData.currentLevel = GlobalFunctions.GetActiveSceneIndex();
            saveManager.Save();

            LevelLoader.instance.LoadLevel(1);
        }

        public void QuitToDesktop()
        {
            saveManager.currentSave.levelData.currentLevel = GlobalFunctions.GetActiveSceneIndex();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}