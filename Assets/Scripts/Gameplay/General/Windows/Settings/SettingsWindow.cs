using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.General.Windows.Settings
{
    public class SettingsWindow : WindowBase
    {
        [SerializeField] private Button[] backButtons;
        [SerializeField] private SettingLineView soundLine;
        [SerializeField] private SettingLineView vibroLine;

        [Inject(Id = "Menu")] private WindowsManager _windowsManager;
        
        private void Awake()
        {
            foreach (var backButton in backButtons) backButton.onClick.AddListener(OnBack);

            soundLine.Init("Sound", "SoundEnabled", true);
            vibroLine.Init("Vibration", "VibrationEnabled", true);
        }
        
        private void OnBack()
        {
            _windowsManager.Close(WindowTypeEnum.MenuSettings).Forget();
        }
    }
}