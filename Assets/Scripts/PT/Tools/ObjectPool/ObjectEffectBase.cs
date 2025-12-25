using UnityEngine;

namespace PT.Tools.ObjectPool
{
    public abstract class ObjectEffectBase
    {
        protected GameObject _objectGameObject;

        protected ObjectEffectBase(GameObject objectGameObject)
        {
            _objectGameObject = objectGameObject;
        }

        public abstract void ActivateEffect();
        public abstract void DeactivateEffect();
    }
}