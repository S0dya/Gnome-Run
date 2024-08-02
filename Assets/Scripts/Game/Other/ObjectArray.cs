using UnityEngine;

namespace EditorLogics
{
    public class ObjectArray : MonoBehaviour
    {
        public GameObject[] Prefabs;
        public Vector3 Direction = Vector3.forward;
        public int Amount = 1;
        public bool Randomize;
    }
}
