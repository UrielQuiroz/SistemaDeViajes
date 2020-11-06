using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    public class BusController : Controller
    {
        BDPasajeEntities db;

        public BusController()
        {
            db = new BDPasajeEntities();
        }
     

        // GET: Bus
        public ActionResult Index(BusModel oModel)
        {
            List<BusModel> ListModel = null;

            //Creamos una variable para hacer el fultrado
            List<BusModel> listFilterModel = new List<BusModel>();
            listarDrops();

            ListModel = (from p in db.Bus
                     join m in db.Sucursal on p.IIDSUCURSAL equals m.IIDSUCURSAL
                     join mo in db.Modelo on p.IIDMODELO equals mo.IIDMODELO
                     join tb in db.TipoBus on p.IIDTIPOBUS equals tb.IIDTIPOBUS
                     where p.BHABILITADO == 1
                     select new BusModel
                     {
                         IDBus = p.IIDBUS,
                         Placa = p.PLACA,
                         Modelo = mo.NOMBRE,
                         Sucursal = m.NOMBRE,
                         TipoBus = tb.NOMBRE,
                         IDModelo = (int) p.IIDMODELO,
                         IDSucursal = (int) p.IIDSUCURSAL, 
                         IDTipoBus = (int) p.IIDTIPOBUS
                         
                     }).ToList();

            if (oModel.IDBus == 0 && oModel.Placa == null && oModel.IDModelo == 0 && oModel.IDSucursal == 0 && oModel.IDTipoBus == 0)
            {
                listFilterModel = ListModel;
            }
            else
            {
                //Filtros
                if (oModel.IDBus != 0) //Filtro por Bus
                {
                    ListModel = ListModel.Where(p => p.IDBus.ToString().Contains(oModel.IDBus.ToString())).ToList();
                }

                if (oModel.Placa != null) //Filtro por placa
                {
                    ListModel = ListModel.Where(p => p.Placa.Contains(oModel.Placa)).ToList();
                }

                if (oModel.IDSucursal != 0)//Filtro por sucursal
                {
                    ListModel = ListModel.Where(p => p.IDSucursal.ToString().Contains(oModel.IDSucursal.ToString())).ToList();
                }

                if (oModel.IDTipoBus != 0)
                {
                    ListModel = ListModel.Where(p => p.IDTipoBus.ToString().Contains(oModel.IDTipoBus.ToString())).ToList();
                }

                if (oModel.IDModelo != 0) //Filtro por modelo
                {
                    ListModel = ListModel.Where(p => p.IDModelo.ToString().Contains(oModel.IDModelo.ToString())).ToList();
                }

                listFilterModel = ListModel;
            }

            return View(listFilterModel);
        }

        #region DropDownList

        List<SelectListItem> DropSucursal;

        private void DropDownSucursal()
        {
            DropSucursal = (from s in db.Sucursal
                            where s.BHABILITADO == 1
                            select new SelectListItem
                            {
                                Text = s.NOMBRE,
                                Value = s.IIDSUCURSAL.ToString()
                            }).ToList();

            DropSucursal.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownListSucursal = DropSucursal;
        }

        List<SelectListItem> DropTipoBus;

        private void DropDownTipoBus()
        {
            DropTipoBus = (from tb in db.TipoBus
                           where tb.BHABILITADO == 1
                           select new SelectListItem
                           {
                               Text = tb.NOMBRE,
                               Value = tb.IIDTIPOBUS.ToString()
                           }).ToList();
            DropTipoBus.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownListTipoBus = DropTipoBus;

        }

        List<SelectListItem> DropModelo;
        private void DropDownModelo()
        {
            DropModelo = (from m in db.Modelo
                          where m.BHABILITADO == 1
                          select new SelectListItem
                          {
                              Text = m.NOMBRE,
                              Value = m.IIDMODELO.ToString()
                          }).ToList();
            DropModelo.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownListModelo = DropModelo;

        }

        List<SelectListItem> DropMarca;
        private void DropDownMarca()
        {
            DropMarca = (from m in db.Marca
                         where m.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = m.NOMBRE,
                             Value = m.IIDMARCA.ToString()
                         }).ToList();
            DropMarca.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownListMarca = DropMarca;

        }

        public void listarDrops()
        {
            DropDownSucursal();
            DropDownTipoBus();
            DropDownModelo();
            DropDownMarca();
        }

        #endregion

        public ActionResult Add()
        {
            listarDrops();
            return View();
        }


        [HttpPost]
        public ActionResult Add(BusModel model)
        {
            int regEncontrados = 0;
            string placa = model.Placa;

            regEncontrados = db.Bus.Where(p => p.PLACA == placa).Count();

            if (!ModelState.IsValid || regEncontrados >= 1)
            {
                if (regEncontrados >= 1)
                {
                    model.msjError = "La el bus ya esxiste";
                }
                listarDrops();
                return View(model);
            }
            else
            {
                Bus oBus = new Bus();
                oBus.IIDSUCURSAL = model.IDSucursal;
                oBus.IIDTIPOBUS = model.IDTipoBus;
                oBus.PLACA = model.Placa;
                oBus.FECHACOMPRA = model.fechaCompra;
                oBus.IIDMODELO = model.IDModelo;
                oBus.NUMEROFILAS = model.NumFilas;
                oBus.NUMEROCOLUMNAS = model.numColumnas;
                oBus.DESCRIPCION = model.Descripcion;
                oBus.OBSERVACION = model.Observacion;
                oBus.IIDMARCA = model.IDMarca;
                oBus.BHABILITADO = 1;
                db.Bus.Add(oBus);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            listarDrops();
            BusModel model = new BusModel();
            Bus oBus = db.Bus.Where(p => p.IIDBUS == id).FirstOrDefault();

            model.IDBus = oBus.IIDBUS;
            model.IDSucursal = (int) oBus.IIDSUCURSAL;
            model.IDTipoBus = (int) oBus.IIDTIPOBUS;
            model.Placa = oBus.PLACA;
            model.fechaCompra = (DateTime)oBus.FECHACOMPRA;
            model.IDModelo = (int)oBus.IIDMODELO;
            model.NumFilas = (int) oBus.NUMEROFILAS;
            model.numColumnas = (int)oBus.NUMEROCOLUMNAS;
            model.Observacion = oBus.OBSERVACION;
            model.Descripcion = oBus.DESCRIPCION;
            model.IDMarca = (int)oBus.IIDMARCA;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(BusModel model)
        {
            int regEncontrados = 0;
            string placa = model.Placa;
            int id = model.IDBus;

            regEncontrados = db.Bus.Where(p => p.PLACA == placa && p.IIDBUS != id && p.BHABILITADO == 1).Count();

            if (!ModelState.IsValid || regEncontrados >= 1)
            {
                if (regEncontrados >= 1)
                {
                    model.msjError = "El bus ya existe";
                }
                listarDrops();
                return View(model);
            }
            else
            {
                Bus oBus = db.Bus.Where(p => p.IIDBUS == id).First();
                oBus.IIDSUCURSAL = model.IDSucursal;
                oBus.IIDTIPOBUS = model.IDTipoBus;
                oBus.PLACA = model.Placa;
                oBus.FECHACOMPRA = model.fechaCompra;
                oBus.IIDMODELO = model.IDModelo;
                oBus.NUMEROFILAS = model.NumFilas;
                oBus.NUMEROCOLUMNAS = model.numColumnas;
                oBus.OBSERVACION = model.Observacion;
                oBus.DESCRIPCION = model.Descripcion;
                oBus.IIDMARCA = model.IDMarca;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Bus oBus = db.Bus.Where(p => p.IIDBUS == id).First();
            oBus.BHABILITADO = 0;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}