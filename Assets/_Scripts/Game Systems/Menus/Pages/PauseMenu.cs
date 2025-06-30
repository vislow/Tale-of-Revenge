using Root;
using Root.Data;
using Root.Input;
using Root.Levels;
using Root.Utility;
using UnityEngine;

namespace Root.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private MenuManager menuManager;

        public void ResumeGame()
        {
            GameManager.instance.ResumeGame();
        }

        public void QuitToMainMenu()
        {
            // SaveHelper.GetCurrentSave().levelData.currentLevel = Utils.GetActiveSceneIndex();
            // SaveHelper.Save();

            LevelManager.instance.LoadLevel(0);
        }

        public void QuitToDesktop()
        {
            SaveHelper.GetCurrentSave().levelData.currentLevel = Utils.GetActiveSceneIndex();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}