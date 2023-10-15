using Microsoft.AspNetCore;

namespace TheResistanceOnline.Server;

public class Program
{
    #region Private Methods

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }

    #endregion

    #region Public Methods

    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    #endregion
}
