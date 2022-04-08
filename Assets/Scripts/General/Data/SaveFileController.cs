using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

using Data;
using LevelLoading;

public class SaveFileController : MonoBehaviour
{
    /// <Description> Variables </Description>

    [SerializeField] private SaveNames fileName;
    [Space]
    public GameObject clearSaveButton;
    public GameObject startButton;

    private TextMeshProUGUI startButtonText;
    private SaveData saveData;
    private bool hasData;

    /// <Description> Methods </Description>
    /// <Description> Unity Methods </Description>

    private void Awake()
    {
        hasData = SaveHelper.GetSaveExists(fileName);

        startButtonText = startButton.GetComponent<TextMeshProUGUI>();

        SetButtonVisibility();
    }

    /// <Description> Custom Methods </Description>

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
            SaveHelper.CurrentSave.levelData.currentLevel = 3;
            SaveHelper.Save();
        }

        LoadSave();
    }

    public void LoadSave()
    {
        SaveHelper.Load(fileName);
        LevelLoader.instance.LoadLevel(SaveHelper.CurrentSave.levelData.currentLevel, true);
    }

    public void ClearSave()
    {
        SaveHelper.ClearSaveData(fileName);

        hasData = SaveHelper.GetSaveExists(fileName);

        SetButtonVisibility();

        EventSystem.current.SetSelectedGameObject(startButton);
    }
}
