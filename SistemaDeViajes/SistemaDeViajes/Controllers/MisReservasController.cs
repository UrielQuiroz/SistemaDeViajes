using SistemaDeViajes.Filters;
using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    [Acceso]
    public class MisReservasController : Controller
    {
        BDPasajeEntities db;

        public MisReservasController()
        {
            db = new BDPasajeEntities();
        }

        // GET: MisReservas
        public ActionResult Index()
        {
            List<ReservaModel> listReservaModel = new List<ReservaModel>();
            var pasajesID = ControllerContext.HttpContext.Request.Cookies["pasajesID"];
            var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

            if (pasajesCantidad != null)
            {
                string valorID = pasajesID.Value;
                string valorCantidad = pasajesCantidad.Value;

                string[] arrayElementos = valorCantidad.Split('{');
                string[] arrayId = valorID.Split('{');
                string[] reserva;

                if (arrayId[0] != "")
                {
                    for (int i = 0; i < arrayElementos.Count(); i++)
                    {
                        ReservaModel model = new ReservaModel();
                        reserva = arrayElementos[i].Split('*');

                        model.IDViaje = int.Parse(arrayId[i]);
                        model.cantidad = int.Parse(reserva[0]);
                        model.fechaViaje = DateTime.Parse(reserva[1]);
                        model.lugarOrigen = reserva[2];
                        model.lugarDestino = reserva[3];
                        model.precio = decimal.Parse(reserva[4]);
                        listReservaModel.Add(model);
                    }
                }
            }



            return View(listReservaModel);
        }



        //////////////////////
        public ActionResult Filtro()
        {
            List<ReservaModel> listReservaModel = new List<ReservaModel>();
            var pasajesID = ControllerContext.HttpContext.Request.Cookies["pasajesID"];
            var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

            if (pasajesCantidad != null)
            {
                string valorID = pasajesID.Value;
                string valorCantidad = pasajesCantidad.Value;

                string[] arrayElementos = valorCantidad.Split('{');
                string[] arrayId = valorID.Split('{');
                string[] reserva;

                if (arrayId[0] != "")
                {
                    for (int i = 0; i < arrayElementos.Count(); i++)
                    {
                        ReservaModel model = new ReservaModel();
                        reserva = arrayElementos[i].Split('*');

                        model.IDViaje = int.Parse(arrayId[i]);
                        model.cantidad = int.Parse(reserva[0]);
                        model.fechaViaje = DateTime.Parse(reserva[1]);
                        model.lugarOrigen = reserva[2];
                        model.lugarDestino = reserva[3];
                        model.precio = decimal.Parse(reserva[4]);
                        listReservaModel.Add(model);
                    }
                }
            }



            return PartialView("_tablaMisReservas", listReservaModel);
        }



        public string GuardarDatos(string total)
        {
            string rpta = "";

            try
            {
                var pasajesID = ControllerContext.HttpContext.Request.Cookies["pasajesID"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

                if (pasajesCantidad != null)
                {
                    string[] arrayElementos = pasajesCantidad.Value.Split('{');
                    string[] arrayId = pasajesID.Value.Split('{');
                    string[] reserva;

                    //using (TransactionScope scope = new System.Transactions.TransactionScope())
                    //{
                        VENTA oVenta = new VENTA();
                        Usuario oUsuario = (Usuario)Session["Usuario"];
                        oVenta.TOTAL = decimal.Parse(total);
                        oVenta.FECHAVENTA = DateTime.Now;
                        oVenta.IIDUSUARIO = oUsuario.IIDUSUARIO;
                        oVenta.BHABILITADO = 1;
                        db.VENTA.Add(oVenta);
                        db.SaveChanges();

                        int idVenta = oVenta.IIDVENTA;
                        for (int i = 0; i < arrayElementos.Count(); i++)
                        {
                            reserva = arrayElementos[i].Split('*');
                            DETALLEVENTA oDetalleVenta = new DETALLEVENTA();
                            oDetalleVenta.IIDVENTA = idVenta;
                            int idViaje = int.Parse(arrayId[i]);
                            oDetalleVenta.IIDVIAJE = idViaje;
                            oDetalleVenta.IIDBUS = int.Parse(reserva[5]);
                            int cantidad = int.Parse(reserva[0]);
                            oDetalleVenta.CANTIDAD = cantidad;
                            oDetalleVenta.PRECIO = int.Parse(reserva[4]);
                            oDetalleVenta.BHABILITADO = 1;
                            db.DETALLEVENTA.Add(oDetalleVenta);

                            //Disminuimos el stock
                            //Viaje oVentaBusqueda = db.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                            Viaje oVentaBusqueda = (from viaje in db.Viaje
                                                    where viaje.IIDVIAJE == idViaje
                                                    select viaje).First();

                            oVentaBusqueda.NUMEROASIENTOSDISPONIBLES = oVentaBusqueda.NUMEROASIENTOSDISPONIBLES - cantidad;
                        db.SaveChanges();
                        }

                        db.SaveChanges();
                        //scope.Complete();
                        rpta = "OK";

                        HttpCookie cookieID = new HttpCookie("pasajesID", "");
                        HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", "");

                        ControllerContext.HttpContext.Response.SetCookie(cookieID);
                        ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);
                    //}
                }

            }
            catch (Exception ex)
            {
                rpta = "";
            }

            return rpta;
        }
    }
}