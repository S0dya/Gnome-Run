using UnityEngine;

namespace EditorLogics
{
    public class ObjectArray : MonoBehaviour
    {
        [Tooltip("Objects, that will be instantiated in an array")] 
        public GameObject[] Prefabs;
        
        [Tooltip("Direction, of objects array. Calculated using objects bounds")] 
        public Vector3 Direction = Vector3.forward;
        
        [Tooltip("Amount of objects of an array")] 
        [Min(0)] public int Amount = 1;

        [Tooltip("Randomize queue of objects in the array")]
        public bool Randomize;
    }
}
