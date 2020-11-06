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
    public class PaginaController : Controller
    {
        BDPasajeEntities db;

        public PaginaController()
        {
            db = new BDPasajeEntities();
        }
        // GET: Pagina
        public ActionResult Index()
        {
            List<PaginaModel> listModel = new List<PaginaModel>();
            listModel = (from p in db.Pagina
                         where p.BHABILITADO == 1
                         select new PaginaModel
                         {
                             IDPagina = p.IIDPAGINA,
                             Mensaje = p.MENSAJE,
                             Accion = p.ACCION,
                             Controlador = p.CONTROLADOR
                         }).ToList();

            return View(listModel);
        }

        public ActionResult Filtro(string MensajePagina)
        {
            List<PaginaModel> listModel = new List<PaginaModel>();

            if (MensajePagina == null)
            {
                listModel = (from p in db.Pagina
                             where p.BHABILITADO == 1
                             select new PaginaModel
                             {
                                 IDPagina = p.IIDPAGINA,
                                 Mensaje = p.MENSAJE,
                                 Accion = p.ACCION,
                                 Controlador = p.CONTROLADOR
                             }).ToList();
            }
            else
            {
                listModel = (from p in db.Pagina
                             where p.BHABILITADO == 1 && p.MENSAJE.Contains(MensajePagina)
                             select new PaginaModel
                             {
                                 IDPagina = p.IIDPAGINA,
                                 Mensaje = p.MENSAJE,
                                 Accion = p.ACCION,
                                 Controlador = p.CONTROLADOR
                             }).ToList();
            }

            return PartialView("_TablaPagina", listModel);
        }

        public string Save(PaginaModel model, int operacion)
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
                    if (operacion == -1)
                    {
                        cantidad = db.Pagina.Where(p => p.MENSAJE == model.Mensaje).Count();
                        if (cantidad >= 1)
                        {
                            rpta = "-1";
                        }
                        else
                        {
                            //Ageragar
                            Pagina oPagina = new Pagina();
                            oPagina.MENSAJE = model.Mensaje;
                            oPagina.ACCION = model.Accion;
                            oPagina.CONTROLADOR = model.Controlador;
                            oPagina.BHABILITADO = 1;
                            db.Pagina.Add(oPagina);
                            rpta = db.SaveChanges().ToString();
                            if (rpta == "0")
                            {
                                rpta = "";
                            }
                        }
                    }
                    else
                    {
                        cantidad = db.Pagina.Where(p => p.MENSAJE == model.Mensaje && p.IIDPAGINA != operacion).Count();
                        if (cantidad >= 1)
                        {
                            rpta = "-1";
                        }
                        else
                        {
                            Pagina oPagina = db.Pagina.Where(p => p.IIDPAGINA == operacion).First();
                            oPagina.MENSAJE = model.Mensaje;
                            oPagina.ACCION = model.Accion;
                            oPagina.CONTROLADOR = model.Controlador;
                            rpta = db.SaveChanges().ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                rpta = "";
            }

            return rpta;
        }

        public int Delete(int idPag)
        {
            int rpta = 0;

            try
            {
                Pagina oPagina = db.Pagina.Where(p => p.IIDPAGINA == idPag).First();
                oPagina.BHABILITADO = 0;
                rpta = db.SaveChanges();
            }
            catch (Exception ex)
            {
                rpta = 0;
            }
            return rpta;
        }

        public JsonResult recuperarDatos(int operacion)
        {
            PaginaModel listPage = (from p in db.Pagina
                                    where p.BHABILITADO == 1 && p.IIDPAGINA == operacion
                                    select new PaginaModel
                                    {
                                        Mensaje = p.MENSAJE,
                                        Accion = p.ACCION,
                                        Controlador = p.CONTROLADOR
                                    }).First();

            return Json(listPage, JsonRequestBehavior.AllowGet);
        }
    }
}