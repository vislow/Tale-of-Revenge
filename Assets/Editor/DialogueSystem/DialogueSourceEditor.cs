using UnityEngine;
using UnityEditor;
using Root.Systems.Dialogue;

[CustomEditor(typeof(DialogueSource))]
public class DialogueSourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueSource source = (DialogueSource)target;

        GUILayout.Space(10);
        GUILayout.Label("Dialogue Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Start"))
            source.StartDialogue();

        if (GUILayout.Button("Continue"))
            source.ContinueDialogue();

        if (GUILayout.Button("Close"))
            source.CloseDialogue();
    }
}
