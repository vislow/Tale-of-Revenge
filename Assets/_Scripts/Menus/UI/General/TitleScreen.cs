using Root.LevelManagement;
using UnityEngine;

namespace UI.General
{
    public class TitleScreen : MonoBehaviour
    {
        private Vector3 previousMousePosition;

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}