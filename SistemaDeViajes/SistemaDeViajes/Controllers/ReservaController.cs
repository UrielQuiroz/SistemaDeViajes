using SistemaDeViajes.Filters;
using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    [Acceso]
    public class ReservaController : Controller
    {
        BDPasajeEntities db;

        public ReservaController()
        {
            db = new BDPasajeEntities();
        }
        // GET: Reserva
        public ActionResult Index()
        {
            DropDownLugar();

            var pasajesID = ControllerContext.HttpContext.Request.Cookies["pasajesID"];
            var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

            if (pasajesID != null)
            {
                ViewBag.listPasajeId = pasajesID.Value;
                ViewBag.listPasajeCantidad = pasajesCantidad.Value;
            }

            var reserva = (from viaje in db.Viaje
                           join lugarOrigen in db.Lugar on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                           join bus in db.Bus on viaje.IIDBUS equals bus.IIDBUS
                           join lugarDestino in db.Lugar on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                           where viaje.BHABILITADO == 1
                           select new ReservaModel
                           {
                               IDViaje = viaje.IIDVIAJE,
                               nombreFoto = viaje.nombrefoto,
                               Foto = viaje.FOTO,
                               lugarOrigen = lugarOrigen.NOMBRE,
                               lugarDestino = lugarDestino.NOMBRE,
                               precio = (decimal) viaje.PRECIO,
                               fechaViaje = (DateTime) viaje.FECHAVIAJE,
                               placaBus = bus.PLACA,
                               DescripcionBus = bus.DESCRIPCION,
                               totalAsientos = (int) bus.NUMEROCOLUMNAS * (int) bus.NUMEROFILAS,
                               asientosDisponibles = (int) viaje.NUMEROASIENTOSDISPONIBLES,
                               IdBus = bus.IIDBUS
                           }).ToList();
            return View(reserva);
        }

        #region Dropdown

        List<SelectListItem> DropLugar;
        private void DropDownLugar()
        {
            DropLugar = (from o in db.Lugar
                         where o.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = o.NOMBRE,
                             Value = o.IIDLUGAR.ToString()
                         }).ToList();

            DropLugar.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownListLugar = DropLugar;
        }

        #endregion

        public string AgregarCookies(string IdViaje, string cantidad, string fechaViaje, string origen, string destino, string precio, string idBus)
        {
            string rpta = "";

            try
            {
                var pasajesID = ControllerContext.HttpContext.Request.Cookies["pasajesID"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

                if (pasajesID != null && pasajesCantidad != null && pasajesID.Value != "" && pasajesCantidad.Value != "")
                {
                    //Se crea por segunda vez
                    string IdCookie = pasajesID.Value + "{" + IdViaje;
                    string cantidadCookie = pasajesCantidad.Value + "{" + cantidad + "*" + fechaViaje + "*" + origen + "*" + destino + "*" + precio + "*" + idBus;

                    HttpCookie cookieID = new HttpCookie("pasajesID", IdCookie); //Almacenamos todos los id's
                    HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", cantidadCookie); //Almacenamos la cantidad

                    ControllerContext.HttpContext.Response.SetCookie(cookieID);
                    ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);
                }
                else
                {
                    //pasajesCantidad(toda la data, menos el IdViaje)
                    string formatoCadena = cantidad + "*" + fechaViaje + "*" + origen + "*" + destino + "*" + precio + "*" + idBus;

                    HttpCookie cookieID = new HttpCookie("pasajesID", IdViaje); //Almacenamos todos los id's
                    HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", formatoCadena); //Almacenamos la cantidad

                    ControllerContext.HttpContext.Response.SetCookie(cookieID);
                    ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);

                    //se crea por primera vez
                }

                rpta = "OK";
            }
            catch (Exception ex)
            {
                rpta = "";
            }

            return rpta;
        }



        public string QuitarCookie(string id)
        {
            string rpta = "";

            try
            {
                var pasajesID = ControllerContext.HttpContext.Request.Cookies["pasajesID"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

                string valorID = pasajesID.Value;
                string valorCantidad = pasajesCantidad.Value;

                string[] arrayId = valorID.Split('{');
                int indiceID = Array.IndexOf(arrayId, id);

                string nuevoID;
                if (valorID.Contains("{" + id))
                {
                    nuevoID = valorID.Replace("{" + id, "");
                }
                else if (valorID.Contains(id + "{"))
                {
                    nuevoID = valorID.Replace(id + "{", "");
                }
                else
                {
                    nuevoID = valorID.Replace(id, "");
                }

                //Texto
                List<string> valor = valorCantidad.Split('{').ToList();
                valor.RemoveAt(indiceID);
                string[] arrayCantidad = valor.ToArray();
                string nuevaCantidad = String.Join("{", arrayCantidad);

                HttpCookie cookieID = new HttpCookie("pasajesID", nuevoID); //Almacenamos todos los id's
                HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", nuevaCantidad); //Almacenamos la cantidad

                ControllerContext.HttpContext.Response.SetCookie(cookieID);
                ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);

                rpta = "OK";
            }
            catch (Exception ex)
            {
                rpta = "";
            }

            return rpta;
        }

    }
}