using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class ClientesModel
    {
        [Display(Name = "ID")]
        public int IDCliente { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }


        [Required]
        [Display(Name = "Apellido Paterno")]
        public string ApPaterno { get; set; }


        [Required]
        [Display(Name = "Apellido Materno")]
        public string ApMaterno { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Ingrese un e-mail valido!")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [Required]
        [Display(Name = "Sexo")]
        public int idSexo { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string TelefonoFijo { get; set; }

        [Required]
        [Display(Name = "Celular")]
        public string TelefonoCelular { get; set; }
        public int bHabilitado { get; set; }

        //Propedad adicional para errores
        public string msjError { get; set; }
    }
}