using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [System.Serializable]
    public class GameData
    {
        [SerializeField] public Dictionary<string, int> IntDict = new();
        [SerializeField] public Dictionary<string, int[]> IntsDict = new();
     
        [SerializeField] public Dictionary<string, bool> BoolDict = new();
    }
}