namespace api_tester_console_app;

public class AppController
{
    private MenuActionService _menuActionService;
    private RequestService _requestService;

    public AppController(MenuActionService menuActionService, RequestService requestService)
    {
        _menuActionService = menuActionService;
        _requestService = requestService;
        Initialize();
    }

    public async Task RunApp()
    {
        Console.WriteLine("Welcome to the API tester");
        while (true)
        {
            var shouldContinue = await HandleMenu(Menu.Main);

            if (!shouldContinue)
            {
                break;
            }
        }
    }

    private async Task<bool> HandleMenu(Menu menuType)
    {
        var currentMenu = _menuActionService.GetMenuActionsByMenuType(menuType);
        PrintMenu(currentMenu);
        Console.WriteLine();
        var operation = Console.ReadKey(true);
        
        switch (menuType)
        {
            case Menu.Main:
                return await HandleMainMenu(operation);
            case Menu.Request:
                await HandleRequestMenu(operation);
                return true;
            default:
                Console.WriteLine("Invalid");
                return true;
        }
    }

    private async Task<bool> HandleMainMenu(ConsoleKeyInfo operation)
    {
        switch (operation.KeyChar)
        {
            case '1':
                return await HandleMenu(Menu.Request);
            case '2':
                Console.WriteLine("Collections not implemented");
                return true;
            case '3':
                Console.WriteLine("Configuration not implemented");
                return true;
            case '4':
                Console.WriteLine("Closing...");
                return false;
            default:
                Console.WriteLine("Action of given key does not exist");
                return true;
        }
    }

    private async Task<bool> HandleRequestMenu(ConsoleKeyInfo operation)
    {
        switch (operation.KeyChar)
        {
            case '1':
                await _requestService.SendRequestView(HttpMethod.Get);
                return true;
            case '2':
                Console.WriteLine("POST request not implemented");
                return true;
            case '3':
                Console.WriteLine("DELETE request not implemented");
                return true;
            case '4':
                Console.WriteLine("PUT request not implemented");
                return true;
            case '5':
                Console.WriteLine("PATCH request not implemented");
                return true;
            case '6':
                return false;
            default:
                Console.WriteLine("Action for the given key does not exist");
                return true;
        }
    }

    private void PrintMenu(List<MenuAction> menuActions)
    {
        foreach (var menuAction in menuActions)
        {
            Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
        }
    }
    private void Initialize()
    {
        _menuActionService.AddNewAction(1, "Custom request", Menu.Main);
        _menuActionService.AddNewAction(2, "Collections", Menu.Main);
        _menuActionService.AddNewAction(3, "Edit configuration", Menu.Main);
        _menuActionService.AddNewAction(4, "Exit", Menu.Main);
        
        _menuActionService.AddNewAction(1, "GET request", Menu.Request);
        _menuActionService.AddNewAction(2, "POST request", Menu.Request);
        _menuActionService.AddNewAction(3, "DELETE request", Menu.Request);
        _menuActionService.AddNewAction(4, "PUT request", Menu.Request);
        _menuActionService.AddNewAction(5, "PATCH request", Menu.Request);
        _menuActionService.AddNewAction(6, "Back", Menu.Request);
    }
}