
namespace PT.Tools.EventListener
{
    public static class GlobalEventBus
    {
        public delegate void EventHandler(GlobalEventEnum eventEnum);
        public static event EventHandler OnEventHandle;

        public static void On(GlobalEventEnum eventEnum)
        {
            DebugManager.Log(DebugCategory.Observer, eventEnum.ToString());

            OnEventHandle?.Invoke(eventEnum);
        }
    }
}
