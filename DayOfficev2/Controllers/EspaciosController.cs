using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DayOfficev2.Models;

namespace DayOfficev2.Controllers
{
    [Authorize]
    public class EspaciosController : Controller
    {
        private dbDayOfficeEntities db = new dbDayOfficeEntities();
        private List<int> ids = new List<int>();
        // GET: Espacios
        public ActionResult Index()
        {
            var user = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);

            if(user.Rol == 3)
            {
                return RedirectToAction("IndexUser");
            }
            else if(user.Rol == 2)
            {
                return RedirectToAction("IndexEspacio");
            }
            else
            {
                var tblEspacio = db.tblEspacio.Include(t => t.tblUsuario);
                CalcularPuntaje();
                var orderedTable = tblEspacio.OrderBy(e => e.Puntaje);
                return View(orderedTable.ToList());
            }
            
        }

        public ActionResult IndexUser()
        {
            var tblEspacio = db.tblEspacio.Include(t => t.tblUsuario);
            CalcularPuntaje();
            var orderedTable = tblEspacio.OrderBy(e => e.Puntaje);
            return View(orderedTable.ToList());
        }

        public ActionResult IndexEspacio()
        {
            var usuario = db.tblUsuario.FirstOrDefault(x => x.CorreoElectronico == User.Identity.Name);
            var espacios = usuario.tblEspacio;
            return View(espacios.ToList());
        }

        public ActionResult Reservar(int id)
        {
            var usuario = db.tblUsuario.FirstOrDefault(x => x.CorreoElectronico == User.Identity.Name);
            var espacio = db.tblEspacio.Find(id);
            tblReservas reserva = new tblReservas();
            reserva.IdEspacio = id;
            reserva.IdUsuario = usuario.IdUsuario;
            reserva.IdDueno = espacio.IdUsuario;
            db.tblReservas.Add(reserva);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult IndexAmenidad(int? id)
        {
            return RedirectToAction("IndexEspacio","AmenidadEspacios", new { id = id });
        }

        // GET: Espacios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEspacio tblEspacio = db.tblEspacio.Find(id);
            if (tblEspacio == null)
            {
                return HttpNotFound();
            }
            return View(tblEspacio);
        }

        // GET: Espacios/Create
        public ActionResult Create()
        {
            //ViewBag.IdUsuario = new SelectList(db.tblUsuario, "IdUsuario", "NombreUsuario");
            return View();
        }

        // POST: Espacios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblEspacio tblEspacio)
        {
            //if (ModelState.IsValid)
            //{
                var usuario = db.tblUsuario.FirstOrDefault(x => x.CorreoElectronico == User.Identity.Name);
                tblEspacio.IdUsuario = usuario.IdUsuario;
                db.tblEspacio.Add(tblEspacio);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //ViewBag.IdUsuario = new SelectList(db.tblUsuario, "IdUsuario", "NombreUsuario", tblEspacio.IdUsuario);
            //return View(tblEspacio);
        }

        // GET: Espacios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEspacio tblEspacio = db.tblEspacio.Find(id);
            if (tblEspacio == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.tblUsuario, "IdUsuario", "NombreUsuario", tblEspacio.IdUsuario);
            return View(tblEspacio);
        }

        // POST: Espacios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEspacio,IdUsuario,NombreEspacio,LatitudEspacio,LongitudEspacio,Costo,VelocidadInternet")] tblEspacio tblEspacio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblEspacio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.tblUsuario, "IdUsuario", "NombreUsuario", tblEspacio.IdUsuario);
            return View(tblEspacio);
        }

        // GET: Espacios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEspacio tblEspacio = db.tblEspacio.Find(id);
            if (tblEspacio == null)
            {
                return HttpNotFound();
            }
            return View(tblEspacio);
        }

        // POST: Espacios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblEspacio tblEspacio = db.tblEspacio.Find(id);
            db.tblEspacio.Remove(tblEspacio);
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

        private void CalcularPuntaje()
        {
            double userLatitud;
            double userLongitud;
            double espacioLatitud;
            double espacioLongitud;
            double precio;

            var user = db.tblUsuario.FirstOrDefault(e => e.CorreoElectronico == User.Identity.Name);

            userLatitud = Convert.ToDouble(user.LatitudUsuario);
            userLongitud = Convert.ToDouble(user.LongitudUsuario);

            foreach (var espacio in db.tblEspacio)
            {
                espacioLatitud = espacio.LatitudEspacio;
                espacioLongitud = espacio.LongitudEspacio;
                precio = espacio.Costo;

                espacio.Puntaje = Math.Sqrt(Math.Pow(userLatitud - espacioLatitud, 2) + Math.Pow(userLongitud - espacioLongitud, 2));

                db.Entry(espacio).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        public ActionResult Comparar(int id)
        {

            var espacio = db.tblEspacio.Find(id);
            espacio.Selected = 1;
            db.Entry(espacio).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {

            var espacio = db.tblEspacio.Find(id);
            espacio.Selected = 0;
            db.Entry(espacio).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CompararView2");
        }
        public ActionResult CompararView2()
        {

            var espacios = db.tblEspacio.Where(m => m.Selected == 1);
            var oespacios = espacios.OrderByDescending(m => m.VelocidadInternet);
            return View(oespacios.ToList());
        }
    }
}
