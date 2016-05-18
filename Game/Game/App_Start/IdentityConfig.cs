using ReviewsApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ReviewsApp
{
    // This is useful if you do not want to tear down the database each time you run the application.
    // You want to create a new database if the Model changes
    public class MyDbInitializer : DropCreateDatabaseIfModelChanges<MyDbContext>
    //public class MyDbInitializer : DropCreateDatabaseAlways<MyDbContext>
    {
        protected override void Seed(MyDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);

            InitializeTestData(context);
        }

        public void InitializeTestData(MyDbContext context)
        {
            //var products = new List<Product>
            //{
            //    new Product { Name = "Product 1", Type = Product.ProductType.Game, Rating = "PG" },
            //    new Product { Name = "Product 2", Type = Product.ProductType.Game, Rating = "PG" }

            //};

            //products.ForEach(product => context.Products.Add(product));
            //context.SaveChanges();
        }

        private void InitializeIdentityForEF(MyDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var myinfo = new MyUserInfo() { FirstName = "ReviewsApp", LastName = "Admin" };
            string name = "Admin";
            string password = "123456";
            string test = "test";

            //Create Role Test and User Test
            RoleManager.Create(new IdentityRole(test));
            UserManager.Create(new ApplicationUser() { UserName = test });

            //Create Role Admin if it does not exist
            if (!RoleManager.RoleExists(name))
            {
                var roleresult = RoleManager.Create(new IdentityRole(name));
            }

            //Create User=Admin with password=123456
            var user = new ApplicationUser();
            user.UserName = name;
            user.HomeTown = "Salem";
            user.MyUserInfo = myinfo;
            var adminresult = UserManager.Create(user, password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, name);
            }
        }
    }
}