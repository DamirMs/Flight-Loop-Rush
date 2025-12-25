using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.General.Windows.Settings
{
    [Serializable]
    public class SettingLineView 
    {
        [SerializeField] private Button toggleButton;
        [SerializeField] private Image stateImage;
        [SerializeField] private Sprite enabledSprite;
        [SerializeField] private Sprite disabledSprite;
        [SerializeField] private TMP_Text label;

        private bool _isEnabled;
        private string _playerPrefsKey;

        public void Init(string settingName, string playerPrefsKey, bool defaultValue)
        {
            label.text = settingName;
            _playerPrefsKey = playerPrefsKey;
        
            _isEnabled = PlayerPrefs.GetInt(_playerPrefsKey, defaultValue ? 1 : 0) == 1;
            UpdateView();

            toggleButton.onClick.RemoveAllListeners();
            toggleButton.onClick.AddListener(Toggle);
        }

        private void Toggle()
        {
            _isEnabled = !_isEnabled;
            PlayerPrefs.SetInt(_playerPrefsKey, _isEnabled ? 1 : 0);
            UpdateView();
        }

        private void UpdateView()
        {
            stateImage.sprite = _isEnabled ? enabledSprite : disabledSprite;
        }
    }
}