using SistemaDeViajes.Filters;
using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    //[Acceso]
    public class ViajesController : Controller
    {
        BDPasajeEntities db;

        public ViajesController()
        {
            db = new BDPasajeEntities();
        }


        // GET: Viajes
        public ActionResult Index()
        {
            listarDrops();
            List<ViajeModel> model = null;

            model = (from v in db.Viaje
                     join o in db.Lugar on v.IIDLUGARORIGEN equals o.IIDLUGAR
                     join d in db.Lugar on v.IIDLUGARDESTINO equals d.IIDLUGAR
                     join b in db.Bus on v.IIDBUS equals b.IIDBUS
                     where v.BHABILITADO == 1
                     select new ViajeModel
                     {
                         IDViaje = v.IIDVIAJE,
                         NombreBus = b.PLACA,
                         NombreOrigen = o.NOMBRE,
                         NombreDestino = d.NOMBRE
                     }).ToList();

            return View(model);
        }


        #region DropDowns

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

        List<SelectListItem> DropBus;
        private void DropDownBus()
        {
            DropBus = (from p in db.Bus
                       where p.BHABILITADO == 1
                       select new SelectListItem
                       {
                           Text = p.PLACA,
                           Value = p.IIDBUS.ToString()
                       }).ToList();
            DropBus.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownListBus = DropBus;
        }

        public void listarDrops()
        {
            DropDownLugar();
            DropDownBus();
        }

        #endregion


        public ActionResult Add()
        {

            listarDrops();
            return View();
        }
        
        [HttpPost]
        public ActionResult Add(ViajeModel model)
        {
            if (!ModelState.IsValid)
            {
                listarDrops();
                return View(model);
            }
            else
            {
                Viaje oViaje = new Viaje();
                oViaje.IIDLUGARORIGEN = model.IDLugarOrige;
                oViaje.IIDLUGARDESTINO = model.IDLugarDestino;
                oViaje.PRECIO = model.Precio;
                oViaje.FECHAVIAJE = model.FechaViaje;
                oViaje.IIDBUS = model.IDBus;
                oViaje.NUMEROASIENTOSDISPONIBLES = model.NOAsientosDisponibles;
                oViaje.BHABILITADO = 1;
                db.Viaje.Add(oViaje);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ViajeModel model = new ViajeModel();
            listarDrops();
            Viaje oViaje = db.Viaje.Where(p => p.IIDVIAJE == id).First();

            model.IDViaje = oViaje.IIDVIAJE;
            model.IDLugarOrige = (int) oViaje.IIDLUGARORIGEN;
            model.IDLugarDestino = (int) oViaje.IIDLUGARDESTINO;
            model.Precio = (decimal) oViaje.PRECIO;
            model.FechaViaje = (DateTime) oViaje.FECHAVIAJE;
            model.IDBus = (int) oViaje.IIDBUS;
            model.NOAsientosDisponibles = (int) oViaje.NUMEROASIENTOSDISPONIBLES;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ViajeModel model)
        {
            int idViaje = model.IDViaje;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                Viaje oViaje = db.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                oViaje.IIDLUGARORIGEN = model.IDLugarOrige;
                oViaje.IIDLUGARDESTINO = model.IDLugarDestino;
                oViaje.PRECIO = model.Precio;
                oViaje.FECHAVIAJE = model.FechaViaje;
                oViaje.IIDBUS = model.IDBus;
                oViaje.NUMEROASIENTOSDISPONIBLES = model.NOAsientosDisponibles;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public string Save(ViajeModel model, HttpPostedFileBase Foto, int Accion)
        {
            string rpta = "";
            try
            {
                if (!ModelState.IsValid || (Foto == null && Accion == -1))
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();

                    
                    if (Foto == null && Accion == -1)
                    {
                        model.mensaje = "La foto es obligatoria";
                        rpta += "<ul><li>Debe ingresar una foto</li></ul>";
                    }

                    rpta += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        rpta += "<li class='list-group-item'>" + item + "</li>";
                    }
                    rpta += "</ul>";
                }
                else
                {
                    byte[] fotoBD = null;
                    if (Foto != null)
                    {
                        BinaryReader lector = new BinaryReader(Foto.InputStream);
                        fotoBD = lector.ReadBytes((int)Foto.ContentLength);
                    }

                    if (Accion == -1)
                    {
                        Viaje oViaje = new Viaje();
                        oViaje.IIDLUGARORIGEN = model.IDLugarOrige;
                        oViaje.IIDLUGARDESTINO = model.IDLugarDestino;
                        oViaje.PRECIO = model.Precio;
                        oViaje.FECHAVIAJE = model.FechaViaje;
                        oViaje.IIDBUS = model.IDBus;
                        oViaje.NUMEROASIENTOSDISPONIBLES = model.NOAsientosDisponibles;
                        oViaje.FOTO = fotoBD;
                        oViaje.nombrefoto = model.nombreFoto;
                        oViaje.BHABILITADO = 1;
                        db.Viaje.Add(oViaje);
                        rpta = db.SaveChanges().ToString();
                        if (rpta == "0")
                        {
                            rpta = "";
                        } 
                    }
                    else
                    {
                        Viaje oViaje = db.Viaje.Where(p => p.IIDVIAJE == Accion).First();
                        oViaje.IIDLUGARORIGEN = model.IDLugarOrige;
                        oViaje.IIDLUGARDESTINO = model.IDLugarDestino;
                        oViaje.PRECIO = model.Precio;
                        oViaje.FECHAVIAJE = model.FechaViaje;
                        oViaje.IIDBUS = model.IDBus;
                        oViaje.NUMEROASIENTOSDISPONIBLES = model.NOAsientosDisponibles;
                        if (Foto != null)
                        {
                            oViaje.FOTO = fotoBD;
                        }

                        rpta = db.SaveChanges().ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                rpta = "";
            }

            return rpta;
        }


        public ActionResult Filtro(int? lugarDeDestino)
        {
            List<ViajeModel> model = new List<ViajeModel>();
            if (lugarDeDestino == null)
            {
                model = (from v in db.Viaje
                         join o in db.Lugar on v.IIDLUGARORIGEN equals o.IIDLUGAR
                         join d in db.Lugar on v.IIDLUGARDESTINO equals d.IIDLUGAR
                         join b in db.Bus on v.IIDBUS equals b.IIDBUS
                         where v.BHABILITADO == 1
                         select new ViajeModel
                         {
                             IDViaje = v.IIDVIAJE,
                             NombreBus = b.PLACA,
                             NombreOrigen = o.NOMBRE,
                             NombreDestino = d.NOMBRE
                         }).ToList();
            }
            else
            {
                model = (from v in db.Viaje
                         join o in db.Lugar on v.IIDLUGARORIGEN equals o.IIDLUGAR
                         join d in db.Lugar on v.IIDLUGARDESTINO equals d.IIDLUGAR
                         join b in db.Bus on v.IIDBUS equals b.IIDBUS
                         where v.BHABILITADO == 1 && v.IIDLUGARDESTINO == lugarDeDestino
                         select new ViajeModel
                         {
                             IDViaje = v.IIDVIAJE,
                             NombreBus = b.PLACA,
                             NombreOrigen = o.NOMBRE,
                             NombreDestino = d.NOMBRE
                         }).ToList();
            }
            return PartialView("_TablaViaje", model);
        }

        public JsonResult recuperarDatos(int id)
        {
            ViajeModel model = new ViajeModel();
            Viaje oViaje = db.Viaje.Where(p => p.IIDVIAJE == id).First();

            model.IDLugarOrige = (int) oViaje.IIDLUGARORIGEN;
            model.IDLugarDestino= (int) oViaje.IIDLUGARDESTINO;
            model.Precio = (decimal) oViaje.PRECIO;

            model.fechaViajeCadena = oViaje.FECHAVIAJE != null ?
                ((DateTime)oViaje.FECHAVIAJE).ToString("yyyy-MM-ddTHH:mm") : "";

            model.IDBus =  (int) oViaje.IIDBUS;
            model.NOAsientosDisponibles = (int) oViaje.NUMEROASIENTOSDISPONIBLES;
            model.nombreFoto = oViaje.nombrefoto;
            model.extension = Path.GetExtension(oViaje.nombrefoto);
            model.fotoRecuperarCadena = Convert.ToBase64String(oViaje.FOTO);

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public int Delete(int id)
        {
            int rpta = 0;

            try
            {
                Viaje oViaje = db.Viaje.Where(p => p.IIDVIAJE == id).First();
                oViaje.BHABILITADO = 0;
                rpta = db.SaveChanges();
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

    }
}