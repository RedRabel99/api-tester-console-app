using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_tester_console_app.MenuActionHandlers;

public class MainMenuActionHandler : MenuActionHandler
{
    RequestMenuActionHandler _requestMenuActions;

    public MainMenuActionHandler(RequestMenuActionHandler requestMenuActions) : base()
    {
        _requestMenuActions = requestMenuActions;
    }

    public override async Task RunMenu()
    {
        await MenuManager.HandleMenuByType(Menu.Main, _menuActionService);
    }

    public async Task HandleRequestMenu()
    {
        await _requestMenuActions.RunMenu();
        Console.WriteLine("Back to main menu");
    }

    public void ShowCollections()
    {
        Console.WriteLine("Show collections\nNot implemented");
    }
    public void EditConfiguration()
    {
        Console.WriteLine("Edit configuration\nNot implemented");
    }
    public static void ExitApp()
    {
        Environment.Exit(0);
    }

    protected override void Initialize()
    {
        _menuActionService.AddNewAction(1, "Custom request", Menu.Main, HandleRequestMenu);
        _menuActionService.AddNewAction(2, "Collections", Menu.Main, ShowCollections);
        _menuActionService.AddNewAction(3, "Edit configuration", Menu.Main, EditConfiguration);
        _menuActionService.AddNewAction(4, "Exit", Menu.Main, ExitApp);
    }
}
