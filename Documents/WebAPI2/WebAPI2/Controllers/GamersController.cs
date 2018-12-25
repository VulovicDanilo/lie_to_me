using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAPI2.Models;

namespace WebAPI2.Controllers
{
    public class GamersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Gamers
        public ActionResult Index()
        {
            return View(db.Gamers.ToList());
        }

        // GET: Gamers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gamer gamer = db.Gamers.Find(id);
            if (gamer == null)
            {
                return HttpNotFound();
            }
            return View(gamer);
        }

        // GET: Gamers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gamers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GamerID,Email,UserName,Wins,Losses")] Gamer gamer)
        {
            if (ModelState.IsValid)
            {
                db.Gamers.Add(gamer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gamer);
        }

        // GET: Gamers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gamer gamer = db.Gamers.Find(id);
            if (gamer == null)
            {
                return HttpNotFound();
            }
            return View(gamer);
        }

        // POST: Gamers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GamerID,Email,UserName,Wins,Losses")] Gamer gamer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gamer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gamer);
        }

        // GET: Gamers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gamer gamer = db.Gamers.Find(id);
            if (gamer == null)
            {
                return HttpNotFound();
            }
            return View(gamer);
        }

        // POST: Gamers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gamer gamer = db.Gamers.Find(id);
            db.Gamers.Remove(gamer);
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
