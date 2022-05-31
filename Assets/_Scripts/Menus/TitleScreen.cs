using Root.Systems.Levels;
using UnityEngine;

namespace Root.Systems
{
    public class TitleScreen : MonoBehaviour
    {
        private Vector3 previousMousePosition;

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