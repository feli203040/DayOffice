using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using System.Security.Principal;

namespace DayOfficev2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            var currentCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            currentCulture.NumberFormat.NumberDecimalSeparator = ".";
            currentCulture.NumberFormat.NumberGroupSeparator = " ";
            currentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture("es-ES");
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo("es-ES");

            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Application_PostAuthenticateRequest();
        }

        protected void Application_PostAuthenticateRequest()
        {
            var context = HttpContext.Current;
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
            DayOfficev2.Models.dbDayOfficeEntities db = new Models.dbDayOfficeEntities();
            var roleId = db.tblUsuario.Where(x => x.CorreoElectronico == userName).FirstOrDefault().Rol;
            roles[0] = db.tblRoles.Where(y => y.IdRol == roleId).FirstOrDefault().NombreRol;

            return roles;
        }
        #endregion
    }
}
