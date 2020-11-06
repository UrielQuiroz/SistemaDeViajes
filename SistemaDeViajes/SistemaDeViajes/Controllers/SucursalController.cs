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
    public class SucursalController : Controller
    {
        BDPasajeEntities db;

        public SucursalController()
        {
            db = new BDPasajeEntities();
        }

        // GET: Sucursal
        public ActionResult Index(SucursalModel oModel)
        {
            string nombre = oModel.Nombre;
            List<SucursalModel> model = null;

            if (nombre == null)
            {
                model = (from p in db.Sucursal
                         where p.BHABILITADO == 1
                         select new SucursalModel
                         {
                             IDSucursal = p.IIDSUCURSAL,
                             Nombre = p.NOMBRE,
                             Direccion = p.DIRECCION,
                             Telefono = p.TELEFONO,
                             Email = p.EMAIL,
                             FechaApertura = (DateTime)p.FECHAAPERTURA
                         }).ToList();
            }
            else
            {
                model = (from p in db.Sucursal
                         where p.BHABILITADO == 1 && p.NOMBRE.Contains(nombre)
                         select new SucursalModel
                         {
                             IDSucursal = p.IIDSUCURSAL,
                             Nombre = p.NOMBRE,
                             Direccion = p.DIRECCION,
                             Telefono = p.TELEFONO,
                             Email = p.EMAIL,
                             FechaApertura = (DateTime)p.FECHAAPERTURA
                         }).ToList();
            }

            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(SucursalModel model)
        {
            int regEncontrados = 0;
            string sucursal = model.Nombre;
            //Validamos si el nombre ya existe
            regEncontrados = db.Sucursal.Where(p => p.NOMBRE == sucursal).Count();

            if (!ModelState.IsValid || regEncontrados >= 1)
            {
                if (regEncontrados >= 1)
                {
                    model.msjError = "El nombre de la sucursal ya existe";
                } 
                return View(model);
            }
            else
            {
                Sucursal oSuc = new Sucursal();
                oSuc.NOMBRE = model.Nombre;
                oSuc.DIRECCION = model.Direccion;
                oSuc.TELEFONO = model.Telefono;
                oSuc.EMAIL = model.Email;
                oSuc.FECHAAPERTURA = model.FechaApertura;
                oSuc.BHABILITADO = 1;
                db.Sucursal.Add(oSuc);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            SucursalModel model = new SucursalModel();
            Sucursal oSuc = db.Sucursal.Where(p => p.IIDSUCURSAL == id).First();

            model.IDSucursal = oSuc.IIDSUCURSAL;
            model.Nombre = oSuc.NOMBRE;
            model.Direccion = oSuc.DIRECCION;
            model.Telefono = oSuc.TELEFONO;
            model.Email = oSuc.EMAIL;
            model.FechaApertura = (DateTime)oSuc.FECHAAPERTURA;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SucursalModel model)
        {
            int regEncontrados = 0;
            string sucursal = model.Nombre;
            int idSuc = model.IDSucursal;

            regEncontrados = db.Sucursal.Where(p => p.NOMBRE == sucursal && p.IIDSUCURSAL != idSuc).Count();

            if (!ModelState.IsValid || regEncontrados >= 1)
            {
                if (regEncontrados >= 1)
                {
                    model.msjError = "El nombre de la sucursal ya existe";
                }
                return View(model);
            }
            else
            {
                Sucursal oSuc = db.Sucursal.Where(p => p.IIDSUCURSAL == idSuc).First();
                oSuc.NOMBRE = model.Nombre;
                oSuc.DIRECCION = model.Direccion;
                oSuc.TELEFONO = model.Telefono;
                oSuc.EMAIL = model.Email;
                oSuc.FECHAAPERTURA = model.FechaApertura;
                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        public ActionResult Delete(int id)
        {
            Sucursal oSuc = db.Sucursal.Where(p => p.IIDSUCURSAL == id).First();
            oSuc.BHABILITADO = 0;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}