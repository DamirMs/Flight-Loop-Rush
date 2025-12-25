using PT.Tools.EventListener;
using UniRx;

namespace Gameplay.General.Other
{
    public class TurnCounter : MonoBehaviourEventListener
    {
        public IReadOnlyReactiveProperty<int> CurrentTurn => _currentTurn;
        private readonly ReactiveProperty<int> _currentTurn = new();
        
        private void Awake()
        {
            AddEventActions(new()
            {
                { GlobalEventEnum.GameStarted, OnGameStarted },
                { GlobalEventEnum.GameTurn, OnGameTurn },
                { GlobalEventEnum.GameEnded, OnGameEnded },
            });
        }
        
        private void OnGameStarted()
        {
            _currentTurn.Value = 0;
        }
        private void OnGameEnded()
        {
            _currentTurn.Value = 0;
        }

        private void OnGameTurn()
        {
            _currentTurn.Value++;
        }
    }
}