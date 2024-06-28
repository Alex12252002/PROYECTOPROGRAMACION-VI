using System;
using System.Collections.Generic;
using System.Text;

namespace AuthFirebase.Models
{
    public class API
    {
        public string ListarCliente { get; set; }
        public string CrearCliente { get; set; }
        public string ActualizarCliente { get; set; }
        public string EliminarCliente { get; set; }

        public string ListarVehiculo{ get; set; }
        public string CrearVehiculo { get; set; }
        public string ActualizarVehiculo { get; set; }
        public string EliminarVehiculo { get; set; }

        public string ListarMantenimiento { get; set; }
        public string CrearMantenimiento { get; set; }
        public string ActualizarMantenimiento { get; set; }
        public string EliminarMantenimiento { get; set; }

        public API()
        {
            this.ListarCliente = "http://192.168.100.7:5009/api/Clientes"; // URL para listar productos
            this.CrearCliente = "http://192.168.100.7:5009/api/Clientes";
            this.ActualizarCliente = "http://192.168.100.7:5009/api/Clientes/{id}"; // URL Para actualizar productos
            this.EliminarCliente = "http://192.168.100.7:5009/api/Clientes/{id}"; // URL Para eliminar productos

            this.ListarVehiculo = "http://192.168.100.7:5009/api/Vehiculoes"; // URL para listar productos
            this.CrearVehiculo = "http://192.168.100.7:5009/api/Vehiculoes";
            this.ActualizarVehiculo = "http://192.168.100.7:5009/api/Vehiculoes/{id}"; // URL Para actualizar productos
            this.EliminarVehiculo = "http://192.168.100.7:5009/api/Vehiculoes/{id"; // URL Para eliminar productos

            this.ListarMantenimiento = "http://192.168.100.7:5009/api/TareaMantenimientoes"; // URL para listar productos
            this.CrearMantenimiento = "http://192.168.100.7:5009/api/TareaMantenimientoes";
            this.ActualizarMantenimiento = "http://192.168.100.7:5009/api/TareaMantenimientoes/{id}"; // URL Para actualizar productos
            this.EliminarMantenimiento = "http://192.168.100.7:5009/api/TareaMantenimientoes/{id"; // URL Para eliminar productos
        }
    }
}
