using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using authen.common.data.Models;
namespace authen.common.BaseClass
{
    public class ListController
    {
        public static List<ControllerAppModel> list { get; set; } = new List<ControllerAppModel>();
        public static List<string> list_not_public { get; set; } = new List<string>();
        public static List<string> list_public { get; set; } = new List<string>();
    }
}
