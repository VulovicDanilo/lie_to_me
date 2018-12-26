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
    public class TownRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TownRoles
        public ActionResult Index()
        {
            return View(db.TownRoles.ToList());
        }

        // GET: TownRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TownRole townRole = db.TownRoles.Find(id);
            if (townRole == null)
            {
                return HttpNotFound();
            }
            return View(townRole);
        }

        // GET: TownRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TownRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TownRoleID,Name,Alignment,Description,Abilities,Goal")] TownRole townRole)
        {
            if (ModelState.IsValid)
            {
                db.TownRoles.Add(townRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(townRole);
        }

        // GET: TownRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TownRole townRole = db.TownRoles.Find(id);
            if (townRole == null)
            {
                return HttpNotFound();
            }
            return View(townRole);
        }

        // POST: TownRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TownRoleID,Name,Alignment,Description,Abilities,Goal")] TownRole townRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(townRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(townRole);
        }

        // GET: TownRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TownRole townRole = db.TownRoles.Find(id);
            if (townRole == null)
            {
                return HttpNotFound();
            }
            return View(townRole);
        }

        // POST: TownRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TownRole townRole = db.TownRoles.Find(id);
            db.TownRoles.Remove(townRole);
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
