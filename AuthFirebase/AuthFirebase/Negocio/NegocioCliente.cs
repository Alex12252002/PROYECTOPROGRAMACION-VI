using AuthFirebase.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AuthFirebase.Negocio
{
    public class NegocioCliente
    {
        static HttpClient client = new HttpClient();
        static API apis = new API();

        // Método para crear un cliente
        public static async Task<Uri> CreateClienteAsync(Clientes cliente)
        {
            try
            {
                // Validación rápida antes de enviar la solicitud
                if (cliente == null)
                {
                    throw new ArgumentNullException(nameof(cliente), "El objeto cliente no puede ser nulo.");
                }

                // Validación de los campos obligatorios
                if (string.IsNullOrWhiteSpace(cliente.Cedula) ||
                    string.IsNullOrWhiteSpace(cliente.Nombres) ||
                    string.IsNullOrWhiteSpace(cliente.Apellidos) ||
                    string.IsNullOrWhiteSpace(cliente.Telefono) ||
                    cliente.Ubicacion == null)
                {
                    throw new ArgumentException("Todos los campos del cliente son requeridos.");
                }

                HttpResponseMessage response = await client.PostAsJsonAsync(apis.CrearCliente, cliente);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Error al crear cliente: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Error al crear cliente: {ex.Message}", ex);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al crear cliente: {ex.Message}");
                throw new Exception($"Error HTTP al crear cliente: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al crear cliente: {ex.Message}");
                throw new Exception($"Error general al crear cliente: {ex.Message}", ex);
            }
        }

        // Método para obtener un cliente por su Id (clave primaria)
        public static async Task<Clientes> GetClienteAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{apis.ListarCliente}/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Clientes>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al obtener cliente: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al obtener cliente: {ex.Message}");
                throw;
            }
        }

        // Método para actualizar un cliente
        public static async Task<Clientes> UpdateClienteAsync(Clientes cliente)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync($"{apis.ActualizarCliente.Replace("{id}", cliente.Id.ToString())}", cliente);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                if (IsJson(responseBody))
                {
                    return await response.Content.ReadFromJsonAsync<Clientes>();
                }
                else
                {
                    Console.WriteLine("La respuesta no es un JSON válido.");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al actualizar cliente: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al actualizar cliente: {ex.Message}");
                throw;
            }
        }

        // Método para eliminar un cliente por su Id (clave primaria)
        public static async Task<bool> DeleteClienteAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{apis.EliminarCliente.Replace("{id}", id.ToString())}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al eliminar cliente: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al eliminar cliente: {ex.Message}");
                throw;
            }
        }

        // Método para listar todos los clientes
        public static async Task<List<Clientes>> ListarClientesAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apis.ListarCliente);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Clientes>>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al listar clientes: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al listar clientes: {ex.Message}");
                throw;
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
