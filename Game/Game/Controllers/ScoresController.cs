using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ReviewsApp.Models;

namespace ReviewsApp.Controllers
{
    public class ScoresController : Controller
    {
        private MyDbContext db;
        private UserManager<ApplicationUser> manager;
        public ScoresController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: Scores
        public async Task<ActionResult> Index()
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());

            return View(db.Scores.Where(x => x.User.UserName == currentUser.UserName));

            //return View(db.Scores.ToList());
            
        }

        // GET: Scores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // GET: Scores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Points,Date")] Score score)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            //if (ModelState.IsValid)
            //{
            //    score.User = currentUser;
            //    db.Scores.Add(score);
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}

            var user = new ApplicationUser { UserName = currentUser.UserName, };
            //db.Users.Attach(currentUser);


            var newScore = new Score { Date = DateTime.Now, Points = score.Points, UserId = currentUser.MyUserInfo.Id };

            try
            {

                //db.Entry(score).State = System.Data.Entity.EntityState.Detached;
                //db.Entry(score).State = System.Data.Entity.EntityState.Modified;
                // db.Entry(score).State = System.Data.Entity.EntityState.Added;

                db.Scores.Add(newScore);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        // GET: Scores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // POST: Scores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Points,Date")] Score score)
        {
            if (ModelState.IsValid)
            {
                db.Entry(score).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(score);
        }

        // GET: Scores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // POST: Scores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Score score = db.Scores.Find(id);
            db.Scores.Remove(score);
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
    }
}
