using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace authen.common.BaseClass
{

    [Route("[controller].ctr/[action]")]
    public abstract class BaseAuthenticationController : Controller
    {

        public JsonResult generateError()
        {
            Response.StatusCode = 400;
            var errorList = ModelState
               .Where(x => x.Value != null)
               .ToList().
               Where(d => d.Value.Errors.Count > 0)
               .Select(kvp =>
                   new
                   {
                       key = kvp.Key,
                       value = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                   }
               ).ToList();
            return Json(errorList);
        }
    }
}
