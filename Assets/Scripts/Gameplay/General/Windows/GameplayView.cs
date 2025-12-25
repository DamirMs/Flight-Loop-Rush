using Cysharp.Threading.Tasks;
using Gameplay.Current.ChickenSkies;
using PT.Tools.EventListener;
using PT.Tools.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.General.Windows
{
    public class GameplayView : MonoBehaviourEventListener
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button menuButton;
        [Space]
        [SerializeField] private GameObject pauseObj;
        [Space]
        [Header("Boost UI")]
        [SerializeField] private Button boostButton;
        [SerializeField] private Image boostCooldownFill;

        [Inject] private PlaneController _planeController;
        [Inject(Id = "Game")] private WindowsManager _windowsManager; 

        private void Awake()
        {
            if (menuButton) menuButton.onClick.AddListener(OpenMenu);
            if (pauseButton) pauseButton.onClick.AddListener(OpenPause);
            
            if (boostButton) boostButton.onClick.AddListener(OnBoostPressed);
        }

        private void OpenMenu()
        {
            _windowsManager.CloseAll().Forget();
            _windowsManager.Open(WindowTypeEnum.GameOver).Forget();
                
            GlobalEventBus.On(GlobalEventEnum.GameEnded);
        }
        private void OpenPause()
        {
            _windowsManager.CloseAll().Forget();
            _windowsManager.Open(WindowTypeEnum.GameOver).Forget();
                
            GlobalEventBus.On(GlobalEventEnum.GameEnded);
            
            // GlobalEventBus.On(GlobalEventEnum.GameMenuOpened);
        }
        
        private void OnBoostPressed()
        {
            if (!_planeController.IsBoostAvailable)
                return;

            _planeController.TryBoost();
            RunBoostCooldown().Forget();
        }
        
        private async UniTaskVoid RunBoostCooldown()
        {
            boostButton.interactable = false;

            float cd = _planeController.BoostCooldown;
            float t = cd;

            boostCooldownFill.fillAmount = 1f;

            while (t > 0f)
            {
                t -= Time.unscaledDeltaTime;
                boostCooldownFill.fillAmount = t / cd;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            boostCooldownFill.fillAmount = 0f;
            boostButton.interactable = true;
        }
    }
}