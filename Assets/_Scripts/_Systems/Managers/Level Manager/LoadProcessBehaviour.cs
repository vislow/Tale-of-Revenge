using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Root.GameManagement;
using Root.LevelManagement;
using Root.LevelLoading.ScriptableObjects;

namespace Root.LevelLoading
{
    public class LoadProcessBehaviour
    {
        /// <Description>
        /// 1. Fade out 
        /// 2. Unload current scene 
        /// 3. Load new scene
        /// 4. Fade in
        ///</Description>
        public static IEnumerator LoadProcess(int levelIndex = default, PassageHandle passageHandle = null)
        {
            TransitionManager transitionManager = TransitionManager.instance;
            GameStateManager.SetState(GameState.Loading);
            transitionManager.FadeOut();

            // Wait for fade out to finish
            while (transitionManager.currentState != TransitionStates.FadeOutFinished) yield return null;

            // Start loading next level
            AsyncOperation loadOperation = new AsyncOperation();

            LevelManager.OnLoadingStarted?.Invoke();

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

            if (passageHandle == null)
            {
                loadOperation = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
            }
            else
            {
                loadOperation = SceneManager.LoadSceneAsync(passageHandle.targetScene.scene.name, LoadSceneMode.Additive);
            }

            // Wait for level to finish loading 
            while (!loadOperation.isDone) yield return null;

            TeleportPlayerToPassage(passageHandle);
            SceneManager.UnloadSceneAsync(1);
            LevelManager.OnLoadingFinished?.Invoke();

            yield return new WaitForEndOfFrame();

            transitionManager.FadeIn();
            UpdateGameState();
        }

        private static void UpdateGameState()
        {
            int sceneIndex = Utility.Utils.GetActiveSceneIndex();

            GameState gameState = default;

            switch (sceneIndex)
            {
                case 0: gameState = GameState.Title; break;
                case 1: gameState = GameState.Loading; break;
                default: gameState = GameState.Gameplay; break;
            }

            GameStateManager.SetState(gameState);
        }

        public static void TeleportPlayerToPassage(PassageHandle passageHandle)
        {
            if (passageHandle == null) return;

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
