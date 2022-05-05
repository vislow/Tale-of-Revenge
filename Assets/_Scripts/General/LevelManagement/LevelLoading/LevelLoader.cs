using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Root.GameManagement;
using Root.LevelManagement;
using Root.LevelLoading.ScriptableObjects;

namespace Root.LevelLoading
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader instance;

        public static Action OnLoadingStarted;
        public static Action OnLoadingFinished;

        private bool loadingLevel;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void LoadNextLevel(bool loadingScreen = true)
        {
            int nextSceneIndex = Utility.Utils.GetActiveSceneIndex() + 1;

            LoadLevel(nextSceneIndex, loadingScreen);
        }

        public void LoadLevel(PassageHandle targetLevel, bool loadingScreen = true)
        {
            if (targetLevel == null)
                return;

            StartLoadProcess(targetLevelHandle: targetLevel, loadingScreen);
        }

        public void LoadLevel(int nextLevelIndex, bool loadingScreen = true, PassageHandle passageHandle = null)
        {
            if (SceneManager.GetSceneByBuildIndex(nextLevelIndex) == null)
                return;

            StartLoadProcess(targetLevelIndex: nextLevelIndex, loadingScreen);
        }

        private void StartLoadProcess(int targetLevelIndex, bool loadingScreen)
            => StartCoroutine(LoadProcess(levelIndex: targetLevelIndex, loadingScreen: loadingScreen));

        private void StartLoadProcess(PassageHandle targetLevelHandle, bool loadingScreen)
            => StartCoroutine(LoadProcess(passageHandle: targetLevelHandle, loadingScreen: loadingScreen));

        private IEnumerator LoadProcess(int levelIndex = default, PassageHandle passageHandle = null, bool loadingScreen = true)
        {
            GameStateManager.SetState(GameState.Loading);

            TransitionManager transitionManager = TransitionManager.instance;

            transitionManager.FadeOut();

            // Wait for fade out to finish

            while (transitionManager.currentState != TransitionStates.FadeOutFinished)
                yield return null;


            if (loadingScreen)
            {
                // Start loading next level

                AsyncOperation loadOperation = new AsyncOperation();

                SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                if (passageHandle == null)
                {
                    loadOperation = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
                }
                else
                {
                    loadOperation = SceneManager.LoadSceneAsync(passageHandle.targetScene.scene.name, LoadSceneMode.Additive);
                }

                OnLoadingStarted?.Invoke();

                // Wait for level to finish loading 

                while (!loadOperation.isDone)
                    yield return null;

                // Unload loading screen & fade out

                SceneManager.UnloadSceneAsync(2);
            }
            else
            {
                // load the target level then fade in

                SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
            }

            TeleportPlayerToPassage(passageHandle);

            yield return new WaitForEndOfFrame();

            transitionManager.FadeIn();

            SetGameState();
        }

        private void SetGameState()
        {
            int sceneIndex = Utility.Utils.GetActiveSceneIndex();

            GameState gameState = default;

            switch (sceneIndex)
            {
                case 0: gameState = GameState.Preload; break;
                case 1: gameState = GameState.Title; break;
                case 2: gameState = GameState.Loading; break;
                default: gameState = GameState.Gameplay; break;
            }

            GameStateManager.SetState(gameState);
        }

        private void TeleportPlayerToPassage(PassageHandle passageHandle)
        {
            if (passageHandle != null)
            {
                PassageManager currentPassageManager = PassageManager.instance;

                if (currentPassageManager == null)
                {
                    Debug.Log("There is no available instance of the passage manager");

                    return;
                }

                int passageId = passageHandle.passageId;

                PassageController targetController = currentPassageManager.passages[passageId];

                if (targetController == null)
                {
                    Debug.Log($"Passage controller for passage #{passageId} cannot be found, you probably have to assign it in the passage manager");

                    return;
                }

                targetController.TeleportPlayer();
            }
        }
    }
}