using System.Security.Authentication.ExtendedProtection;
using api_tester_console_app.MenuActionHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api_tester_console_app;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var services = serviceCollection.BuildServiceProvider();
       
       var app = services.GetRequiredService<App>();
       await app.RunApp();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<MenuActionService>();
        services.AddTransient<RequestService>();
        services.AddTransient<MainMenuActionHandler>();
        services.AddTransient<RequestMenuActionHandler>();
        services.AddTransient<App>();
    }
}