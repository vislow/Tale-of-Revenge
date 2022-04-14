using UnityEngine;

using Utility;
using LevelLoading;

// <Description> 
// This script is the overarching management script for the App Object
// The App Object (located in the resources folder) holds all of the 
// games persistant objects and scripts
// </Description>
namespace GameManagement.Preload
{
    public class App : MonoBehaviour
    {
        public static App instance;

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
            // If the current scene is not 0, return, otherwise, load the first scene
            if (Utils.GetActiveSceneIndex() != 0) return;

            LevelLoader.instance.LoadNextLevel(loadingScreen: false);
        }
    }
}