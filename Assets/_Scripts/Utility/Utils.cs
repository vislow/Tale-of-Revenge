using UnityEngine;
using UnityEngine.SceneManagement;

namespace Root.Utility
{
    public class Utils
    {
        public static void ConsoleLog(Object source, string message) => Debug.Log($"<b>{source.name}</b>: {message}", source);
        public static void ConsoleLog(string source, string message) => Debug.Log($"<b>{source}</b>: {message}");

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
