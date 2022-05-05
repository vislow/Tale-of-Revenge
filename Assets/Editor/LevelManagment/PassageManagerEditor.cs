using UnityEngine;
using UnityEditor;

using Root.LevelLoading;
using Root.LevelLoading.ScriptableObjects;

[CustomEditor(typeof(PassageManager))]
public class PassageManagerEditor : Editor
{
    private PassageManager passageManager;
    private SceneHandle sceneHandle;

    private GUIStyle listStyle
    {
        get
        {
            GUIStyle style = new GUIStyle();

            style = EditorStyles.helpBox;

            return style;
        }
    }

    private GUIStyle listElementStyle
    {
        get
        {
            GUIStyle style = new GUIStyle();

            style = EditorStyles.helpBox;
            style.padding = new RectOffset(10, 10, 10, 10);
            style.margin = new RectOffset(5, 5, 7, 7);

            return style;
        }
    }

    public override void OnInspectorGUI()
    {
        passageManager = (PassageManager)target;
        sceneHandle = passageManager.sceneHandle;

        GUILayout.Space(5f);
        DrawSceneHandleField();
        if (sceneHandle == null) return;
        GUILayout.Space(5f);
        DrawPassageControllerList();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(passageManager);
        }
    }

    private void DrawSceneHandleField()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Scene", GUILayout.MaxWidth(50f));

        passageManager.sceneHandle = (SceneHandle)EditorGUILayout.ObjectField(passageManager.sceneHandle, typeof(SceneHandle), true);

        GUILayout.EndHorizontal();
    }

    private void DrawPassageControllerList()
    {
        if (sceneHandle.scene == null) return;

        GUILayout.BeginVertical(listStyle);

        DrawControllerListHeader();

        for (int i = 0; i < sceneHandle.passages.Count; i++)
        {
            DrawPassageListElement(sceneHandle.passages[i], i);
        }

        GUILayout.EndVertical();
    }

    private void DrawControllerListHeader()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField($"{sceneHandle.scene.name} Passages", EditorStyles.boldLabel);

        GUILayout.EndHorizontal();

        DrawHorizontalLine();
    }

    private void DrawPassageListElement(PassageHandle passage, int passageIndex)
    {
        if (passage == null) return;

        GUILayout.BeginVertical(listElementStyle);

        EditorGUILayout.LabelField("Passage Name: " + passage.passageName);
        EditorGUILayout.LabelField("Target Level: " + passage.targetScene.scene.name);
        EditorGUILayout.LabelField("Target Passage: " + passage.targetPassageId);

        if (passageManager.passages.Length != sceneHandle.passages.Count)
        {
            passageManager.passages = new PassageController[sceneHandle.passages.Count];
        }

        passageManager.passages[passageIndex] = (PassageController)EditorGUILayout.ObjectField("Passage Controller: ", passageManager.passages[passageIndex], typeof(PassageController), true);

        var pageController = passageManager.passages[passageIndex];

        if (pageController != null)
            pageController.id = passageIndex;

        //DrawTargetPassagesList(passage);

        GUILayout.EndVertical();
    }

    private void DrawTargetPassagesList(PassageHandle passage)
    {
        if (passage == null || passage.targetScene == null) return;

        string[] passageNames = new string[passage.targetScene.passages.Count];

        for (int i = 0; i < passageNames.Length; i++)
        {
            passageNames[i] = passage.targetScene.passages[i].passageName.ToString();
        }

        passage.targetPassageId = EditorGUILayout.Popup(passage.targetPassageId, passageNames);
    }

    private void DrawHorizontalLine(int i_height = 1)
    {
        var spacing = 5f;

        GUILayout.Space(spacing);

        Rect rect = EditorGUILayout.GetControlRect(false, i_height);

        rect.height = i_height;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

        GUILayout.Space(spacing);
    }
}
