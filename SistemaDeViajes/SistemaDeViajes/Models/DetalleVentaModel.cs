using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class DetalleVentaModel
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdViaje { get; set; }
        public int idBus { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }

        //Propiedades adicionales
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Placa { get; set; }
    }
}