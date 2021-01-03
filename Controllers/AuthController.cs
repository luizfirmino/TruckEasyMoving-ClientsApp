using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientsApp.Models;

namespace ClientsApp.Controllers
{
    public class AuthController : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (!(filterContext.HttpContext.Request.Cookies["AuthID"].Value == filterContext.HttpContext.Session["AuthID"].ToString()))
                {
                    filterContext.HttpContext.Response.Redirect("/Home/Login");
                }
            }
            catch
            {
                filterContext.HttpContext.Response.Redirect("/Home/Login");
            }

        }

    }

}