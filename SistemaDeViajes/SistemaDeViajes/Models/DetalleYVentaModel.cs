using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class DetalleYVentaModel
    {
        public List<ReservasRealizadasModel> ReservasRealizadas { get; set; }
        public List<DetalleVentaModel> detalleVenta { get; set; } 
    }
}