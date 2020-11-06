using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class SucursalModel
    {
        [Display(Name ="ID")]
        public int IDSucursal { get; set; }


        [Display(Name = "Nombre")]
        [Required]
        public string Nombre { get; set; }


        [Display(Name = "Dirección")]
        [Required]
        public string Direccion { get; set; }


        [Display(Name = "Telefono")]
        [Required]
        public string Telefono { get; set; }

        [Display(Name = "E-mail")]
        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un e-mail valido!")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Apertura")]
        public DateTime FechaApertura { get; set; }

        public int bHabilitado { get; set; }


        //Propiedad adicional para errores
        public string msjError { get; set; }
    }
}