using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using authen.common.data.Models;

namespace authen.system.web.Controller
{

    partial class sys_userController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            id = "sys_user",
            title = "sys_user",
            url = "/sys_user",
            icon = "fa fa-user",
            icon_image = "",
            translate = "sys_user",
            type = "item",
            controller = "sys_user",
            module = "system",
            is_badge = false,
            is_approval = false,
            is_show_all_user = false,
            is_show_domain_not_init = false,
            type_user = 1,
            list_controller_action_public = new List<string>()
            {
                "sys_user;Authenticate",
                 "sys_user;sign_up",

            },
            list_controller_action_publicNonLogin = new List<string>()
            {
                "sys_user;generate_token",

            },
        };
        //  private bool checkModelStateCreate(sys_user_model item)
        //{
        //    return checkModelStateCreateEdit(ActionEnumForm.create, item);
        //}

        //private bool checkModelStateEdit(sys_user_model item)
        //{
        //    return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        //}
    
}
}
