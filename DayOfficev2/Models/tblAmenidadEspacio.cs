//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DayOfficev2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblAmenidadEspacio
    {
        public int IdAmenidadEspcio { get; set; }
        public int IdEspacio { get; set; }
        public int IdAmenidad { get; set; }
    
        public virtual tblAmenidades tblAmenidades { get; set; }
        public virtual tblEspacio tblEspacio { get; set; }
    }
}