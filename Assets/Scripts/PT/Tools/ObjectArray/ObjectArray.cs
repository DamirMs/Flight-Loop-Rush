using UnityEngine;

namespace PT.Tools.ObjectArray
{
    public class ObjectArray : MonoBehaviour
    {
        [Tooltip("Objects, that will be instantiated in an array")]
        [SerializeField] private GameObject[] prefabs;

        [Tooltip("Direction, of objects array. Calculated using objects bounds")]
        [SerializeField] private Vector3 direction = Vector3.forward;

        [Tooltip("Amount of objects of an array")]
        [Min(0)][SerializeField] private int amount = 1;

        [Tooltip("Randomize queue of objects in the array")]
        [SerializeField] private bool randomize;

        public GameObject[] Prefabs => prefabs;
        public Vector3 Direction => direction;
        public int Amount => amount;
        public bool Randomize => randomize;
    }
}
