using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class TipoUsuarioModel
    {
        [Display(Name = "ID")]
        public int IdTipoUsuario { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public int bHabilitado { get; set; }
    }
}