using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Contraseña { get; set; }

        public string TipoUsuario { get; set; }

        [Required]
        public int ID { get; set; }

        [Required]
        public int IdRol { get; set; }


        public int bHabilitado { get; set; }

        //propiedad adicional
        public string nombrePersona { get; set; }
        public string nombreUsuario { get; set; }
        public string nombreRol { get; set; }
        public string nombrTipoUsuario { get; set; } 

        public string mensajeError { get; set; }
    }
}