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
    [AuthController]
    public class OrdersController : Controller
    {
        private OrderDBContext db = new OrderDBContext();

        // GET: Orders/Details/5        
        public ActionResult Details(String id)
        {

            if (id == null) {
                return RedirectToAction("/Home/Login");
            }
            
            Order order = db.Orders.Where(i => i.ID == id).First();
            if (order == null) {
                return HttpNotFound();
            }

            if (order.orderStatusId == 7)
            { // Checkout - Pending Payment
                return Redirect("/Checkout/Details/" + id);
            }            

            var viewModel = new Order();
            viewModel = order;
            viewModel.resources = db.Resources.Where(i => i.orderId == order.orderId).ToList();
            viewModel.addresses = db.Addresses.Where(i => i.orderId == order.orderId).ToList();
            viewModel.payments = db.Payments.Where(i => i.orderId == order.orderId).ToList();
            viewModel.review = db.Review.Find(order.orderId);
            return View(viewModel);
        }

        // POST: OrderReview/Edit/5
        [HttpPost]
        public ActionResult Details([Bind(Include = "orderId,stars,review")] OrderReview review)
        {

            if (ModelState.IsValid) { 
                if ((db.Review.Find(review.orderId)) == null)
                {
                    db.Review.Add(review);
                    db.SaveChanges();
                } 
                else 
                {
                    using (var context = new OrderDBContext())
                    {
                        context.Entry(review).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }                
                }
            }
            return RedirectToAction("Details");
           
        }


        /*
        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }
        */

        /*
        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,orderStatusID,customerID,orderServiceID,orderSourceID,dateSchedule,timeSchedule,contractNumber,duration,notes")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,orderStatusID,customerID,orderServiceID,orderSourceID,dateSchedule,timeSchedule,contractNumber,duration,notes")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
