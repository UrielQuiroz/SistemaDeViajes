using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeViajes.Models
{
    public class BusModel
    {

        [Display(Name = "ID")]
        public int IDBus { get; set; }

        [Required]
        [Display(Name = "Sucursal")]
        public int IDSucursal { get; set; }


        [Required]
        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Compra")]
        public DateTime fechaCompra { get; set; }

        [Required]
        [Display(Name = "Marca")]
        public int IDMarca { get; set; }

        [Required]
        [Display(Name = "Modelo")]
        public int IDModelo { get; set; }


        [Required]
        [Display(Name = "Filas")]
        public int NumFilas { get; set; }


        [Required]
        [Display(Name = "Columnas")]
        public int numColumnas { get; set; }

        [Required]
        [Display(Name = "Tipo de Bus")]
        public int IDTipoBus { get; set; }


        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        public int bHabilitado { get; set; }
         



        //Propiedades Adicionales
        [Display(Name = "Sucursal")]
        public string Sucursal { get; set; }
         
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Display(Name = "Tipo de Bus")]
        public string TipoBus { get; set; }

        //Propiedad adicional para errores
        public string msjError { get; set; }
    }
}