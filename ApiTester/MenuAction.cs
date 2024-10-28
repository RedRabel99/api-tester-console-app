namespace api_tester_console_app;

public class MenuAction(int id, string name, Menu menuType, Delegate actionToPerform)
{
    public int Id { get; } = id;
    public string Name { get;} = name;
    public Menu MenuType { get;} = menuType;

    public Delegate ActionToPerform { get; } = actionToPerform;
}