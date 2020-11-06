using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class EmpleadoModel
    {
        [Display(Name = "ID")]
        public int IDEmpleado { get; set; }

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
        [Display(Name = "Fecha de Contrato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaContrato { get; set; }

        [Required]
        [Display(Name = "Sueldo")]
        public decimal Sueldo { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int IDTipoUser { get; set; }

        [Required]
        [Display(Name = "Contrato")]
        public int IDTipoContrato { get; set; }

        [Required]
        [Display(Name = "Sexo")]
        public int IdSexo { get; set; }
        public int bHabilitado { get; set; }


        //Propiedades Adicionales
        [Display(Name = "Tipo de Contrato")]
        public string NombreContrato { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public string nombreTipoUsuario { get; set; }


        //Propiedad adicional para errores
        public string msjError { get; set; }

    }
}