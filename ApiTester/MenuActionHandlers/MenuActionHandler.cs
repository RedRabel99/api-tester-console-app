using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_tester_console_app.MenuActionHandlers
{
    public abstract class MenuActionHandler
    {
        protected MenuActionService _menuActionService;

        protected MenuActionHandler()
        {
            _menuActionService = new MenuActionService();
            Initialize();
        }
        public abstract Task RunMenu();
        protected abstract void Initialize();
    }
}
