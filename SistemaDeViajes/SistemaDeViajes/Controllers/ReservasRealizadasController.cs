using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    public class ReservasRealizadasController : Controller
    {
        BDPasajeEntities db;

        public ReservasRealizadasController()
        {
            db = new BDPasajeEntities();
        }

        // GET: ReservasRealizadas
        public ActionResult Index()
        {
            //Obtenemos el ID del usuario logueado
            Usuario oUsuario = (Usuario)Session["Usuario"];
            int idUsuario = oUsuario.IIDUSUARIO;

            List<ReservasRealizadasModel> listModel = new List<ReservasRealizadasModel>();

            listModel = (from venta in db.VENTA
                         where venta.BHABILITADO == 1 && venta.IIDUSUARIO == idUsuario
                         select new ReservasRealizadasModel
                         {
                             IDVenta = venta.IIDVENTA,
                             FechaVenta = (DateTime)venta.FECHAVENTA,
                             Total = (decimal)venta.TOTAL
                         }).ToList();

            return View(listModel);
        }

        public JsonResult listarDetalleVenta(int idVenta)
        {
            List<DetalleVentaModel> listModel;
            listModel = (from detalle in db.DETALLEVENTA
                         join bus in db.Bus on detalle.IIDBUS equals bus.IIDBUS
                         join viaje in db.Viaje on detalle.IIDVIAJE equals viaje.IIDVIAJE
                         join origen in db.Lugar on viaje.IIDLUGARORIGEN equals origen.IIDLUGAR
                         join destino in db.Lugar on viaje.IIDLUGARDESTINO equals destino.IIDLUGAR
                         where detalle.BHABILITADO == 1 && detalle.IIDVENTA == idVenta
                         select new DetalleVentaModel
                         {
                             IdDetalleVenta = detalle.IIDETALLEVENTA,
                             IdViaje = (int)detalle.IIDVIAJE,
                             Origen = origen.NOMBRE,
                             Destino = destino.NOMBRE,
                             Placa = bus.PLACA,
                             Precio = (decimal)detalle.PRECIO,
                             Cantidad = (int)detalle.CANTIDAD
                         }).ToList();

            return Json(listModel, JsonRequestBehavior.AllowGet);
        }
    }
}