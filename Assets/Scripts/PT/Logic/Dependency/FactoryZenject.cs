using UnityEngine;
using Zenject;

namespace PT.Logic.Dependency
{
    public interface IFactoryZenject
    {
        GameObject InstantiateObject(GameObject obj, Transform parent);
        T InstantiateForComponent<T>(GameObject prefab, Transform parent) where T : Component;
    }

    public class FactoryZenject : IFactoryZenject
    {
        private readonly DiContainer _container;

        public FactoryZenject(DiContainer container)
        {
            _container = container;
        }

        public GameObject InstantiateObject(GameObject prefab, Transform parent)
        {
            var go = _container.InstantiatePrefab(prefab, parent);
            _container.InjectGameObject(go);
            return go;
        }

        public T InstantiateForComponent<T>(GameObject prefab, Transform parent) where T : Component
        {
            return _container.InstantiatePrefabForComponent<T>(prefab, parent);
        }
    }
}