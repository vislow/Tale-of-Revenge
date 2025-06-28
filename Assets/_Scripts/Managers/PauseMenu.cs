using Root;
using Root.Data;
using Root.Input;
using Root.Levels;
using Root.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Root
{
    public class PauseMenu : MonoBehaviour
    {
        public void ResumeGame()
        {

        }

        public void QuitToMainMenu()
        {
            SaveHelper.GetCurrentSave().levelData.currentLevel = Utils.GetActiveSceneIndex();
            SaveHelper.Save();

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