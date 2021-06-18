using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DayOfficev2.Models;

namespace DayOfficev2.Controllers
{
    [Authorize]
    public class AmenidadesController : Controller
    {
        private dbDayOfficeEntities db = new dbDayOfficeEntities();

        // GET: Amenidades
        public ActionResult Index()
        {
            var user = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            if (user.Rol != 1)
            {
                return RedirectToAction("IndexEspacio");
            }
            else
            {
                return View(db.tblAmenidades.ToList());
            }
            
        }

        public ActionResult IndexEspacio()
        {
            return View(db.tblAmenidades.ToList());
        }

        // GET: Amenidades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAmenidades tblAmenidades = db.tblAmenidades.Find(id);
            if (tblAmenidades == null)
            {
                return HttpNotFound();
            }
            return View(tblAmenidades);
        }

        // GET: Amenidades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Amenidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAmenidad,TipoAmenidad")] tblAmenidades tblAmenidades)
        {
            if (ModelState.IsValid)
            {
                db.tblAmenidades.Add(tblAmenidades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblAmenidades);
        }

        // GET: Amenidades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAmenidades tblAmenidades = db.tblAmenidades.Find(id);
            if (tblAmenidades == null)
            {
                return HttpNotFound();
            }
            return View(tblAmenidades);
        }

        // POST: Amenidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAmenidad,TipoAmenidad")] tblAmenidades tblAmenidades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblAmenidades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblAmenidades);
        }

        // GET: Amenidades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAmenidades tblAmenidades = db.tblAmenidades.Find(id);
            if (tblAmenidades == null)
            {
                return HttpNotFound();
            }
            return View(tblAmenidades);
        }

        // POST: Amenidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblAmenidades tblAmenidades = db.tblAmenidades.Find(id);
            db.tblAmenidades.Remove(tblAmenidades);
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
