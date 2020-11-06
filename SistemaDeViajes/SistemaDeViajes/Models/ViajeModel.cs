using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class ViajeModel
    {
        [Display(Name = "ID")]
        public int IDViaje { get; set; }


        [Required]
        [Display(Name = "Origen")]
        public int IDLugarOrige { get; set; }


        [Required]
        [Display(Name = "Destino")]
        public int IDLugarDestino { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaViaje { get; set; }

        [Required]
        [Display(Name = "Bus")]
        public int IDBus { get; set; }

        [Required]
        [Display(Name = "Asientos Disponibles")]
        public int NOAsientosDisponibles { get; set; }
        public int bHabilitado { get; set; }
        public string Foto { get; set; }


        //Propiedades adicionales
        [Display(Name = "Origen")]
        public string NombreOrigen { get; set; }
        [Display(Name = "Destino")]
        public string NombreDestino { get; set; }
        [Display(Name = "Bus")]
        public string NombreBus { get; set; }

        public string nombreFoto { get; set; }

        public string mensaje { get; set; }

        public string fechaViajeCadena { get; set; }

        public string extension { get; set; }

        public string fotoRecuperarCadena { get; set; }
        
    }
}