using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClientsApp.Models;

namespace ClientsApp.Controllers
{
    public class HomeController : Controller
    {

        private AuthDBContext db = new AuthDBContext();

        public ActionResult Index()
        {
            return Redirect("/Home/Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                Auth auth = db.Auth.Find(login.orderNumber);
                if (auth == null)
                {
                    ModelState.AddModelError("", "Invalid login credentials.");
                }else { 

                    if (auth.phoneNumber == login.phoneNumber) {

                        string authId = Guid.NewGuid().ToString();
                        Session["AuthID"] = authId;

                        var cookie = new HttpCookie("AuthID");
                        cookie.Value = authId;
                        Response.Cookies.Add(cookie);
                    
                        if (auth.orderStatusId == 7) { // Checkout - Pending Payment
                            return Redirect("/Checkout/Details/" + auth.ID);
                        } else { 
                            return Redirect("/Orders/Details/" + auth.ID);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login credentials.");
                    }
                }
            }
            return View(login);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logout()
        {

            Session.Abandon();
            Response.Cookies.Clear();

            HttpCookie c = new HttpCookie("AuthID");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);

            Session.Clear();

            return RedirectToAction("Login");
        }
    }
}