using UnityEngine;
using UnityEditor;

namespace EditorLogics
{
    [CustomEditor(typeof(RandomInitializer))]
    public class RandomCreator : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            RandomInitializer randomInitializer = (RandomInitializer)target;

            if (GUILayout.Button("Clear Objects", GUILayout.Width(100), GUILayout.Height(20)))
            {
                ClearObjects(randomInitializer);
            }

            if (GUILayout.Button("Create Objects"))
            {
                ClearObjects(randomInitializer);

                var size = CalculateMeshSize(randomInitializer.GetMesh());

                CreateObjects(size[0], size[1], randomInitializer.ObjectsToInstantiate, randomInitializer.transform, 
                    randomInitializer.SpawnChance, randomInitializer.Step, randomInitializer.YOffset, randomInitializer.Rotated);
            }

            serializedObject.ApplyModifiedProperties();
        }
        private Vector3[] CalculateMeshSize(MeshFilter meshFilter)
        {
            Bounds bounds = meshFilter.sharedMesh.bounds;

            Vector3 min = meshFilter.transform.TransformPoint(bounds.min);
            Vector3 max = meshFilter.transform.TransformPoint(bounds.max);

            return new Vector3[2] { min, max };
        }

        private void CreateObjects(Vector3 startPos, Vector3 endPos, GameObject[] objs, Transform parent, float spawnChance, float step, float yOffset, bool rotated)
        {
            for (float i = startPos.x; i < endPos.x; i += 1 + step)
            {
                for (float j = startPos.z; j < endPos.z; j += 1 + step)
                {
                    if (spawnChance > Random.value)
                    {
                        var pos = new Vector3(i + Random.value, yOffset, j + Random.value);
                        var rot = rotated ? Quaternion.Euler(0, Random.Range(0, 360), 0) : Quaternion.identity;

                        Instantiate(objs[objs.Length == 1 ? 0 : Random.Range(0, objs.Length)], pos, rot, parent);
                    }
                }
            }
        }

        private void ClearObjects(RandomInitializer randomInitializer)
        {
            while (randomInitializer.transform.childCount > 0)
                foreach (Transform transform in randomInitializer.transform)
                    DestroyImmediate(transform.gameObject);
        }
    }
}