using System.Linq.Expressions;

namespace api_tester_console_app;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the API tester app.");
        Console.WriteLine("Enter the action you want to perform");
        var menuActionService = new MenuActionService();
        Initialize(menuActionService);
        PrintMenu(menuActionService.GetMenuActionsByMenuName("Main"));

        var operation = Console.ReadKey();
        Console.WriteLine(); // Prints new line so the pressed key is not  overlapping with next text
        switch (operation.KeyChar)
        {
            case '1':
                Console.WriteLine("Feature not yet implemented");
                break;
            case '2':
                Console.WriteLine("Feature not yet implemented");
                break;
            case '3':
                Console.WriteLine("Feature not yet implemented");
                break;
            case '4':
                Console.WriteLine("Goodbye");
                return;
                break;
            default:
                Console.WriteLine("There is no corresponding action to given key.");
                break;
        }
    }

    private static void Initialize(MenuActionService menuActionService)
    {
        menuActionService.AddNewAction(1, "Custom request", "Main");
        menuActionService.AddNewAction(2, "Collections", "Main");
        menuActionService.AddNewAction(3, "Edit configuration", "Main");
        menuActionService.AddNewAction(4, "Exit", "Main");
    }

    private static void PrintMenu(List<MenuAction> menuActions)
    {
        foreach (var menuAction in menuActions)
        {
            Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
        }
    }
}