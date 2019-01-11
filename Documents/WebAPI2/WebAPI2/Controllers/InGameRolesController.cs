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
    public class InGameRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InGameRoles
        public ActionResult Index()
        {
            return View(db.InGameRoles.ToList());
        }

        // GET: InGameRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InGameRole inGameRole = db.InGameRoles.Find(id);
            if (inGameRole == null)
            {
                return HttpNotFound();
            }
            return View(inGameRole);
        }

        // GET: InGameRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InGameRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InGameRoleID,Offence,Defence")] InGameRole inGameRole)
        {
            if (ModelState.IsValid)
            {
                db.InGameRoles.Add(inGameRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inGameRole);
        }

        // GET: InGameRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InGameRole inGameRole = db.InGameRoles.Find(id);
            if (inGameRole == null)
            {
                return HttpNotFound();
            }
            return View(inGameRole);
        }

        // POST: InGameRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InGameRoleID,Offence,Defence")] InGameRole inGameRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inGameRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inGameRole);
        }

        // GET: InGameRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InGameRole inGameRole = db.InGameRoles.Find(id);
            if (inGameRole == null)
            {
                return HttpNotFound();
            }
            return View(inGameRole);
        }

        // POST: InGameRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InGameRole inGameRole = db.InGameRoles.Find(id);
            db.InGameRoles.Remove(inGameRole);
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
