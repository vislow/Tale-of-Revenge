using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class GlobalFunctions : MonoBehaviour
    {
        public static int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;

        public static string GetActiveSceneName() => SceneManager.GetActiveScene().name;

        public static bool GetSceneExists(string sceneName) => SceneManager.GetSceneByName(sceneName) == null;
    }
}
