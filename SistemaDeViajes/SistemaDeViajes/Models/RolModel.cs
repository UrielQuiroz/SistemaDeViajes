using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class RolModel
    {

        [Display(Name ="ID")]
        public int IDRol { get; set; }


        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo nombre es requerido")]
        public string Nombre { get; set; }


        [Required(ErrorMessage ="El campo Descripcion es requerido")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public int bHanilitado { get; set; }
    }
}