using UnityEngine;

namespace EditorLogics
{
    public class LevelObjectArray : ObjectArray
    {
        [Header("Level")]
        public int LevelsAmount = 1;

        public GameObject LevelTemplatePrefab;

        public GameObject LevelStartPrefab;
        public GameObject FlagsPrefab;
        public GameObject[] GatesPrefabs;
    }
}
