using CoffeeFoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CoffeeFoodOrder.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var cart = Session["cart"] as List<CartItem> ?? new List<CartItem>();
            return View(cart);
        }

        public ActionResult Add(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            var cart = Session["cart"] as List<CartItem> ?? new List<CartItem>();
            var item = cart.FirstOrDefault(x => x.Product.Id == id);
            if (item != null)
                item.Quantity++;
            else
                cart.Add(new CartItem { Product = product, Quantity = 1 });

            Session["cart"] = cart;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Remove(int id)
        {
            var cart = Session["cart"] as List<CartItem>;
            if (cart != null)
            {
                var item = cart.FirstOrDefault(x => x.Product.Id == id);
                if (item != null)
                    cart.Remove(item);

                Session["cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Clear()
        {
            Session["cart"] = new List<CartItem>();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Checkout()
        {
            var cart = Session["cart"] as List<CartItem> ?? new List<CartItem>();
            if (!cart.Any())
                return RedirectToAction("Index");

            // Тут більше нічого не зберігаємо в базу
            Session["cart"] = new List<CartItem>();
            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View(); // просто показує подяку
        }
    }
}
