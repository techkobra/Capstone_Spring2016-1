using ReviewsApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace ReviewsApp.Controllers
{
    public class HomeController : Controller
    {
        public  readonly MyDbContext db;
        private  readonly UserManager<ApplicationUser> manager;

        public HomeController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyDbContext()));
        }
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Game()
        {
            // Instantiate the ASP.NET Identity system
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyDbContext()));

            //Get the current logged in User and look up the user in ASP.NET Identity
            
            var currentUser = manager.FindById(User.Identity.GetUserId());

            var scores = db.Scores.Where(x => x.UserId == currentUser.MyUserInfo.Id);//currentUser.Scores;
            
            var rawScores = new List<int>(new int[1000]);
            int i = 0;


            scores.ForEach(x => rawScores[i++] = x.Points);
            //IEnumerable<int> rawScoresOrderedDescending = rawScores.OrderByDescending(x => x);
            //((ObjectQuery)rawScoresOrderedDescending).MergeOption = MergeOption.NoTracking;

            //return View(new int[10]);
            return View(rawScores.OrderByDescending(x => x));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateScore(int newScore)
        {
            // Instantiate the ASP.NET Identity system
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());
            
            //db.Users.Attach(currentUser);

            //currentUser.Scores.Add(new Score {Date = DateTime.Now, Points = newScore, User = currentUser});
            //var score = new Score {Date = DateTime.Now, Points = newScore, User = currentUser};

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = currentUser.UserName, };
                //db.Users.Attach(currentUser);
                
                
                var score = new Score { Date = DateTime.Now, Points = newScore, UserId = currentUser.MyUserInfo.Id};
                
                try
                {

                    //db.Entry(score).State = System.Data.Entity.EntityState.Detached;
                    //db.Entry(score).State = System.Data.Entity.EntityState.Modified;
                   // db.Entry(score).State = System.Data.Entity.EntityState.Added;

                    db.Scores.Add(score);
                    await db.SaveChangesAsync();
                    
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                return Redirect(Request.UrlReferrer.ToString());
                //return RedirectToAction("Game", "Home");
            }


            return null;
        }

        // Only Authenticated users can access thier profile
        [Authorize]
        public ActionResult Profile()
        {
            // Instantiate the ASP.NET Identity system
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyDbContext()));
            
            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId()); 
            
            
            // Recover the profile information about the logged in user
            ViewBag.HomeTown = currentUser.HomeTown;
            ViewBag.FirstName = currentUser.MyUserInfo.FirstName;

            return View();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}