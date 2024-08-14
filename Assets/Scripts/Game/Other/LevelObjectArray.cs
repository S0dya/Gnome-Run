using UnityEngine;

namespace EditorLogics
{
    public class LevelObjectArray : ObjectArray
    {
        [Space] [Space] [Space]
        [Header("Level")]
        [Tooltip("Amount of object arrays to create")]
        [Min(0)] public int LevelsAmount = 1;

        [Tooltip("Amount of new Level Parts added in a sequance for object array")]
        [Min(0)] public int ProgressionValue = 0;

        [Tooltip("Direction of Levels object arrays")]
        public Vector3 LevelDirection = Vector3.zero;

        [Tooltip("Parent of the Level")]
        public GameObject LevelTemplatePrefab;

        public GameObject LevelStartPrefab;
        public GameObject FlagsPrefab;
        public GameObject[] GatesPrefabs;
    }
}
