using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class PaginaModel
    {
        [Display(Name = "ID")]
        public int IDPagina { get; set; }

   
        [Required]
        [Display(Name ="Mensaje")]
        public string Mensaje { get; set; }


        [Required]
        [Display(Name = "Acción")]
        public string Accion { get; set; }

        [Required]
        [Display(Name = "Controlador")]
        public string Controlador { get; set; }
        public int bHabilitado { get; set; }
    }
}