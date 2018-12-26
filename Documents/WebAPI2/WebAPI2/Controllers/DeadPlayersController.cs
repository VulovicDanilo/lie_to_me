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
    public class DeadPlayersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DeadPlayers
        public ActionResult Index()
        {
            return View(db.DeadPlayers.ToList());
        }

        // GET: DeadPlayers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeadPlayer deadPlayer = db.DeadPlayers.Find(id);
            if (deadPlayer == null)
            {
                return HttpNotFound();
            }
            return View(deadPlayer);
        }

        // GET: DeadPlayers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeadPlayers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeadPlayerID,DeathNote,Available")] DeadPlayer deadPlayer)
        {
            if (ModelState.IsValid)
            {
                db.DeadPlayers.Add(deadPlayer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deadPlayer);
        }

        // GET: DeadPlayers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeadPlayer deadPlayer = db.DeadPlayers.Find(id);
            if (deadPlayer == null)
            {
                return HttpNotFound();
            }
            return View(deadPlayer);
        }

        // POST: DeadPlayers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeadPlayerID,DeathNote,Available")] DeadPlayer deadPlayer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deadPlayer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deadPlayer);
        }

        // GET: DeadPlayers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeadPlayer deadPlayer = db.DeadPlayers.Find(id);
            if (deadPlayer == null)
            {
                return HttpNotFound();
            }
            return View(deadPlayer);
        }

        // POST: DeadPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeadPlayer deadPlayer = db.DeadPlayers.Find(id);
            db.DeadPlayers.Remove(deadPlayer);
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
