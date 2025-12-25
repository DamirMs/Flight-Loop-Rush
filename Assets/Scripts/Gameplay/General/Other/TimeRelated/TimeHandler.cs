using PT.Tools.EventListener;

namespace Gameplay.General.Other.TimeRelated
{
    public class TimeHandler : MonoBehaviourEventListener
    {
        private readonly TimeToggler _timeToggler = new();
        
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted},
                { GlobalEventEnum.GameMenuOpened, OnGameMenuOpened},
                { GlobalEventEnum.GameMenuClosed, OnGameMenuClosed},
                { GlobalEventEnum.GameEnded, OnGameEnded},
                { GlobalEventEnum.GameLeaveToMenu, OnGameLeaveToMenu},
                { GlobalEventEnum.GameUpgradesOpened, OnGameUpgradesOpened},
                { GlobalEventEnum.GameUpgradesClosed, OnGameUpgradesClosed},
            });
        }
        
        private void OnGameStarted()
        {
            _timeToggler.ToggleTime(true);
        }
        private void OnGameMenuClosed()
        {
            _timeToggler.ToggleTime(true);
        }
        private void OnGameMenuOpened()
        {
            _timeToggler.ToggleTime(false);
        }
        private void OnGameEnded()
        {
            _timeToggler.ToggleTime(false);
        }
        private void OnGameLeaveToMenu()
        {
            _timeToggler.ToggleTime(true);
        }
        private void OnGameUpgradesOpened()
        {
            _timeToggler.ToggleTime(false);
        }
        private void OnGameUpgradesClosed()
        {
            _timeToggler.ToggleTime(true);
        }
    }
}