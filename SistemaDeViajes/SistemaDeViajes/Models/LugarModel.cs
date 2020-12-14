using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class LugarModel
    {
        public int IDLugar { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int bHabilitado { get; set; }
    }
}