using Root.Systems.Input;
using Root.Systems.States;
using UnityEditor;
using UnityEngine;

public class GameManagerWindow : EditorWindow
{
    private static GameStateManager gameStateManager;
    private static InputManager inputManager;

    private string status;

    GUIStyle headerStyle
    {
        get
        {
            GUIStyle style = new GUIStyle();
            headerStyle.fontStyle = FontStyle.Bold;
            return style;
        }
    }

    [MenuItem("Custom Editors/Game Manager")]
    public static void ShowWindow() => GetWindow<GameManagerWindow>("Game Manager");

    private void Awake() => GameStateManager.OnGameStateChanged += RenderGameManagerStats;

    private void OnGUI()
    {
        if (EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            gameStateManager = GameStateManager.instance;
            inputManager = InputManager.instance;

            RenderGameManagerStats();
            RenderInputManagerStats();
        }
        else
        {
            status = "Waiting for Editor to Play";
            GUILayout.Label(status, EditorStyles.boldLabel);
        }
    }

    private void RenderGameManagerStats(GameState gameState)
    {
        RenderGameManagerStats();
        Repaint();
    }

    private void RenderGameManagerStats()
    {
        BeginSection("Game State Manager");

        string gameState =
            "Game State: " +
            (gameStateManager == null ? "Inactive" : gameStateManager.currentGameState.ToString());

        GUILayout.Label(gameState);

        EndSection();
    }

    private void RenderInputManagerStats()
    {
        BeginSection("Input Manager");

        string horizontalInput = $"Horizontal Input: {(inputManager == null ? "Inactive" : inputManager.horizontalInput.ToString())}";
        string verticalInput = $"Vertical Input: {(inputManager == null ? "Inactive" : inputManager.verticalInput.ToString())}";

        GUILayout.Label(horizontalInput + "\n" + verticalInput);

        EndSection();
    }

    private void BeginSection(string header)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label(header, EditorStyles.boldLabel);
    }

    private void EndSection() => GUILayout.EndVertical();
}
