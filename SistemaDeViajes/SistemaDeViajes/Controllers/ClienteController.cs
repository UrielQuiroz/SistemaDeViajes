using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    public class ClienteController : Controller
    {

        BDPasajeEntities db;
        public ClienteController()
        {
            db = new BDPasajeEntities();
        }

        // GET: Cliente
        public ActionResult Index(ClientesModel oModel)
        {
            List<ClientesModel> listModel = null;
            int sexo = oModel.idSexo;

            DropDownSexo();
            ViewBag.lista = listSexo;


            if (sexo == 0)
            {
                listModel = (from c in db.Cliente
                             where c.BHABILITADO == 1
                             select new ClientesModel
                             {
                                 IDCliente = c.IIDCLIENTE,
                                 Nombre = c.NOMBRE + " " + c.APPATERNO,
                                 Email = c.EMAIL,
                                 Direccion = c.DIRECCION,
                                 idSexo = (int)c.IIDSEXO,
                                 TelefonoFijo = c.TELEFONOFIJO,
                                 TelefonoCelular = c.TELEFONOCELULAR

                             }).ToList();
            }
            else
            {
                listModel = (from c in db.Cliente
                             where c.BHABILITADO == 1 && c.IIDSEXO == sexo
                             select new ClientesModel
                             {
                                 IDCliente = c.IIDCLIENTE,
                                 Nombre = c.NOMBRE + " " + c.APPATERNO,
                                 Email = c.EMAIL,
                                 Direccion = c.DIRECCION,
                                 idSexo = (int)c.IIDSEXO,
                                 TelefonoFijo = c.TELEFONOFIJO,
                                 TelefonoCelular = c.TELEFONOCELULAR

                             }).ToList();
            }

            return View(listModel);
        }

        #region DropDowns

        List<SelectListItem> listSexo;

        private void DropDownSexo()
        {
            listSexo = (from s in db.Sexo
                        where s.BHABILITADO == 1
                        select new SelectListItem
                        {
                            Text = s.NOMBRE,
                            Value = s.IIDSEXO.ToString()
                        }).ToList();
            listSexo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
        }

        #endregion


        public ActionResult Add()
        {
            DropDownSexo();
            ViewBag.lista = listSexo;
            return View();
        }



        [HttpPost]
        public ActionResult Add(ClientesModel model)
        {
            int regEncontrados = 0;
            string nombre = model.Nombre;
            string apellidoPaterno = model.ApPaterno;
            string apellidoMaterno = model.ApMaterno;

            //Validamos si ya existe el nombre
            regEncontrados = db.Cliente.Where(p => p.NOMBRE == nombre && p.APPATERNO == apellidoPaterno && p.APMATERNO == apellidoMaterno).Count();

            if (!ModelState.IsValid || regEncontrados >= 1)
            {
                DropDownSexo();
                ViewBag.lista = listSexo;

                if (regEncontrados >= 1)
                {
                    model.msjError = "El nombre del cliente ya existe";
                }

                return View(model);
            }
            else
            {
                Cliente oCliente = new Cliente();
                oCliente.NOMBRE = model.Nombre;
                oCliente.APPATERNO = model.ApPaterno;
                oCliente.APMATERNO = model.ApMaterno;
                oCliente.EMAIL = model.Email;
                oCliente.DIRECCION = model.Direccion;
                oCliente.IIDSEXO = model.idSexo;
                oCliente.TELEFONOFIJO = model.TelefonoFijo;
                oCliente.TELEFONOCELULAR = model.TelefonoCelular;
                oCliente.BHABILITADO = 1;
                db.Cliente.Add(oCliente);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            DropDownSexo();
            ViewBag.lista = listSexo;
            ClientesModel model = new ClientesModel();
            Cliente oCliente = db.Cliente.Where(p => p.IIDCLIENTE == id).First();

            model.IDCliente = oCliente.IIDCLIENTE;
            model.Nombre = oCliente.NOMBRE;
            model.ApPaterno = oCliente.APPATERNO;
            model.ApMaterno = oCliente.APMATERNO;
            model.Email = oCliente.EMAIL;
            model.Direccion = oCliente.DIRECCION;
            model.idSexo = (int)oCliente.IIDSEXO;
            model.TelefonoFijo = oCliente.TELEFONOFIJO;
            model.TelefonoCelular = oCliente.TELEFONOCELULAR;

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(ClientesModel model)
        {
            int regEncontrados = 0;
            string nombre = model.Nombre;
            string apellidoPaterno = model.ApPaterno;
            string apellidoMaterno = model.ApMaterno;
            int idCliente = model.IDCliente;

            regEncontrados = db.Cliente.Where(p => p.NOMBRE == nombre && p.APPATERNO == apellidoPaterno && p.APMATERNO == apellidoMaterno && p.IIDCLIENTE != idCliente).Count();

            if (!ModelState.IsValid || regEncontrados >= 1)
            {
                DropDownSexo();
                ViewBag.lista = listSexo;
                if (regEncontrados >= 1)
                {
                    model.msjError = "El nombre del cliente ya existe";
                }
                return View(model);
            }
            else
            {
                Cliente oCliente = db.Cliente.Where(p => p.IIDCLIENTE == idCliente).FirstOrDefault();
                oCliente.NOMBRE = model.Nombre;
                oCliente.APPATERNO = model.ApPaterno;
                oCliente.APMATERNO = model.ApMaterno;
                oCliente.EMAIL = model.Email;
                oCliente.DIRECCION = model.Direccion;
                oCliente.IIDSEXO = model.idSexo;
                oCliente.TELEFONOFIJO = model.TelefonoFijo;
                oCliente.TELEFONOCELULAR = model.TelefonoCelular;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Cliente oCliente = db.Cliente.Where(p => p.IIDCLIENTE == id).FirstOrDefault();
            oCliente.BHABILITADO = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}