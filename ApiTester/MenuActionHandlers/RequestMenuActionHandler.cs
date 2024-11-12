using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_tester_console_app.MenuActionHandlers;

public class RequestMenuActionHandler : MenuActionHandler
{
    private RequestService _requestService;

    public RequestMenuActionHandler(RequestService requestService) : base()
    {
        _requestService = requestService;
    }

    protected override void Initialize()
    {
        _menuActionService.AddNewAction(1, "Quick request", Menu.CustomRequest, CreateQuickRequestView);
        _menuActionService.AddNewAction(2, "Advanced request", Menu.CustomRequest, CreateAdvancedRequestView);
    }

    public async Task CreateQuickRequestView()
    {
        await _requestService.QuickRequestView();
    }

    public async Task CreateAdvancedRequestView()
    {
        await _requestService.AdvancedRequestView();
    }

    public override async Task RunMenu()
    {
        await MenuManager.HandleMenuByType(Menu.CustomRequest, _menuActionService);
    }
}
