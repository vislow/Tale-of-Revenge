using UnityEngine;
using UnityEditor;

using GameManagement;

public class GameManagerWindow : EditorWindow
{
    private static GameStateManager gameStateManager;

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
    public static void ShowWindow()
    {
        GetWindow<GameManagerWindow>("Game Manager");
    }

    private void OnGUI()
    {
        gameStateManager = GameStateManager.instance;
        RenderGameManagerStats();
    }

    private void RenderGameManagerStats()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);

        GUILayout.Label("Game State Manager", EditorStyles.boldLabel);

        string gameState =
            "Game State: " +
            (gameStateManager == null ? "Inactive" : gameStateManager.currentGameState.ToString());

        GUILayout.Label(gameState);

        GUILayout.EndVertical();
    }
}
