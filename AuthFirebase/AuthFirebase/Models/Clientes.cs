using System;
using System.Collections.Generic;
using System.Text;

namespace AuthFirebase.Models
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public Ubicacion Ubicacion { get; set; }
        public Clientes()
        {
            Ubicacion = new Ubicacion(); // Inicializar Ubicacion en el constructor
        }
    }
}
