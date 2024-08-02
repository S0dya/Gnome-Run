using System.Collections.Generic;
using UnityEngine;

namespace LevelsRelated 
{
    [CreateAssetMenu(menuName = "Data/Levels List")]
    public class SO_LocationAndLevels : ScriptableObject
    {
        public bool randomizedLevels;
        public List<Level> levelsList;

        public GameObject locationPrefab;
    }
}