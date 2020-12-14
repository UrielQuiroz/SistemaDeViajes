using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    public class LugarController : Controller
    {
        BDPasajeEntities db;

        public LugarController()
        {
            db = new BDPasajeEntities();
        }
        // GET: Lugar
        public ActionResult Index()
        {
            List<LugarModel> listModel;

            listModel = (from p in db.Lugar
                         where p.BHABILITADO == 1
                         select new LugarModel
                         {
                             IDLugar = p.IIDLUGAR,
                             Nombre = p.NOMBRE,
                             Descripcion = p.DESCRIPCION
                         }).ToList();

            return View();
        }
    }
}