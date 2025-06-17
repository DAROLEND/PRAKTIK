using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CoffeeFoodOrder.Models
{
    public class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Категорії
            var categories = new List<Category>
            {
                new Category { Name = "Гарячі напої" },
                new Category { Name = "Фаст-фуд" },
                new Category { Name = "Піца" },
                new Category { Name = "Холодні напої" },
                new Category { Name = "Десерти" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Товари
            context.Products.AddRange(new List<Product>
            {
                new Product { Name = "Капучино", Description = "Кава з молочною пінкою", Price = 45, CategoryId = categories[0].Id, ImageUrl = "/Images/cappuccino.jpg" },
                new Product { Name = "Американо", Description = "Класична чорна кава", Price = 35, CategoryId = categories[0].Id, ImageUrl = "/Images/americano.jpg" },
                new Product { Name = "Лате", Description = "Ніжна кава з молоком", Price = 50, CategoryId = categories[0].Id, ImageUrl = "/Images/latte.jpg" },

                new Product { Name = "Гамбургер", Description = "Булка з котлетою, сиром та салатом", Price = 65, CategoryId = categories[1].Id, ImageUrl = "/Images/burger.jpg" },
                new Product { Name = "Хот-дог", Description = "Класичний хот-дог з гірчицею", Price = 55, CategoryId = categories[1].Id, ImageUrl = "/Images/hotdog.jpg" },
                new Product { Name = "Картопля фрі", Description = "Хрустка картопля", Price = 40, CategoryId = categories[1].Id, ImageUrl = "/Images/fries.jpg" },

                new Product { Name = "Піцца Маргарита", Description = "З томатами та сиром моцарела", Price = 110, CategoryId = categories[2].Id, ImageUrl = "/Images/pizza.jpg" },
                new Product { Name = "Піцца Пепероні", Description = "З пікантною ковбаскою", Price = 130, CategoryId = categories[2].Id, ImageUrl = "/Images/pepperoni.jpg" },
                new Product { Name = "Піцца Гавайська", Description = "З куркою та ананасами", Price = 170, CategoryId = categories[2].Id, ImageUrl = "/Images/hawaiian.jpg" },

                new Product { Name = "Кола 0.5л", Description = "Охолоджений напій", Price = 25, CategoryId = categories[3].Id, ImageUrl = "/Images/coke.jpg" },
                new Product { Name = "Фанта 0.5л", Description = "Апельсиновий газований напій", Price = 25, CategoryId = categories[3].Id, ImageUrl = "/Images/fanta.jpg" },
                new Product { Name = "Сік яблучний", Description = "Натуральний сік", Price = 30, CategoryId = categories[3].Id, ImageUrl = "/Images/applejuice.jpg" },

                new Product { Name = "Тірамісу", Description = "Італійський десерт", Price = 60, CategoryId = categories[4].Id, ImageUrl = "/Images/tiramisu.jpg" },
                new Product { Name = "Чізкейк", Description = "Класичний десерт з вершковим сиром", Price = 70, CategoryId = categories[4].Id, ImageUrl = "/Images/cheesecake.jpg" },
                new Product { Name = "Мафін шоколадний", Description = "М'який мафін з шоколадом", Price = 45, CategoryId = categories[4].Id, ImageUrl = "/Images/muffin.jpg" }
            });
            context.SaveChanges();

            // Менеджери
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Роль Admin
            const string roleName = "Admin";
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }

            if (userManager.FindByName("admin") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    FullName = "Адміністратор",
                    PhoneNumber = "380123456789"
                };

                var result = userManager.Create(admin, "Admin@123");

                if (result.Succeeded)
                {
                    userManager.AddToRole(admin.Id, roleName);
                }
            }
        }
    }
}
