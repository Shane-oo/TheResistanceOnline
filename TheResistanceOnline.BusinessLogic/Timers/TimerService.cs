using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Timers
{
    public interface ITimerService
    {
        bool CheckTimerHasStarted();

        void Execute([NotNull] object stateInfo);

        void PrepareTimer(Action action);
    }

    [UsedImplicitly]
    public class TimerService: ITimerService
    {
        #region Fields

        private Action _action;
        private AutoResetEvent _autoResetEvent;
        private Timer _timer;

        #endregion

        #region Properties

        public bool IsTimerStarted { get; set; }

        public DateTime TimerStarted { get; set; }

        #endregion

        #region Public Methods

        public bool CheckTimerHasStarted()
        {
            return IsTimerStarted;
        }

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

        #endregion
    }
}
