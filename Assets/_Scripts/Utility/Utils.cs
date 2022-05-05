using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class Utils
    {
        public static int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;

        public static string GetActiveSceneName() => SceneManager.GetActiveScene().name;

        public static bool GetSceneExists(string sceneName) => SceneManager.GetSceneByName(sceneName) == null;

        public static Vector3 MouseWorldPosition
        {
            get
            {
                Camera mainCam = Camera.main;

                if (mainCam == null) return Vector3.zero;

                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);

                return mainCam.ScreenToWorldPoint(position);
            }
        }
    }
}
