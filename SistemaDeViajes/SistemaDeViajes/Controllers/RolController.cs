using SistemaDeViajes.Filters;
using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    //[Acceso]
    public class RolController : Controller
    {
        BDPasajeEntities db;

        public RolController()
        {
            db = new BDPasajeEntities();
        }
        // GET: Rol
        public ActionResult Index()
        {
            List<RolModel> listModel = new List<RolModel>();

            listModel = (from p in db.Rol
                         where p.BHABILITADO == 1
                         select new RolModel
                         {
                             IDRol = p.IIDROL,
                             Nombre = p.NOMBRE,
                             Descripcion = p.DESCRIPCION
                         }).ToList();

            return View(listModel);
        }

        public ActionResult Filtro(string NombreRol)
        {
            List<RolModel> listModel = new List<RolModel>();

            if (NombreRol == null)
            {
                listModel = (from p in db.Rol
                             where p.BHABILITADO == 1
                             select new RolModel
                             {
                                 IDRol = p.IIDROL,
                                 Nombre = p.NOMBRE,
                                 Descripcion = p.DESCRIPCION
                             }).ToList();
            }
            else
            {
                listModel = (from p in db.Rol
                             where p.BHABILITADO == 1 && p.NOMBRE.Contains(NombreRol)
                             select new RolModel
                             {
                                 IDRol = p.IIDROL,
                                 Nombre = p.NOMBRE,
                                 Descripcion = p.DESCRIPCION
                             }).ToList();
            }

            return PartialView("_TablaRol", listModel);
        }

        public string Save(RolModel model, int accion)
        {
            string rpta = "";

            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();

                rpta += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    rpta += "<li class='list-group-item'>" + item + "</li>";
                }
                rpta += "</ul>";
            }
            else
            {
                int cantidad = 0;
                if (accion == -1)
                {
                    cantidad = db.Rol.Where(p => p.NOMBRE == model.Nombre && p.BHABILITADO == 1).Count();

                    //Validamos si ya existe el nombre en la BD
                    if (cantidad >= 1)
                    {
                        rpta = "-1";
                    }
                    else
                    {
                        Rol oRol = new Rol();
                        oRol.NOMBRE = model.Nombre;
                        oRol.DESCRIPCION = model.Descripcion;
                        oRol.BHABILITADO = 1;
                        db.Rol.Add(oRol);
                        rpta = db.SaveChanges().ToString();
                        if (rpta == "0")
                        {
                            rpta = "";
                        }
                    }
                }
                else
                {
                    cantidad = db.Rol.Where(p => p.NOMBRE == model.Nombre && p.IIDROL != accion && p.BHABILITADO == 1).Count();
                    //Validamos si ya existe el registro en la BD
                    if (cantidad >= 1)
                    {
                        rpta = "-1";
                    }
                    else
                    {
                        Rol oRol = db.Rol.Where(p => p.IIDROL == accion).First();
                        oRol.NOMBRE = model.Nombre;
                        oRol.DESCRIPCION = model.Descripcion;
                        rpta = db.SaveChanges().ToString();
                    }
                }
            }

            return rpta;
        }

        public string Delete(RolModel model)
        {
            string rpta = "";

            try
            {
                int id = model.IDRol;

                Rol oRol = db.Rol.Where(p => p.IIDROL == id).First();
                oRol.BHABILITADO = 0;
                rpta = db.SaveChanges().ToString();

            }
            catch (Exception)
            {
                rpta = "";
            }

            return rpta;
        }

        public JsonResult recuperarDatos(int Accion)
        {
            RolModel model = new RolModel();
            model = (from p in db.Rol
                     where p.BHABILITADO == 1 && p.IIDROL == Accion
                     select new RolModel
                     {
                         IDRol = p.IIDROL,
                         Nombre = p.NOMBRE,
                         Descripcion = p.DESCRIPCION
                     }).First();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}

