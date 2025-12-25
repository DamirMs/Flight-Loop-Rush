using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.IOS.PrivacyPolicy
{
    public class PrivacyPolicyWindow : WindowBase
    {
        [SerializeField] private Button[] closeButtons;
        
        [Inject(Id = "Menu")] private WindowsManager _windowsManager; 

        private void Awake()
        {
            foreach (var button in closeButtons)
            {
                if (button) button.onClick.AddListener(() => _windowsManager.Close(WindowTypeEnum.MenuPrivacyPolicy).Forget());
            }
        }
    }
}