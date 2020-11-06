using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    public class HomeController : Controller
    {
        BDPasajeEntities db;

        public HomeController()
        {
            db = new BDPasajeEntities();
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}