using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayOfficev2.Models;

namespace DayOfficev2.Controllers
{
    public class ReservasController : Controller
    {
        private dbDayOfficeEntities db = new dbDayOfficeEntities();

        // GET: Reservas
        public ActionResult Index()
        {
            var user = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            if (user.Rol == 3)
            {
                return RedirectToAction("IndexUser");
            }
            else if (user.Rol == 2)
            {
                return RedirectToAction("IndexEspacio");
            }
            else
            {
                return View();
            }
        }

        public ActionResult IndexUser()
        {
            var user = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            var reservas = user.tblReservas;
            return View(reservas.ToList());
        }

        public ActionResult IndexEspacio()
        {
            var user = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == User.Identity.Name);
            var reservas = db.tblReservas.Where(u => u.IdDueno == user.IdUsuario);
            return View(reservas.ToList());
        }

        public ActionResult Delete(int id)
        {
            tblReservas reserva = db.tblReservas.Find(id);
            db.tblReservas.Remove(reserva);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}