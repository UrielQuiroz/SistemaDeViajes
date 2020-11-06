using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Filters
{
    public class Acceso : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //Si session["Usuario"] es nulo, retornara al Login ("Cuando el usuario quiera entrar por la url")

            var usuario = HttpContext.Current.Session["Usuario"];
            List<MenuModel> roles = (List<MenuModel>) HttpContext.Current.Session["Rol"];

            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = filterContext.ActionDescriptor.ActionName;

            int cantidad = roles.Where(p => p.nombreControlador == controller).Count();

            if (usuario == null || cantidad == 0)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}