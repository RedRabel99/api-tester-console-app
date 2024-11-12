using api_tester_console_app.MenuActionHandlers;

namespace api_tester_console_app;

public static class MenuManager
{
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

    public static bool GetConfirmation(string question)
    {
        Console.WriteLine(question);
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
}
