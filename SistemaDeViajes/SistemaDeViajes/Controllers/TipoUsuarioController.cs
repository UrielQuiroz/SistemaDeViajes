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
    public class TipoUsuarioController : Controller
    {
        BDPasajeEntities db;

        public TipoUsuarioController()
        {
            db = new BDPasajeEntities();
        }


        //Variable que almacena el modelo que recibmos de la vista "Index"
        private TipoUsuarioModel oModelVal;
         
        private bool SearchTypeUser(TipoUsuarioModel model)
        {
            bool searchID = true;
            bool searchNombre = true;
            bool searchDescripcion = true;

            if (oModelVal.IdTipoUsuario > 0)
            {
                searchID = model.IdTipoUsuario.ToString().Contains(oModelVal.IdTipoUsuario.ToString());
            }

            if (oModelVal.Nombre != null)
            {
                searchNombre = model.Nombre.ToString().Contains(oModelVal.Nombre);
            }

            if (oModelVal.Descripcion != null)
            {
                searchDescripcion = model.Descripcion.ToString().Contains(oModelVal.Descripcion);
            }

            return (searchID & searchNombre & searchDescripcion); 
        }


        // GET: TipoUsuario
        public ActionResult Index(TipoUsuarioModel oModel)
        {
            //Pasamos el modelo a la variable global "oModelVal"
            oModelVal = oModel;

            List<TipoUsuarioModel> listModel = null;

            //Cramos una variable para hacer el filtrado
            List<TipoUsuarioModel> listFilterModel;

            listModel = (from p in db.TipoUsuario
                         where p.BHABILITADO == 1
                         select new TipoUsuarioModel
                         {
                             IdTipoUsuario = p.IIDTIPOUSUARIO,
                             Nombre = p.NOMBRE,
                             Descripcion = p.DESCRIPCION
                         }).ToList();

            if (oModel.IdTipoUsuario == 0 && oModel.Nombre == null && oModel.Descripcion == null)
            {
                listFilterModel = listModel;
            }
            else
            {
                Predicate<TipoUsuarioModel> predicate = new Predicate<TipoUsuarioModel>(SearchTypeUser);
                listFilterModel = listModel.FindAll(predicate);
            }

            return View(listFilterModel);
        }
    }
}