using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.IOS.Menu
{
    public class MenuBootstrapper : MonoBehaviour
    {
        [Inject(Id = "Menu")] private WindowsManager _windowsManager; 
        
        private void Start()
        {
            _windowsManager.Open(WindowTypeEnum.Menu).Forget();
        }
    }
}