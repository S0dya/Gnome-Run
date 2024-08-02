using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Delete Saves", GUILayout.Width(200), GUILayout.Height(20)))
        {
            PlayerPrefs.DeleteAll();
            File.Delete(Path.Combine(Application.persistentDataPath, "GameData.json"));
            if (File.Exists(Path.Combine(Application.dataPath, @"YandexGame\WorkingData\Editor\SavesEditorYG.json"))) File.Delete(Path.Combine(Application.dataPath, @"YandexGame\WorkingData\Editor\SavesEditorYG.json"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
