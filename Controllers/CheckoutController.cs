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
using Square;
using Square.Models;
using Square.Apis;
using Square.Exceptions;
using System.Web.Configuration;
using System.Web.WebPages;

namespace ClientsApp.Controllers
{
    [AuthController]
    public class CheckoutController : Controller
    {
        private OrderDBContext db = new OrderDBContext();
        
        // GET: Checkout/Details/MD5 Hash ID
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Home/Login");
            }

            Models.Order order = db.Orders.Where(i => i.ID == id).First();
            if (order == null)
            {
                return HttpNotFound();
            }


            if (order.billing.paid == "1") {
                return RedirectToAction("Orders/Details/" + order.ID);
            }

            var viewModel = new Models.Order();
            viewModel = order;

            return View(viewModel);
        }

        // GET: Checkout/Payment/checkoutId
        public ActionResult Payment(string id, string checkoutId)
        {
            if (id == null)
            {
                return RedirectToAction("Home/Login");
            }

            Models.Order order = db.Orders.Where(i => i.ID == id).First();
            if (order == null)
            {
                return HttpNotFound();
            }

            if (!(db.CheckPayment(order.orderId, checkoutId)))
            {
                return HttpNotFound();
            }

            var viewModel = new Models.Order();
            viewModel = order;

            return View(viewModel);
        }


        // POST: Checkout/Venmo/5
        [HttpPost]
        public ActionResult Venmo(string id)
        {

            if (id == null)
            {
                return RedirectToAction("/Home/Login");
            }

            Models.Order order = db.Orders.Where(i => i.ID == id).First();
            if (order == null)
            {
                return HttpNotFound();
            }

            Models.OrderBilling billing = db.Billing.FirstOrDefault(i => i.orderId == order.orderId);
            if (billing == null)
            {
                return HttpNotFound();
            }

            try { 

                // Save checkout on the base
                db.CreatePayment(order.orderId, "", "VENMO");
                return Redirect(WebConfigurationManager.AppSettings["VENMO_RedirectUrl"]);

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", new { error = e.Message });
            }

        }

        // POST: Payment/Details/5
        [HttpPost]
        public ActionResult SquarePayment(string id)
        {

            if (id == null)
            {
                return RedirectToAction("/Home/Login");
            }

            Models.Order order = db.Orders.Where(i => i.ID == id).First();
            if (order == null)
            {
                return HttpNotFound();
            }

            Models.OrderBilling billing = db.Billing.FirstOrDefault(i => i.orderId == order.orderId);
            if (billing == null)
            {
                return HttpNotFound();
            }

            CheckoutSquare square = new CheckoutSquare();
            ICheckoutApi checkoutApi = square.client.CheckoutApi;
            try
            {
                // create line items for the order
                // This example assumes the order information is retrieved and hard coded
                // You can find different ways to retrieve order information and fill in the following lineItems object.
                List<OrderLineItem> lineItems = new List<OrderLineItem>();

                Money firstLineItemBasePriceMoney = new Money.Builder()
                  .Amount(order.billing.totalChargesSquareAPI)
                  .Currency("USD")
                  .Build();
                
                OrderLineItem firstLineItem = new OrderLineItem.Builder("1")
                  .Name(order.service)
                  .BasePriceMoney(firstLineItemBasePriceMoney)
                  .Build();

                lineItems.Add(firstLineItem);

                // create Order object with line items
                Square.Models.Order squareOrder = new Square.Models.Order.Builder(square.locationId)
                  .LineItems(lineItems)
                  .Build();

                // create order request with order
                CreateOrderRequest orderRequest = new CreateOrderRequest.Builder()
                  .Order(squareOrder)
                  .IdempotencyKey(order.contractNumber)
                  .Build();

                // create checkout request with the previously created order
                CreateCheckoutRequest createCheckoutRequest = new CreateCheckoutRequest.Builder(
                    Guid.NewGuid().ToString(),
                    orderRequest)
                  .RedirectUrl(WebConfigurationManager.AppSettings["SQUARE_RedirectUrl"] + id)
                  .MerchantSupportEmail("info@truckeasymoving.com")
                  .PrePopulateBuyerEmail(order.email)
                  .Build();

                // create checkout response, and redirect to checkout page if successful
                CreateCheckoutResponse response = checkoutApi.CreateCheckout(square.locationId, createCheckoutRequest);

                // Save checkout on the base
                db.CreatePayment(order.orderId, response.Checkout.Id, "SQUARE API");

                return Redirect(response.Checkout.CheckoutPageUrl);
            }
            catch (ApiException e)
            {
                return RedirectToAction("Error", new { error = e.Message });
            }

        }

    }
}
