using api_tester_console_app.MenuActionHandlers;

namespace api_tester_console_app
{
    public class MenuManager
    {
        private MenuActionService _menuActionService;
        private MainMenuActionHandler _mainMenuActions;

        public MenuManager(MenuActionService menuActionService, MainMenuActionHandler mainMenuActions)
        {
            _menuActionService = menuActionService;
            _mainMenuActions = mainMenuActions;
        }

        public async Task RunApp()
        {
            Console.WriteLine("Welcome to the API tester");
            while (true)
            {
                await _mainMenuActions.RunMenu();
            }
        }

        public async static Task HandleMenuByType(Menu menuType, MenuActionService menuActionService)
        {
            var currentMenu = menuActionService.GetMenuActionsByMenuType(menuType);
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
                    var selectedAction = currentMenu[int.Parse(operation.KeyChar.ToString()) - 1];
                    if (selectedAction.ActionToPerform is Func<Task> asyncAction)
                    {
                        await asyncAction();
                    }
                    else if (selectedAction.ActionToPerform is Action syncAction)
                    {
                        syncAction();
                    }
                    return;
                }
            }
        }

        public static void PrintMenu(List<MenuAction> menuActions)
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

        //    private void Initialize()
        //    {
        //        //TODO: Create Initialize method in each menuActions classes to avoid depending on one instance of menuActionService
        //        _menuActionService.AddNewAction(1, "Custom request", Menu.Main, _mainMenuActions.HandleRequestMenu);
        //        _menuActionService.AddNewAction(2, "Collections", Menu.Main, _mainMenuActions.ShowCollections);
        //        _menuActionService.AddNewAction(3, "Edit configuration", Menu.Main, _mainMenuActions.EditConfiguration);
        //        _menuActionService.AddNewAction(4, "Exit", Menu.Main, MainMenuActions.ExitApp);

        //        _menuActionService.AddNewAction(1, "GET request", Menu.MethodType, () => { });
        //        _menuActionService.AddNewAction(2, "POST request", Menu.MethodType, () => { });
        //        _menuActionService.AddNewAction(3, "DELETE request", Menu.MethodType, () => { });
        //        _menuActionService.AddNewAction(4, "PUT request", Menu.MethodType, () => { });
        //        _menuActionService.AddNewAction(5, "PATCH request", Menu.MethodType, () => { });

        //        _menuActionService.AddNewAction(1, "Quick request", Menu.CustomRequest, RequestMenuActions.CreateQuickRequestView);
        //        _menuActionService.AddNewAction(2, "Advanced request", Menu.CustomRequest, RequestMenuActions.CreateAdvancedRequestView);
        //    }
        //}
    }
}
