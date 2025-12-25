using UnityEngine;

namespace PT.Tools.Drawings
{
    [ExecuteInEditMode]
    public class DebugGizmosDrawer : MonoBehaviour
    {
        [SerializeField] private bool enableDebugDraw;

        private void OnValidate()
        {
            DebugDrawer.Enabled = enableDebugDraw;
        }

        private void OnDrawGizmos()
        {
            if (!enableDebugDraw) return;
            DebugDrawer.UpdateGizmos();
        }
    }
}
