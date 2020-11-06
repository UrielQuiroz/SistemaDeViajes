using SistemaDeViajes.Filters;
using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    [Acceso]
    public class UsuarioController : Controller
    {
        BDPasajeEntities db;

        public UsuarioController()
        {
            db = new BDPasajeEntities();
       
        }

        #region DropDowns
        private void DropDownPersona()
        {
            List<SelectListItem> DropPersonas = new List<SelectListItem>();

            List<SelectListItem> DropCliente = (from p in db.Cliente
                                                where p.BHABILITADO == 1 && p.bTieneUsuario != 1
                                                select new SelectListItem
                                                {
                                                    Text = p.NOMBRE + " " + p.APPATERNO + " (C)",
                                                    Value = p.IIDCLIENTE.ToString()
                                                }).ToList();

            List<SelectListItem> DropEmpleado = (from p in db.Empleado
                                                 where p.BHABILITADO == 1 && p.bTieneUsuario != 1
                                                 select new SelectListItem
                                                 {
                                                     Text = p.NOMBRE + " " + p.APPATERNO + " (E)",
                                                     Value = p.IIDEMPLEADO.ToString()
                                                 }).ToList();
            DropPersonas.AddRange(DropCliente);
            DropPersonas.AddRange(DropEmpleado);
            DropPersonas = DropPersonas.OrderBy(p => p.Text).ToList();
            DropPersonas.Insert(0, new SelectListItem { Text = "--SELECCIONE--", Value = "" });
            ViewBag.DropDownPersonas = DropPersonas;
        }

        private void DropDownRol()
        {
            List<SelectListItem> DropRol;
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
        #endregion

        // GET: Usuario
        public ActionResult Index()
        {
            DropDownPersona();
            DropDownRol();

            List<UsuarioModel> listUsuario = new List<UsuarioModel>();

            List<UsuarioModel> listUsuarioCliente = new List<UsuarioModel>();
            listUsuarioCliente = (from u in db.Usuario
                                   join c in db.Cliente on u.IID equals c.IIDCLIENTE
                                   join r in db.Rol on u.IIDROL equals r.IIDROL
                                   where u.bhabilitado == 1 && u.TIPOUSUARIO == "C"
                                   select new UsuarioModel
                                   {
                                       IdUsuario = u.IIDUSUARIO,
                                       nombrePersona = c.NOMBRE + " " + c.APPATERNO,
                                       nombreUsuario = u.NOMBREUSUARIO,
                                       nombreRol = r.NOMBRE,
                                       nombrTipoUsuario = "Cliente"

                                   }).ToList();


            List<UsuarioModel> listUsuarioEmpleado = new List<UsuarioModel>();
            listUsuarioEmpleado = (from u in db.Usuario
                                  join e in db.Empleado on u.IID equals e.IIDEMPLEADO
                                  join r in db.Rol on u.IIDROL equals r.IIDROL
                                  where u.bhabilitado == 1 && u.TIPOUSUARIO == "E"
                                  select new UsuarioModel
                                  {
                                      IdUsuario = u.IIDUSUARIO,
                                      nombrePersona = e.NOMBRE + " " + e.APPATERNO,
                                      nombreUsuario = u.NOMBREUSUARIO,
                                      nombreRol = r.NOMBRE,
                                      nombrTipoUsuario = "Empleado"

                                  }).ToList();

            listUsuario.AddRange(listUsuarioCliente);
            listUsuario.AddRange(listUsuarioEmpleado);
            listUsuario = listUsuario.OrderBy(p => p.IdUsuario).ToList();
            return View(listUsuario);
        }


        public ActionResult Filtro(string nombrePersonaFilter)
        {
            DropDownPersona();
            DropDownRol(); 

            List<UsuarioModel> listUsuario = new List<UsuarioModel>();

            if (nombrePersonaFilter == null)
            {
                List<UsuarioModel> listUsuarioCliente = new List<UsuarioModel>();
                listUsuarioCliente = (from u in db.Usuario
                                      join c in db.Cliente on u.IID equals c.IIDCLIENTE
                                      join r in db.Rol on u.IIDROL equals r.IIDROL
                                      where u.bhabilitado == 1 && u.TIPOUSUARIO == "C"
                                      select new UsuarioModel
                                      {
                                          IdUsuario = u.IIDUSUARIO,
                                          nombrePersona = c.NOMBRE + " " + c.APPATERNO,
                                          nombreUsuario = u.NOMBREUSUARIO,
                                          nombreRol = r.NOMBRE,
                                          nombrTipoUsuario = "Cliente"

                                      }).ToList();


                List<UsuarioModel> listUsuarioEmpleado = new List<UsuarioModel>();
                listUsuarioEmpleado = (from u in db.Usuario
                                       join e in db.Empleado on u.IID equals e.IIDEMPLEADO
                                       join r in db.Rol on u.IIDROL equals r.IIDROL
                                       where u.bhabilitado == 1 && u.TIPOUSUARIO == "E"
                                       select new UsuarioModel
                                       {
                                           IdUsuario = u.IIDUSUARIO,
                                           nombrePersona = e.NOMBRE + " " + e.APPATERNO,
                                           nombreUsuario = u.NOMBREUSUARIO,
                                           nombreRol = r.NOMBRE,
                                           nombrTipoUsuario = "Empleado"

                                       }).ToList();

                listUsuario.AddRange(listUsuarioCliente);
                listUsuario.AddRange(listUsuarioEmpleado);
                listUsuario = listUsuario.OrderBy(p => p.IdUsuario).ToList();
            }
            else
            {
                List<UsuarioModel> listUsuarioCliente = new List<UsuarioModel>();
                listUsuarioCliente = (from u in db.Usuario
                                      join c in db.Cliente on u.IID equals c.IIDCLIENTE
                                      join r in db.Rol on u.IIDROL equals r.IIDROL
                                      where u.bhabilitado == 1 && u.TIPOUSUARIO == "C" &&
                                      (
                                        c.NOMBRE.Contains(nombrePersonaFilter) || c.APPATERNO.Contains(nombrePersonaFilter) || c.APMATERNO.Contains(nombrePersonaFilter)
                                      )
                                      select new UsuarioModel
                                      {
                                          IdUsuario = u.IIDUSUARIO,
                                          nombrePersona = c.NOMBRE + " " + c.APPATERNO,
                                          nombreUsuario = u.NOMBREUSUARIO,
                                          nombreRol = r.NOMBRE,
                                          nombrTipoUsuario = "Cliente"

                                      }).ToList();


                List<UsuarioModel> listUsuarioEmpleado = new List<UsuarioModel>();
                listUsuarioEmpleado = (from u in db.Usuario
                                       join e in db.Empleado on u.IID equals e.IIDEMPLEADO
                                       join r in db.Rol on u.IIDROL equals r.IIDROL
                                       where u.bhabilitado == 1 && u.TIPOUSUARIO == "E" &&
                                       (
                                         e.NOMBRE.Contains(nombrePersonaFilter) || e.APPATERNO.Contains(nombrePersonaFilter) || e.APMATERNO.Contains(nombrePersonaFilter)
                                       )
                                       select new UsuarioModel
                                       {
                                           IdUsuario = u.IIDUSUARIO,
                                           nombrePersona = e.NOMBRE + " " + e.APPATERNO,
                                           nombreUsuario = u.NOMBREUSUARIO,
                                           nombreRol = r.NOMBRE,
                                           nombrTipoUsuario = "Empleado"

                                       }).ToList();

                listUsuario.AddRange(listUsuarioCliente);
                listUsuario.AddRange(listUsuarioEmpleado);
                listUsuario = listUsuario.OrderBy(p => p.IdUsuario).ToList();
            }


            return PartialView("_TablaUsuario", listUsuario);
        }


        public string Save(UsuarioModel model, int Accion)
        {
            string rpta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    var query = (from values in ModelState.Values
                                 from error in values.Errors
                                 select error.ErrorMessage).ToList();

                    rpta += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        rpta += "<li class='list-group'>" + item + "</li>";
                    }
                    rpta += "</ul>";

                }
                else
                {
                    int cantidad = 0;
                    if (Accion == -1)
                    {
                        cantidad = db.Usuario.Where(p => p.NOMBREUSUARIO == model.Nombre && p.bhabilitado == 1).Count();
                        //Validamos si ya existe el nombre del usuario
                        if (cantidad >= 1)
                        {
                            rpta = "-1";
                        }
                        else
                        {
                            //Agregar
                            Usuario oUsuario = new Usuario();
                            oUsuario.NOMBREUSUARIO = model.Nombre;

                            //Cifrar password
                            SHA256Managed sha = new SHA256Managed();
                            byte[] bytePassword = Encoding.Default.GetBytes(model.Contraseña);
                            byte[] bytePassCifrada = sha.ComputeHash(bytePassword);
                            string passCifrada = BitConverter.ToString(bytePassCifrada).Replace("-", "");

                            oUsuario.CONTRA = passCifrada;
                            oUsuario.TIPOUSUARIO = model.nombrePersona.Substring(model.nombrePersona.Length - 2, 1);
                            oUsuario.IID = model.ID;
                            oUsuario.IIDROL = model.IdRol;
                            oUsuario.bhabilitado = 1;
                            db.Usuario.Add(oUsuario);

                            if (oUsuario.TIPOUSUARIO.Equals("C"))
                            {
                                Cliente oCliente = db.Cliente.Where(p => p.IIDCLIENTE == oUsuario.IID).FirstOrDefault();
                                oCliente.bTieneUsuario = 1;
                            }
                            else
                            {
                                Empleado oEmpleado = db.Empleado.Where(p => p.IIDEMPLEADO == oUsuario.IID).First();
                                oEmpleado.bTieneUsuario = 1;
                            }

                            rpta = db.SaveChanges().ToString();

                            if (rpta == "0")
                            {
                                rpta = "";
                            }

                        }

                    }
                    else
                    {
                        cantidad = db.Usuario.Where(p => p.NOMBREUSUARIO == model.Nombre && p.bhabilitado == 1 && p.IIDUSUARIO != Accion).Count();
                        //Validamos si ya existe el nombre del usuario
                        if (cantidad >= 1)
                        {
                            rpta = "-1";
                        }
                        else
                        {
                            //Editar
                            Usuario oUser = db.Usuario.Where(p => p.IIDUSUARIO == Accion).First();
                            oUser.NOMBREUSUARIO = model.Nombre;
                            oUser.IIDROL = model.IdRol;
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
                Usuario oUser = db.Usuario.Where(p => p.IIDUSUARIO == id).First();
                oUser.bhabilitado = 0;

                rpta = db.SaveChanges();
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

        public JsonResult recuperardatos(int id)
        {
            UsuarioModel model = new UsuarioModel();
            Usuario oUser = db.Usuario.Where(p => p.IIDUSUARIO == id).First();
            model.Nombre = oUser.NOMBREUSUARIO;
            model.IdRol = (int) oUser.IIDROL;

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}