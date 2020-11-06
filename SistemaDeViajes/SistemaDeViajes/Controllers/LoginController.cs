using SistemaDeViajes.ClasesAuxiliares;
using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    public class LoginController : Controller
    {
        BDPasajeEntities db;

        public LoginController()
        {
            db = new BDPasajeEntities();
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            Session["Rol"] = null;
            return RedirectToAction("Index");
        }

        public string Login(UsuarioModel model)
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
                string nombreDeUsuario = model.Nombre;
                string contra = model.Contraseña;

                //Cifrar password
                SHA256Managed sha = new SHA256Managed();
                byte[] bytePassword = Encoding.Default.GetBytes(contra);
                byte[] bytePassCifrada = sha.ComputeHash(bytePassword);
                string passCifrada = BitConverter.ToString(bytePassCifrada).Replace("-", "");

                int veces = db.Usuario.Where(p => p.NOMBREUSUARIO == nombreDeUsuario && p.CONTRA == passCifrada).Count();
                rpta = veces.ToString();
                if (rpta == "0")
                {
                    rpta = "Usuario o contraseña incorrecta";
                }
                else
                {
                    Usuario oUsuario = db.Usuario.Where(p => p.NOMBREUSUARIO == nombreDeUsuario && p.CONTRA == passCifrada).First();
                    Session["Usuario"] = oUsuario;

                    if (oUsuario.TIPOUSUARIO == "C")
                    {
                        Cliente oCliente = db.Cliente.Where(p => p.IIDCLIENTE == oUsuario.IID).First();
                        Session["nombreCompleto"] = oCliente.NOMBRE + " " + oCliente.APPATERNO;
                    }
                    else
                    {
                        Empleado oEmpleado = db.Empleado.Where(p => p.IIDEMPLEADO == oUsuario.IID).First();
                        Session["nombreCompleto"] = oEmpleado.NOMBRE + " " + oEmpleado.APPATERNO;
                    }

                    List<MenuModel> listMenu = (from usuario in db.Usuario
                                                join rol in db.Rol on usuario.IIDROL equals rol.IIDROL
                                                join rolpagina in db.RolPagina on rol.IIDROL equals rolpagina.IIDROL
                                                join pagina in db.Pagina on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                                                where rol.IIDROL == oUsuario.IIDROL && rolpagina.IIDROL == oUsuario.IIDROL && usuario.IIDUSUARIO == oUsuario.IIDUSUARIO
                                                select new MenuModel
                                                {
                                                    msj = pagina.MENSAJE,
                                                    nombreAccion = pagina.ACCION,
                                                    nombreControlador = pagina.CONTROLADOR
                                                }).ToList();
                    Session["Rol"] = listMenu;

                }
            }

            return rpta;
        }


        public string RecuperarClave(string IIDTIPO, string correo, string celular)
        {

            string msj = "";

            int cantidad = 0;
            int idCliente;

            if (IIDTIPO == "C")
            {
                //existe una persona con esa información
                cantidad = db.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == celular).Count();
            }

            if (cantidad == 0)
            {
                msj = "No existe una persona registrada con esta información";
            }
            else
            {
                idCliente = db.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == celular).First().IIDCLIENTE;

                //Verificamos si existe el usuario
                int nveces = db.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == celular).Count();

                if (nveces == 0)
                {
                    msj = "No tiene usuario";
                }
                else
                {
                    //Obtenemos el Id del usuario
                    Usuario oUsuario = db.Usuario.Where(p => p.IID == idCliente && p.TIPOUSUARIO == "C").First();
                    //Modificamos su clave
                    Random ra = new Random();
                    int n1 = ra.Next(0, 9);
                    int n2 = ra.Next(0, 9);
                    int n3 = ra.Next(0, 9);
                    int n4 = ra.Next(0, 9);
                    string nuevaContra = n1.ToString() + n2 + n3 + n4;

                    //Cifrar password
                    SHA256Managed sha = new SHA256Managed();
                    byte[] bytePassword = Encoding.Default.GetBytes(nuevaContra);
                    byte[] bytePassCifrada = sha.ComputeHash(bytePassword);
                    string passCifrada = BitConverter.ToString(bytePassCifrada).Replace("-", "");
                    //Mandamos la nueva clave
                    oUsuario.CONTRA = passCifrada;
                    msj = db.SaveChanges().ToString();
                    Correo.EnviarCorreo(correo, "Recuperar clave", "Se actualizo su clave, ahora su clave es: " + nuevaContra, "");
                }

            }

            return msj;
        }
    }
}