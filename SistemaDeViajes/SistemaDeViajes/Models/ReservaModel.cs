using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class ReservaModel
    {
        public int IDViaje { get; set; }
        public string nombreFoto { get; set; }
        public byte[] Foto { get; set; }
        public string lugarOrigen { get; set; }
        public string lugarDestino { get; set; }
        public decimal precio { get; set; }
        public DateTime fechaViaje { get; set; }
        public string placaBus { get; set; }
        public string DescripcionBus { get; set; }
        public int asientosDisponibles { get; set; }
        public int totalAsientos { get; set; }

        public int IdBus { get; set; }

        public int cantidad { get; set; }
    }
}