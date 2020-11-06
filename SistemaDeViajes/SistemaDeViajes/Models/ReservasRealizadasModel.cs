using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class ReservasRealizadasModel
    {
        public int IDVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Total { get; set; }
    }
}