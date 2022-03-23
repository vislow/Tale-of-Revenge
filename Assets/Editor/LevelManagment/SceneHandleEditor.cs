using UnityEngine;
using UnityEditor;

using LevelLoading.ScriptableObjects;

[CustomEditor(typeof(SceneHandle))]
public class SceneHandleEditor : Editor
{
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
        sceneHandle = (SceneHandle)target;

        GUILayout.Space(5f);
        DrawSceneReferenceField();

        if (sceneHandle == null) return;

        GUILayout.Space(5f);
        DrawPassagesList();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(sceneHandle);
        }
    }

    private void DrawSceneReferenceField()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Scene", GUILayout.MaxWidth(50f));

        sceneHandle.scene = EditorGUILayout.ObjectField(sceneHandle.scene, typeof(SceneAsset), true);

        GUILayout.EndHorizontal();
    }

    private void DrawPassagesList()
    {
        GUILayout.BeginVertical(listStyle);

        DrawPassageListHeader();

        for (int i = 0; i < sceneHandle.passages.Count; i++)
        {
            DrawPassageListElement(sceneHandle.passages[i], i);
        }

        GUILayout.EndVertical();
    }

    private void DrawPassageListHeader()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Level Passages", EditorStyles.boldLabel);
        DrawAddPassageButton();

        GUILayout.EndHorizontal();

        DrawHorizontalLine();
    }

    private void DrawPassageListElement(PassageHandle passage, int id)
    {
        if (passage == null) return;

        GUILayout.BeginVertical(listElementStyle);

        passage.passageId = id;

        if (passage.passageName == "" || passage.passageName == null)
            passage.passageName = "Passage" + id;

        passage.passageName = EditorGUILayout.TextField("Passage Name", passage.passageName);

        passage.targetScene = (SceneHandle)EditorGUILayout.ObjectField("Target Scene", passage.targetScene, typeof(SceneHandle), true);

        DrawTargetPassagesList(passage);

        GUILayout.Space(3f);

        if (GUILayout.Button("Delete Passage"))
        {
            AssetDatabase.RemoveObjectFromAsset(passage);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            sceneHandle.passages.Remove((PassageHandle)passage);
        }

        GUILayout.EndVertical();
    }

    private void DrawTargetPassagesList(PassageHandle passage)
    {
        if (passage == null || passage.targetScene == null || passage.targetScene.passages.Count == 0) return;

        string[] passageNames = new string[passage.targetScene.passages.Count];

        for (int i = 0; i < passageNames.Length; i++)
        {
            //if (passageNames[i] == null) continue;
            PassageHandle passageHandle = passage.targetScene.passages[i];

            if (passageHandle == null) continue;

            string passageName = passageHandle.passageName;

            if (passageName == null)
                passageName = "";

            passageNames[i] = passageName.ToString();
        }

        passage.targetPassageId = EditorGUILayout.Popup(passage.targetPassageId, passageNames);
    }

    private void DrawAddPassageButton()
    {
        if (GUILayout.Button("Add Passage"))
        {
            PassageHandle passageHandle = ScriptableObject.CreateInstance<PassageHandle>();
            passageHandle.name = "Passage" + sceneHandle.passages.Count;

            AssetDatabase.AddObjectToAsset(passageHandle, sceneHandle);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.SetDirty(sceneHandle);
            EditorUtility.SetDirty(passageHandle);

            sceneHandle.passages.Add((PassageHandle)passageHandle);
        }
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