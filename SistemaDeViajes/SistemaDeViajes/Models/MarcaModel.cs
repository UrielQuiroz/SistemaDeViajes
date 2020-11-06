using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class MarcaModel
    {
        [Display(Name = "ID")]
        public int IDMarca { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }

        public int bHabilitado { get; set; }

        //Propiedad adicional para errores
        public string msjError { get; set; }
    }
}