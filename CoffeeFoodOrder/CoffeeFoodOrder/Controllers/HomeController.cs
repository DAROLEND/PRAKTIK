using CoffeeFoodOrder.Models;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CoffeeFoodOrder.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? categoryId, string search, string sort, int page = 1, int pageSize = 9)
        {
            var products = db.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId);

            if (!string.IsNullOrWhiteSpace(search))
                products = products.Where(p => p.Name.Contains(search));

            // Сортування
            ViewBag.Sort = sort;
            switch (sort)
            {
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name", categoryId);
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Search = search;

            return View(products.ToPagedList(page, pageSize));
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
    }
}
