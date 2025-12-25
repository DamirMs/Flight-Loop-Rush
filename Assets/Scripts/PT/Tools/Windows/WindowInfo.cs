using System;
using UnityEngine;

namespace PT.Tools.Windows
{
    [Serializable]
    public class WindowInfo
    {
        [SerializeField] private WindowTypeEnum type;
        [SerializeField] private WindowBase prefab;
        
        public WindowTypeEnum Type => type;
        public WindowBase Prefab => prefab;
    }
}