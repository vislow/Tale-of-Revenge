using Root.Levels;
using UnityEngine;

namespace Root
{
    public class TitleScreen : MonoBehaviour
    {
        public void StartGame()
        {
            LevelManager.instance.LoadLevel(2);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}