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
    public class AmenidadEspaciosController : Controller
    {
        private dbDayOfficeEntities db = new dbDayOfficeEntities();

        // GET: AmenidadEspacios
        public ActionResult Index()
        {
            var user = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            if (user.Rol == 2)
            {
                return RedirectToAction("Create");
            }else
            {
                var tblAmenidadEspacio = db.tblAmenidadEspacio.Include(t => t.tblAmenidades).Include(t => t.tblEspacio);
                return View(tblAmenidadEspacio.ToList());
            }   
        }

        public ActionResult IndexEspacio(int? id)
        {
            var espacio = db.tblEspacio.FirstOrDefault(x => x.IdEspacio == id);
            var amenidad = espacio.tblAmenidadEspacio;

            return View(amenidad.ToList());
        }

        // GET: AmenidadEspacios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAmenidadEspacio tblAmenidadEspacio = db.tblAmenidadEspacio.Find(id);
            if (tblAmenidadEspacio == null)
            {
                return HttpNotFound();
            }
            return View(tblAmenidadEspacio);
        }

        // GET: AmenidadEspacios/Create
        public ActionResult Create()
        {
            var user = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            ViewBag.IdAmenidad = new SelectList(db.tblAmenidades, "IdAmenidad", "TipoAmenidad");
            ViewBag.IdEspacio = new SelectList(db.tblEspacio.Where(x => x.IdUsuario == user.IdUsuario), "IdEspacio", "NombreEspacio");
            return View();
        }

        // POST: AmenidadEspacios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAmenidadEspcio,IdEspacio,IdAmenidad")] tblAmenidadEspacio tblAmenidadEspacio)
        {
            if (ModelState.IsValid)
            {
                db.tblAmenidadEspacio.Add(tblAmenidadEspacio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAmenidad = new SelectList(db.tblAmenidades, "IdAmenidad", "TipoAmenidad", tblAmenidadEspacio.IdAmenidad);
            ViewBag.IdEspacio = new SelectList(db.tblEspacio, "IdEspacio", "NombreEspacio", tblAmenidadEspacio.IdEspacio);
            return View(tblAmenidadEspacio);
        }

        // GET: AmenidadEspacios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAmenidadEspacio tblAmenidadEspacio = db.tblAmenidadEspacio.Find(id);
            if (tblAmenidadEspacio == null)
            {
                return HttpNotFound();
            }
            var user = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            ViewBag.IdAmenidad = new SelectList(db.tblAmenidades, "IdAmenidad", "TipoAmenidad", tblAmenidadEspacio.IdAmenidad);
            ViewBag.IdEspacio = new SelectList(db.tblEspacio.Where(x => x.IdUsuario == user.IdUsuario), "IdEspacio", "NombreEspacio", tblAmenidadEspacio.IdEspacio);
            return View(tblAmenidadEspacio);
        }

        // POST: AmenidadEspacios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAmenidadEspcio,IdEspacio,IdAmenidad")] tblAmenidadEspacio tblAmenidadEspacio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblAmenidadEspacio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAmenidad = new SelectList(db.tblAmenidades, "IdAmenidad", "TipoAmenidad", tblAmenidadEspacio.IdAmenidad);
            ViewBag.IdEspacio = new SelectList(db.tblEspacio, "IdEspacio", "NombreEspacio", tblAmenidadEspacio.IdEspacio);
            return View(tblAmenidadEspacio);
        }

        // GET: AmenidadEspacios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAmenidadEspacio tblAmenidadEspacio = db.tblAmenidadEspacio.Find(id);
            if (tblAmenidadEspacio == null)
            {
                return HttpNotFound();
            }
            return View(tblAmenidadEspacio);
        }

        // POST: AmenidadEspacios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblAmenidadEspacio tblAmenidadEspacio = db.tblAmenidadEspacio.Find(id);
            db.tblAmenidadEspacio.Remove(tblAmenidadEspacio);
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
