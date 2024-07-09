﻿using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api_tester_console_app;

class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var services = serviceCollection.BuildServiceProvider();
       
       var appController = services.GetRequiredService<AppController>();
       appController.RunApp().Wait();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<MenuActionService>();
        services.AddTransient<RequestService>();
        services.AddTransient<AppController>();
    }
}