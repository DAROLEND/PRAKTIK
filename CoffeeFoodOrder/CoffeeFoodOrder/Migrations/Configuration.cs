using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace CoffeeFoodOrder.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CoffeeFoodOrder.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CoffeeFoodOrder.Models.ApplicationDbContext context)
        {
        }
    }
}
