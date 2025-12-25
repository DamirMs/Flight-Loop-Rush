using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.IOS.Tutorial
{
    public class TutorialWindow : WindowBase
    {
        [SerializeField] private Button[] closeButtons;
        
        [Inject(Id = "Menu")] private WindowsManager _windowsManager; 

        private void Awake()
        {
            foreach (var button in closeButtons)
            {
                if (button) button.onClick.AddListener(() => _windowsManager.Close(WindowTypeEnum.MenuTutorial).Forget());
            }
        }
    }
}