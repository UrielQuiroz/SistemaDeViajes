using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class RolPaginaModel
    {
        [Display(Name = "ID")]
        public int IDRolPagina { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public int IDRol { get; set; }

        [Required]
        [Display(Name = "Pagina")]
        public int IDPagina { get; set; }
        public int bHabilitado { get; set; }

        //Propiedades Adicionalees
        [Display(Name = "Rol")]
        public string nombreRol { get; set; }

        [Display(Name = "Pagina")]
        public string nombrePagina { get; set; }
    }
}