using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineSinavPortali.Filters
{
    public class AkademisyenFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextWrapper hcw = new HttpContextWrapper(HttpContext.Current);
            
            if(filterContext.HttpContext.Session["akademisyenAd"]==null ||
                String.IsNullOrEmpty(filterContext.HttpContext.Session["akademisyenAd"].ToString()))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Home" }, { "action", "AkademisyenGiris" } } );
            }
        }
    }

}