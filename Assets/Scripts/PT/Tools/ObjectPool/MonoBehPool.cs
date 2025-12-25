using UnityEngine;

namespace PT.Tools.ObjectPool
{
    public class MonoBehPool<T> : PoolBase<T> where T : MonoBehaviour
    {
        protected override T CreateObject()
        {
            var obj = GameObject.Instantiate(_prefab, _parent).GetComponent<T>();

            return obj;
        }
        protected override void OnGet(T obj)
        {
            obj.SetActive(true);
        }
        protected override void OnSet(T obj)
        {
            obj.SetActive(false);
        }
    }
}