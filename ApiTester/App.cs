using api_tester_console_app.MenuActionHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_tester_console_app;

class App(MainMenuActionHandler mainMenuActions)
{
    private MainMenuActionHandler _mainMenuActions = mainMenuActions;

    public async Task RunApp()
    {
        Console.WriteLine("Welcome to the API tester");
        while (true)
        {
            await _mainMenuActions.RunMenu();
        }
    }
}
