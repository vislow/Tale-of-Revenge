using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using GameManagement;
using LevelManagement;
using LevelLoading.ScriptableObjects;

namespace LevelLoading
{
    public class LevelLoader : MonoBehaviour
    {
        /// <Description> Variables </Description>

        public static LevelLoader instance;

        public static Action OnLoadingStarted;
        public static Action OnLoadingFinished;

        private bool loadingLevel;

        /// <Description> Methods </Description>
        /// <Description> Unity Methods </Description>

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

        /// <Description> Custom Methods </Description>

        public void LoadNextLevel(bool loadingScreen = true)
        {
            int nextSceneIndex = Utils.GlobalFunctions.GetActiveSceneIndex() + 1;

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

            /// <Description> Wait for fade out to finish </Description>

            while (transitionManager.currentState != TransitionStates.FadeOutFinished)
                yield return null;


            if (loadingScreen)
            {
                /// <Description> Start loading next level </Description>

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

                /// <Description> Wait for level to finish loading </Description>

                while (!loadOperation.isDone)
                    yield return null;

                /// <Description> Unload loading screen & fade out </Description>

                SceneManager.UnloadSceneAsync(2);
            }
            else
            {
                /// <Description> load the target level then fade in </Description>

                SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
            }

            TeleportPlayerToPassage(passageHandle);

            yield return new WaitForEndOfFrame();

            transitionManager.FadeIn();

            SetGameState();
        }

        private void SetGameState()
        {
            int sceneIndex = Utils.GlobalFunctions.GetActiveSceneIndex();

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