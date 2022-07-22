using TheResistanceOnline.BusinessLogic.Timers;

namespace TheResistanceOnline.SocketServer.DI
{
    public static class DISetup
    {
        #region Public Methods

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITimerService, TimerService>();
            
        }

        #endregion
    }
}
