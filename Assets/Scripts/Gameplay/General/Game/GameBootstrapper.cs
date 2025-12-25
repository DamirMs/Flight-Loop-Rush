using PT.Tools.EventListener;
using UnityEngine;

namespace Gameplay.General.Game
{
    public class GameBootstrapper : MonoBehaviour
    {
        private void Start()
        {
            GlobalEventBus.On(GlobalEventEnum.GameStarted);
        }
    }
}