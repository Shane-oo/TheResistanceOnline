using JetBrains.Annotations;
namespace TheResistanceOnline.BusinessLogic.Timers
{
    public interface ITimerService
    {
        void Execute(object? stateInfo);

        void PrepareTimer(Action action);

        bool CheckTimerHasStarted();
    }
     [UsedImplicitly]
    public class TimerService: ITimerService
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;
        public DateTime TimerStarted { get; set; }
        public bool IsTimerStarted { get; set; }

        #region Public Methods
        
        public void Execute(object? stateInfo)
        {
            _action();
            if ((DateTime.Now - TimerStarted).TotalSeconds > 60)
            {
                IsTimerStarted = false;
                _timer.Dispose();
            }
        }

        public void PrepareTimer(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 1000, 2000);
            TimerStarted = DateTime.Now;
            IsTimerStarted = true;
        }

        public bool CheckTimerHasStarted()
        {
            return IsTimerStarted;
        }
        #endregion
    }
}
