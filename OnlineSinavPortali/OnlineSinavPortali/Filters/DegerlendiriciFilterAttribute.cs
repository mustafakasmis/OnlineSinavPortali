using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineSinavPortali.Filters
{
    public class DegerlendiriciFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

            HttpContextWrapper hcw = new HttpContextWrapper(HttpContext.Current);

            if (filterContext.HttpContext.Session["degerlendiriciAd"] == null ||
                String.IsNullOrEmpty(filterContext.HttpContext.Session["degerlendiriciAd"].ToString()))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Home" }, { "action", "DegerlendiriciGiris" } });
            }

        }
    }
}