using AuthFirebase.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AuthFirebase.Negocio
{
    public class NegocioVehiculo
    {
        static HttpClient client = new HttpClient();
        static API apis = new API();

        // Método para crear un vehículo
        public static async Task<Uri> CreateVehiculoAsync(Vehiculo vehiculo)
        {
            try
            {
                // Validación rápida antes de enviar la solicitud
                if (vehiculo == null)
                {
                    throw new ArgumentNullException(nameof(vehiculo), "El objeto vehículo no puede ser nulo.");
                }

                // Validación de los campos obligatorios
                if (string.IsNullOrWhiteSpace(vehiculo.Placa) ||
                    string.IsNullOrWhiteSpace(vehiculo.Marca) ||
                    string.IsNullOrWhiteSpace(vehiculo.Modelo) ||
                    vehiculo.Anio == 0)
                {
                    throw new ArgumentException("Todos los campos del vehículo son requeridos.");
                }

                HttpResponseMessage response = await client.PostAsJsonAsync(apis.CrearVehiculo, vehiculo);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Error al crear vehículo: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Error al crear vehículo: {ex.Message}", ex);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al crear vehículo: {ex.Message}");
                throw new Exception($"Error HTTP al crear vehículo: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al crear vehículo: {ex.Message}");
                throw new Exception($"Error general al crear vehículo: {ex.Message}", ex);
            }
        }

        // Método para obtener un vehículo por su Id (clave primaria)
        public static async Task<Vehiculo> GetVehiculoAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{apis.ListarVehiculo}/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Vehiculo>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al obtener vehículo: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al obtener vehículo: {ex.Message}");
                throw;
            }
        }

        // Método para actualizar un vehículo
        public static async Task<Vehiculo> UpdateVehiculoAsync(Vehiculo vehiculo)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync($"{apis.ActualizarVehiculo.Replace("{id}", vehiculo.Id.ToString())}", vehiculo);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                if (IsJson(responseBody))
                {
                    return await response.Content.ReadFromJsonAsync<Vehiculo>();
                }
                else
                {
                    Console.WriteLine("La respuesta no es un JSON válido.");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al actualizar vehículo: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al actualizar vehículo: {ex.Message}");
                throw;
            }
        }

        // Método para eliminar un vehículo por su Id (clave primaria)
        public static async Task<bool> DeleteVehiculoAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{apis.EliminarVehiculo.Replace("{id}", id.ToString())}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al eliminar vehículo: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al eliminar vehículo: {ex.Message}");
                throw; // Re-lanza la excepción para un manejo superior si es necesario
            }
        }


        // Función auxiliar para verificar si una cadena es JSON válido
        private static bool IsJson(string input)
        {
            input = input.Trim();
            return (input.StartsWith("{") && input.EndsWith("}")) || (input.StartsWith("[") && input.EndsWith("]"));
        }
    }
}
