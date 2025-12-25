using System.Collections.Generic;
using UnityEngine;

namespace PT.Tools.ObjectPool
{
    public abstract class PoolBase<T>
    {
        private readonly Queue<T> _pool = new();

        private protected GameObject _prefab;
        private protected Transform _parent;

        private bool _overridesOnGet;
        private bool _overridesOnSet;

        public Queue<T> Init(GameObject prefab, Transform parent, int size, bool overridesOnGet = false, bool overridesOnSet = false)
        {
            _prefab = prefab;
            _parent = parent;
            _overridesOnGet = overridesOnGet;
            _overridesOnSet = overridesOnSet;

            for (int i = 0; i < size; i++)
            {
                T obj = CreateObject();
                Set(obj);
            }

            return _pool;
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                T obj = _pool.Dequeue();
                if (!_overridesOnGet) OnGet(obj);

                return obj;
            }

            return CreateObject();
        }

        public void Set(T obj)
        {
            if (_pool.Contains(obj)) return;
            if (!_overridesOnSet) OnSet(obj);
            _pool.Enqueue(obj);
        }

        protected abstract T CreateObject();
        protected abstract void OnGet(T obj);
        protected abstract void OnSet(T obj);
    }

    public class ObjectPool : PoolBase<GameObject>
    {
        protected override GameObject CreateObject()
        {
            return GameObject.Instantiate(_prefab, _parent);
        }
        protected override void OnGet(GameObject obj)
        {
            obj.SetActive(true);
        }
        protected override void OnSet(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}