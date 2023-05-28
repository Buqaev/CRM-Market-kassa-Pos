using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _4YolMarket.Filters
{
    public class LoggedUser: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["LoggedUser"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login");
                return;
            }
            //if (HttpContext.Current.Session["LoggedUser"] == null)
            //{
            //    filterContext.Result = new RedirectResult("~/Login");
            //    return;
            //}

            base.OnActionExecuting(filterContext);
        }
    }
}