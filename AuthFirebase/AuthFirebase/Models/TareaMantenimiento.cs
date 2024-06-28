using System;
using System.Collections.Generic;
using System.Text;

namespace AuthFirebase.Models
{
    public class TareaMantenimiento
    {
        public int id { get; set; }
        public string tareas { get; set; }
      
        public int Kilometraje { get; set; }
        public string Repuesto { get; set; }
        public string Observacion { get; set; }
    }
}
