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
    public class EmpleadoController : Controller
    {
        BDPasajeEntities db;

        public EmpleadoController()
        {
            db = new BDPasajeEntities();
        }
        // GET: Empleado
        public ActionResult Index(EmpleadoModel oModel)
        {
            List<EmpleadoModel> model = null;
            DropTipoUser();
            int idTipoUser = oModel.IDTipoUser;

            if (idTipoUser == 0)
            {
                model = (from e in db.Empleado
                         join tu in db.TipoUsuario on e.IIDTIPOUSUARIO equals tu.IIDTIPOUSUARIO
                         join tc in db.TipoContrato on e.IIDTIPOCONTRATO equals tc.IIDTIPOCONTRATO
                         where e.BHABILITADO == 1
                         select new EmpleadoModel
                         {
                             IDEmpleado = e.IIDEMPLEADO,
                             Nombre = e.NOMBRE + " " + e.APPATERNO,
                             nombreTipoUsuario = tu.NOMBRE,
                             NombreContrato = tc.NOMBRE

                         }).ToList();
            }
            else
            {
                model = (from e in db.Empleado
                         join tu in db.TipoUsuario on e.IIDTIPOUSUARIO equals tu.IIDTIPOUSUARIO
                         join tc in db.TipoContrato on e.IIDTIPOCONTRATO equals tc.IIDTIPOCONTRATO
                         where e.BHABILITADO == 1 && e.IIDTIPOUSUARIO == idTipoUser
                         select new EmpleadoModel
                         {
                             IDEmpleado = e.IIDEMPLEADO,
                             Nombre = e.NOMBRE + " " + e.APPATERNO,
                             nombreTipoUsuario = tu.NOMBRE,
                             NombreContrato = tc.NOMBRE

                         }).ToList();
            }


            return View(model);
        }

        #region DropDowns


        List<SelectListItem> listUser;
        private void DropTipoUser()
        {
            listUser = (from p in db.TipoUsuario
                        where p.BHABILITADO == 1
                        select new SelectListItem
                        {
                            Text = p.NOMBRE,
                            Value = p.IIDTIPOUSUARIO.ToString()

                        }).ToList();

            listUser.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.listDropUser = listUser;
        }


        List<SelectListItem> listContrato;
        private void DropTipoContrato()
        {
            listContrato = (from p in db.TipoContrato
                            where p.BHABILITADO == 1
                            select new SelectListItem
                            {
                                Text = p.NOMBRE,
                                Value = p.IIDTIPOCONTRATO.ToString()
                            }).ToList();

            listContrato.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.listDropContrato = listContrato;
        }

        List<SelectListItem> listSexo;
        private void DropSexo()
        {
            listSexo = (from p in db.Sexo
                        where p.BHABILITADO == 1
                        select new SelectListItem
                        {
                            Text = p.NOMBRE,
                            Value = p.IIDSEXO.ToString()
                        }).ToList();

            listSexo.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.listDropSexo = listSexo;
        }

        public void listarDropDowns()
        {
            DropSexo();
            DropTipoContrato();
            DropTipoUser();
        }

        #endregion

        public ActionResult Add()
        {
            listarDropDowns();
            return View();
        }


        [HttpPost]
        public ActionResult Add(EmpleadoModel model)
        {
            int regEncontrados = 0;
            string nombre = model.Nombre;
            string apellidoP = model.ApPaterno;
            string apellidoM = model.ApMaterno; 

            regEncontrados = db.Empleado.Where(p => p.NOMBRE == nombre && p.APPATERNO == apellidoP && p.APMATERNO == apellidoM).Count();

            if (!ModelState.IsValid || regEncontrados>=1)
            {
                if (regEncontrados >= 1)
                {
                    model.msjError = "El nombre del empleado ya existe";
                }
                listarDropDowns();
                return View(model);
            }
            else
            {
                Empleado oEmpleado = new Empleado();
                oEmpleado.NOMBRE = model.Nombre;
                oEmpleado.APPATERNO = model.ApPaterno;
                oEmpleado.APMATERNO = model.ApMaterno;
                oEmpleado.FECHACONTRATO = model.FechaContrato;
                oEmpleado.SUELDO = model.Sueldo;
                oEmpleado.IIDTIPOUSUARIO = model.IDTipoUser;
                oEmpleado.IIDTIPOCONTRATO = model.IDTipoContrato;
                oEmpleado.IIDSEXO = model.IdSexo;
                oEmpleado.BHABILITADO = 1;
                db.Empleado.Add(oEmpleado);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            listarDropDowns();
            EmpleadoModel model = new EmpleadoModel();
            Empleado oEmpleado = db.Empleado.Where(p => p.IIDEMPLEADO == id).FirstOrDefault();

            model.IDEmpleado = oEmpleado.IIDEMPLEADO;
            model.Nombre = oEmpleado.NOMBRE;
            model.ApPaterno = oEmpleado.APPATERNO;
            model.ApMaterno = oEmpleado.APMATERNO;
            model.FechaContrato = (DateTime) oEmpleado.FECHACONTRATO;
            model.Sueldo = (decimal) oEmpleado.SUELDO;
            model.IDTipoUser = (int) oEmpleado.IIDTIPOUSUARIO;
            model.IDTipoContrato = (int) oEmpleado.IIDTIPOCONTRATO;
            model.IdSexo = (int) oEmpleado.IIDSEXO;

            return View(model);

        }


        [HttpPost]
        public ActionResult Edit(EmpleadoModel model)
        {
            int regEncontrados = 0;
            string nombre = model.Nombre;
            string apellidoP = model.ApPaterno;
            string apellidoM = model.ApMaterno;
            int idEmpleado = model.IDEmpleado;

            //Validamos y el nombre del empleado ya existe
            regEncontrados = db.Empleado.Where(p => p.NOMBRE == nombre && p.APPATERNO == apellidoP && p.APMATERNO == apellidoM && p.IIDEMPLEADO != idEmpleado).Count();


            if (!ModelState.IsValid || regEncontrados >= 1)
            {
                if (regEncontrados >= 1)
                {
                    model.msjError = "El nombre del empleado ya existe";
                }
                listarDropDowns();
                return View(model);
            }
            else
            {
                Empleado oEmpleado = db.Empleado.Where(p => p.IIDEMPLEADO == idEmpleado).First();
                oEmpleado.NOMBRE = model.Nombre;
                oEmpleado.APPATERNO = model.ApPaterno;
                oEmpleado.APMATERNO = model.ApMaterno;
                oEmpleado.FECHACONTRATO = model.FechaContrato;
                oEmpleado.SUELDO = model.Sueldo;
                oEmpleado.IIDTIPOUSUARIO = model.IDTipoUser;
                oEmpleado.IIDTIPOCONTRATO = model.IDTipoContrato;
                oEmpleado.IIDSEXO = model.IdSexo;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Empleado oEmpleado = db.Empleado.Where(p => p.IIDEMPLEADO == id).FirstOrDefault();
            oEmpleado.BHABILITADO = 0;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}