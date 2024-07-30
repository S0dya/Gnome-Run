using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Lvls List")]
public class SO_LevelsList : ScriptableObject
{
    public bool randomizedLvls;
    public List<Level> lvls;
}
