using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DayOfficev2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DayOfficev2.Controllers
{
    public class HomeController : Controller
    {
        private dbDayOfficeEntities db = new dbDayOfficeEntities();

        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var user = db.tblUsuario.FirstOrDefault(e => e.CorreoElectronico == email
                && e.Contrasena == password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.CorreoElectronico, true);
                    return RedirectToAction("Index", "Usuarios");
                }
                else
                {
                    return RedirectToAction("Index", new { message = "Nombre de usuario o contraseña incorrectos" });
                }
            }
            else
            {
                return RedirectToAction("Index", new { message = "Ingresa tu mail y contraseña" });
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var context = HttpContext;
            if (context.Request.IsAuthenticated)
            {
                string[] roles = LookupRolesForUser(context.User.Identity.Name);
                var newUser = new GenericPrincipal(context.User.Identity, roles);
                context.User = Thread.CurrentPrincipal = newUser;
            }
        }

        #region helper

        private string[] LookupRolesForUser(string userName)
        {
            string[] roles = new string[1];
            var roleId = db.tblUsuario.Where(x => x.CorreoElectronico == userName).FirstOrDefault().Rol;
            roles[0] = db.tblRoles.Where(y => y.IdRol == roleId).FirstOrDefault().NombreRol;

            return roles;
        }
        #endregion
    }
}