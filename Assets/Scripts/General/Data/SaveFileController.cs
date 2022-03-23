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

    private SaveManager saveManager { get => SaveManager.instance; }
    private TextMeshProUGUI startButtonText;
    private SaveData saveData;
    private bool hasData;

    /// <Description> Methods </Description>
    /// <Description> Unity Methods </Description>

    private void Awake()
    {
        hasData = SaveManager.GetSaveExists(fileName);

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
            saveManager.Save(fileName);
            saveManager.currentSave.levelData.currentLevel = 3;
            saveManager.Save();
        }

        LoadSave();
    }

    public void LoadSave()
    {
        saveManager.Load(fileName);
        LevelLoader.instance.LoadLevel(saveManager.currentSave.levelData.currentLevel, true);
    }

    public void ClearSave()
    {
        saveManager.ClearSaveData(fileName);

        hasData = SaveManager.GetSaveExists(fileName);

        SetButtonVisibility();

        EventSystem.current.SetSelectedGameObject(startButton);
    }
}
