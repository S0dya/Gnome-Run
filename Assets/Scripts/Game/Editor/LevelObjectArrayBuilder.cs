using UnityEngine;
using UnityEditor;

namespace EditorLogics
{
    [CustomEditor(typeof(LevelObjectArray))]
    public class LevelObjectArrayBuilder : ObjectArrayBuilder
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelObjectArray = (LevelObjectArray)target;

            if (GUILayout.Button("Create Levels"))
            {
                ClearObjects(levelObjectArray.transform);

                for (int i = 0; i < levelObjectArray.LevelsAmount; i++)
                {
                    var levelPartGo = new GameObject("LevelPart");
                    levelPartGo.transform.parent = levelObjectArray.transform;

                    CreateArray(levelObjectArray.Prefabs, levelObjectArray.Direction,
                        levelObjectArray.Amount, levelObjectArray.Randomize, levelPartGo.transform);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
