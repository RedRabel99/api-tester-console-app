namespace api_tester_console_app;

public class MenuAction(int id, string name, string menuName)
{
    public int Id { get; } = id;
    public string Name { get;} = name;
    public string MenuName { get;} = menuName;
}