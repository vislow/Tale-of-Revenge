using System.Collections;
using Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Root.Levels
{
    public class LoadProcessBehaviour
    {
        /// <Description>
        /// 1. Fade out 
        /// 2. Unload current scene 
        /// 3. Load new scene
        /// 4. Fade in
        /// </Description>
        public static IEnumerator LoadProcess(int levelIndex = default, PassageHandle passageHandle = null)
        {
            TransitionManager transitionManager = TransitionManager.instance;
            transitionManager.FadeOut();

            // Wait for fade out to finish
            while (transitionManager.currentState != TransitionStates.FadeOutFinished) yield return null;

            GameManager.SetState(GameState.Loading);

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
                var targetScene = passageHandle.targetScene.sceneName;
                loadOperation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
            }

            // Wait for level to finish loading 
            while (!loadOperation.isDone) yield return null;

            TeleportPlayerToTargetPassage(passageHandle);

            SceneManager.UnloadSceneAsync(1);
            LevelManager.OnLoadingFinished?.Invoke();

            yield return new WaitForEndOfFrame();

            transitionManager.FadeIn();
            UpdateGameState();
        }

        private static void UpdateGameState()
        {
            int sceneIndex = Utility.Utils.GetActiveSceneIndex();

            GameState gameState;

            switch (sceneIndex)
            {
                case 0: gameState = GameState.Title; break;
                case 1: gameState = GameState.Loading; break;
                default: gameState = GameState.Gameplay; break;
            }

            GameManager.SetState(gameState);
        }

        public static void TeleportPlayerToTargetPassage(PassageHandle passageHandle)
        {
            PassageManager passageManager = PassageManager.instance;

            if (passageManager == null)
            {
                ConsoleLog("There is no available instance of the passage manager");
                return;
            }

            if (passageHandle == null)
            {
                // ConsoleLog("No passage handle found");
                return;
            }

            // Debug.Log(passageHandle.targetPassageId);

            int targetPassageId = passageHandle.targetPassageId;
            PassageController targetController = passageManager.passages[targetPassageId];

            if (targetController == null)
            {
                ConsoleLog($"Passage controller for passage #{targetPassageId} cannot be found, you probably have to assign it in the passage manager");
                return;
            }

            targetController.TeleportPlayer();
        }

        [ContextMenu("Run Log Test")]
        private void LogTest() => ConsoleLog("Log test");
        private static void ConsoleLog(string message) => Utility.Utils.ConsoleLog(LevelManager.instance, message);
    }
}
