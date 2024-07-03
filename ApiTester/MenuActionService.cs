namespace api_tester_console_app;

public class MenuActionService
{
    private List<MenuAction> _menuActions;

    public MenuActionService()
    {
        _menuActions = new List<MenuAction>();
    }

    public void AddNewAction(int id, string name, string menuName)
    {
        _menuActions.Add(new MenuAction(id, name, menuName));
    }

    public List<MenuAction> GetMenuActionsByMenuName(string menuName)
    {
        var result = new List<MenuAction>();
        foreach (var menuAction in _menuActions)
        {
            if (menuAction.MenuName == menuName) result.Add(menuAction);
        }

        return result;
    }
}