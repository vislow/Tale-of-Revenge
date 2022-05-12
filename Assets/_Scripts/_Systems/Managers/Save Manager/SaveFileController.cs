using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Root.Systems.Levels;

namespace Root.Data
{
    public class SaveFileController : MonoBehaviour
    {
        [SerializeField] private SaveNames fileName;
        [Space]
        [SerializeField] private GameObject clearSaveButton;
        [SerializeField] private GameObject startButton;

        private TextMeshProUGUI startButtonText;
        private SaveData saveData;
        private bool hasData;

        private void Awake()
        {
            hasData = SaveHelper.GetSaveExists(fileName);
            startButtonText = startButton.GetComponent<TextMeshProUGUI>();

            SetButtonVisibility();
        }

        private void SetButtonVisibility()
        {
            clearSaveButton.SetActive(hasData);
            startButtonText.text = hasData ? "Continue" : "New Game";
        }

        public void StartGame()
        {
            if (!hasData)
            {
                SaveHelper.Save(fileName);
                SaveHelper.GetCurrentSave().levelData.currentLevel = 2;
                SaveHelper.Save();
            }

            LoadSave();
        }

        public void LoadSave()
        {
            SaveHelper.Load(fileName);
            LevelManager.instance.LoadLevel(SaveHelper.GetCurrentSave().levelData.currentLevel);
        }

        public void ClearSave()
        {
            SaveHelper.ClearSaveData(fileName);
            hasData = SaveHelper.GetSaveExists(fileName);

            SetButtonVisibility();

            EventSystem.current.SetSelectedGameObject(startButton);
        }
    }
}