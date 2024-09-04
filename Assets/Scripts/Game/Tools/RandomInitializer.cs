using UnityEngine;

namespace EditorLogics
{
    public class RandomInitializer : MonoBehaviour
    {
        [SerializeField] MeshFilter meshFilter;

        public GameObject[] ObjectsToInstantiate;
        [Range(0.01f, 1)] public float SpawnChance = 1;
        [Min(0)] public float Step = 1;
        public float YOffset = 1;
        public bool Rotated;

        public MeshFilter GetMesh()
        {
            return meshFilter != null ? meshFilter : GetComponent<MeshFilter>();
        }

        private void OnDrawGizmosSelected()
        {
            MeshFilter meshFilter = GetMesh();

            if (meshFilter != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(meshFilter.transform.position, meshFilter.transform.TransformPoint(meshFilter.sharedMesh.bounds.size));
            }
        }
    }
}
