using PT.Tools.Effects;
using UnityEngine;

namespace PT.Tools.ObjectPool
{
    public class ParticleSystemPool : PoolBase<ParticleSystem>
    {
        protected override ParticleSystem CreateObject()
        {
            var ps = GameObject.Instantiate(_prefab, _parent).GetComponent<ParticleSystem>();

            return ps;
        }
        protected override void OnGet(ParticleSystem ps)
        {
            ps.SetActive(true);
            ps.Play();
        }
        protected override void OnSet(ParticleSystem ps)
        {
            ps.Clear();
            ps.SetActive(false);
        }
    }
}