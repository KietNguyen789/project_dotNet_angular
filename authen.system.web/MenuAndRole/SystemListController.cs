using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using authen.common.data.Models;
using authen.system.web.Controller;
namespace authen.system.web.MenuAndRole
{
    public static class SystemListController
    {
            public static List<ControllerAppModel> list_controller = new List<ControllerAppModel>()
            {
                sys_userController.declare,
            };
    }
}
