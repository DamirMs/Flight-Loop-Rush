using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.IOS
{
    public class MenuAdditionalButtons : MonoBehaviour
    {
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button privacyPolicyButton;
        
        [Inject(Id = "Menu")] private WindowsManager _windowsManager; 
        
        private void Awake()
        {
            if (shopButton) shopButton.onClick.AddListener(OnShopPressed);
            if (tutorialButton) tutorialButton.onClick.AddListener(OnTutorialPressed);
            if (privacyPolicyButton) privacyPolicyButton.onClick.AddListener(() => _windowsManager.Open(WindowTypeEnum.MenuPrivacyPolicy).Forget());
        }
        
        private void OnShopPressed()
        {
            _windowsManager.Open(WindowTypeEnum.MenuShop).Forget();
        }
        private void OnTutorialPressed()
        {
            _windowsManager.Open(WindowTypeEnum.MenuTutorial).Forget();
        }
    }
}