using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Root.Systems.Levels
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;

        public static Action OnLoadingStarted;
        public static Action OnLoadingFinished;

        private bool loadingLevel;

        private void Awake()
        {
            if (instance == null) instance = this; else Destroy(this);
        }

        public void LoadNextLevel()
        {
            int nextSceneIndex = Utility.Utils.GetActiveSceneIndex() + 1;

            LoadLevel(nextSceneIndex);
        }

        public void LoadLevel(PassageHandle targetLevel)
        {
            if (targetLevel == null) return;

            StartLoadProcess(targetLevelHandle: targetLevel);
        }

        public void LoadLevel(int nextLevelIndex, PassageHandle passageHandle = null)
        {
            if (SceneManager.GetSceneByBuildIndex(nextLevelIndex) == null) return;

            StartLoadProcess(targetLevelIndex: nextLevelIndex);
        }

        private void StartLoadProcess(int targetLevelIndex) => StartCoroutine(LoadProcessBehaviour.LoadProcess(levelIndex: targetLevelIndex));

        private void StartLoadProcess(PassageHandle targetLevelHandle) => StartCoroutine(LoadProcessBehaviour.LoadProcess(passageHandle: targetLevelHandle));
    }
}