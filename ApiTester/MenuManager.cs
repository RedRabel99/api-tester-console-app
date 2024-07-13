namespace api_tester_console_app
{
    public class MenuManager
    {
        private MenuActionService _menuActionService;
        private RequestService _requestService;

        public MenuManager(MenuActionService menuActionService, RequestService requestService)
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
                await MainMenuView();
            }
        }
        
        public static ConsoleKeyInfo HandleMenu(List<MenuAction> currentMenu)
        {
            Console.WriteLine("----------");
            PrintMenu(currentMenu);
            Console.WriteLine("----------");
            var maxValue = currentMenu.Count;
            while (true)
            {
                var operation = Console.ReadKey(true);

                if (char.IsNumber(operation.KeyChar) &&
                    (operation.KeyChar >= '1' && operation.KeyChar <= maxValue.ToString()[0]))
                {
                    return operation;
                }
            }
        }

        private async Task MainMenuView()
        {
            var mainMenu = _menuActionService.GetMenuActionsByMenuType(Menu.Main);
            var operation = HandleMenu(mainMenu);

            switch (operation.KeyChar)
            {
                case '1':
                    await CustomRequestMenuView();
                    break;
                case '2':
                    CollectionsMenuView();
                    break;
                case '3':
                    EditConfigurationMenuView();
                    break;
                case '4':
                    Exit();
                    break;
            }
        }

        private async Task CustomRequestMenuView()
        {
            var menu = _menuActionService.GetMenuActionsByMenuType(Menu.CustomRequest);
            var operation = HandleMenu(menu);

            switch (operation.KeyChar)
            {
                case '1':
                    try
                    {
                        var httpRequestMassage = _requestService.QuickRequestView();
                        await _requestService.SendRequestAsync(httpRequestMassage);
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine("Invalid request parameters");
                    }
                    break;
                case '2':
                    // Implement Advanced request functionality
                    break;
            }
        }

        private void CollectionsMenuView()
        {
            Console.WriteLine("Not implemented");
            // Implement Collections menu functionality
        }

        private void EditConfigurationMenuView()
        {
            Console.WriteLine("Not implemented");
            // Implement Edit configuration menu functionality
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        private static void PrintMenu(List<MenuAction> menuActions)
        {
            foreach (var menuAction in menuActions)
            {
                Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
            }
        }

        public static bool GetConfirmation()
        {
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            while (true)
            {
                var operation = Console.ReadKey(true);
                switch (operation.KeyChar)
                {
                    case '2':
                        return false;
                    case '1':
                        return true;
                }
            }
        }

        private void Initialize()
        {
            _menuActionService.AddNewAction(1, "Custom request", Menu.Main);
            _menuActionService.AddNewAction(2, "Collections", Menu.Main);
            _menuActionService.AddNewAction(3, "Edit configuration", Menu.Main);
            _menuActionService.AddNewAction(4, "Exit", Menu.Main);

            _menuActionService.AddNewAction(1, "GET request", Menu.MethodType);
            _menuActionService.AddNewAction(2, "POST request", Menu.MethodType);
            _menuActionService.AddNewAction(3, "DELETE request", Menu.MethodType);
            _menuActionService.AddNewAction(4, "PUT request", Menu.MethodType);
            _menuActionService.AddNewAction(5, "PATCH request", Menu.MethodType);

            _menuActionService.AddNewAction(1, "Quick request", Menu.CustomRequest);
            _menuActionService.AddNewAction(2, "Advanced request", Menu.CustomRequest);
        }
    }
}
