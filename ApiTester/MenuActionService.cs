namespace api_tester_console_app;

public class MenuActionService
{
    private readonly List<MenuAction> _menuActions;

    public MenuActionService()
    {
        _menuActions = new List<MenuAction>();
    }

    public void AddNewAction(int id, string name, Menu menuType, Delegate actionToPerform)
    {
        _menuActions.Add(new MenuAction(id, name, menuType, actionToPerform));
    }

    public List<MenuAction> GetMenuActionsByMenuType(Menu menuType)
    {
        var result = new List<MenuAction>();
        foreach (var menuAction in _menuActions)
        {
            if (menuAction.MenuType == menuType) result.Add(menuAction);
        }

        return result;
    }
}