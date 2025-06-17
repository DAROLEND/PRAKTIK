using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CoffeeFoodOrder.Models;

namespace CoffeeFoodOrder.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Checkout()
        {
            var cart = Session["cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0) return RedirectToAction("Index", "Home");

            ViewBag.Total = cart.Sum(x => x.Product.Price * x.Quantity);
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(string name, string phone)
        {
            Session["cart"] = null;
            ViewBag.Name = name;
            ViewBag.Phone = phone;
            return View("Thanks");
        }
    }
}
