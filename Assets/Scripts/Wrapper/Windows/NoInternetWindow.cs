using PT.Tools.Windows;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Wrapper.Windows
{
    public class NoInternetWindow : WindowBase
    {
        [SerializeField] private Button okButton;

        private void Awake()
        {
            if (okButton) okButton.onClick.AddListener(Application.Quit);
        }
    }
}