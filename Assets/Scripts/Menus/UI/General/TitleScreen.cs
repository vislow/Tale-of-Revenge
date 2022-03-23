using LevelManagement;
using UnityEngine;

namespace UI.General
{
    public class TitleScreen : MonoBehaviour
    {
        /// <Description> Variables </Description>
        
        private Vector3 previousMousePosition;

        /// <Description> Methods </Description>
        /// <Description> Custom Methods </Description>

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}