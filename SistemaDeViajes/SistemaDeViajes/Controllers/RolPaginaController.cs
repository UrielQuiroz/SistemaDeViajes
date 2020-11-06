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
    public class RolPaginaController : Controller
    {
        BDPasajeEntities db;

        public RolPaginaController()
        {
            db = new BDPasajeEntities();
        }

        // GET: RolPagina
        public ActionResult Index()
        {
            DropDownRol();
            DropDownPagina();
            List<RolPaginaModel> listModel = null;

            listModel = (from rp in db.RolPagina
                         join p in db.Pagina on rp.IIDPAGINA equals p.IIDPAGINA
                         join r in db.Rol on rp.IIDROL equals r.IIDROL
                         where rp.BHABILITADO == 1
                         select new RolPaginaModel
                         {
                             IDRolPagina = rp.IIDROLPAGINA,
                             nombrePagina = p.MENSAJE,
                             nombreRol = r.NOMBRE
                         }).ToList();

            return View(listModel);
        }

        #region DropDowns

        List<SelectListItem> DropRol;

        private void DropDownRol()
        {
            DropRol = (from p in db.Rol
                       where p.BHABILITADO == 1
                       select new SelectListItem
                       {
                           Text = p.NOMBRE,
                           Value = p.IIDROL.ToString()
                       }).ToList();
            DropRol.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownListRol = DropRol;
        }

        List<SelectListItem> DropPagina;

        private void DropDownPagina()
        {
            DropPagina = (from p in db.Pagina
                       where p.BHABILITADO == 1
                       select new SelectListItem
                       {
                           Text = p.MENSAJE,
                           Value = p.IIDPAGINA.ToString()
                       }).ToList();
            DropPagina.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownListPagina = DropPagina;
        }

        #endregion


        public ActionResult Filtro(int? IDRolFilter)
        {
            List<RolPaginaModel> listModel = null;
            if (IDRolFilter == null)
            {
                listModel = (from rp in db.RolPagina
                             join p in db.Pagina on rp.IIDPAGINA equals p.IIDPAGINA
                             join r in db.Rol on rp.IIDROL equals r.IIDROL
                             where rp.BHABILITADO == 1
                             select new RolPaginaModel
                             {
                                 IDRolPagina = rp.IIDROLPAGINA,
                                 nombrePagina = p.MENSAJE,
                                 nombreRol = r.NOMBRE
                             }).ToList();
            }
            else
            {
                listModel = (from rp in db.RolPagina
                             join p in db.Pagina on rp.IIDPAGINA equals p.IIDPAGINA
                             join r in db.Rol on rp.IIDROL equals r.IIDROL
                             where rp.BHABILITADO == 1 && rp.IIDROL == IDRolFilter
                             select new RolPaginaModel
                             {
                                 IDRolPagina = rp.IIDROLPAGINA,
                                 nombrePagina = p.MENSAJE,
                                 nombreRol = r.NOMBRE
                             }).ToList();
            }

            return PartialView("_TablaRolPagina", listModel);

        }


        public string Save(RolPaginaModel model, int accion)
        {
            string rpta = "";

            try
            {
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
                        cantidad = db.RolPagina.Where(p => p.IIDPAGINA == model.IDPagina && p.IIDROL == model.IDRol && p.BHABILITADO == 1).Count();
                        //Validamos si ya existe en la BD
                        if (cantidad >= 1)
                        {
                            rpta = "-1";
                        }
                        else
                        {
                            RolPagina oRolPagina = new RolPagina();
                            oRolPagina.IIDPAGINA = model.IDPagina;
                            oRolPagina.IIDROL = model.IDRol;
                            oRolPagina.BHABILITADO = 1;
                            db.RolPagina.Add(oRolPagina);
                            rpta = db.SaveChanges().ToString();
                            if (rpta == "0")
                            {
                                rpta = "";
                            }
                        }
                    }
                    else
                    {
                        cantidad = db.RolPagina.Where(p => p.IIDPAGINA == model.IDPagina && p.IIDROL == model.IDRol && p.IIDROLPAGINA != accion && p.BHABILITADO == 1).Count();
                        //Validamos si ya existe en la BD
                        if (cantidad >= 1)
                        {
                            rpta = "-1";
                        }
                        else
                        {
                            RolPagina oRolPagina = db.RolPagina.Where(p => p.IIDROLPAGINA == accion).First();
                            oRolPagina.IIDPAGINA = model.IDPagina;
                            oRolPagina.IIDROL = model.IDRol;
                            rpta = db.SaveChanges().ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                rpta = "";
            }

            return rpta;
        }

        public int Delete(int id)
        { 
            int rpta = 0;

            try
            {
                RolPagina oRolPagina = db.RolPagina.Where(p => p.IIDROLPAGINA == id).First();
                oRolPagina.BHABILITADO = 0;
                rpta = db.SaveChanges();
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

        public JsonResult recuperarDatos(int accion)
        {
            RolPaginaModel model = new RolPaginaModel();
            RolPagina oRol = db.RolPagina.Where(p => p.IIDROLPAGINA == accion).First();
            model.IDPagina = (int)oRol.IIDPAGINA;
            model.IDRol = (int)oRol.IIDROL;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
