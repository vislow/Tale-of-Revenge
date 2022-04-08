using UnityEngine;

using Utility;
using LevelLoading;

namespace GameManagement.Preload
{
    public class App : MonoBehaviour
    {
        /// <Description> Variables </Description>

        public static App instance;

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

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if (Utils.GetActiveSceneIndex() != 0)
                return;

            LevelLoader.instance.LoadNextLevel(loadingScreen: false);
        }
    }
}