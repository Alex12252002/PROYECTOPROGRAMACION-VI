using AuthFirebase.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AuthFirebase.Negocio
{
    public class NegocioMantenimiento
    {
        static HttpClient client = new HttpClient();
        static API apis = new API();

        // Método para crear una tarea de mantenimiento
        public static async Task<Uri> CreateMantenimientoAsync(TareaMantenimiento tarea)
        {
            try
            {
                // Validación rápida antes de enviar la solicitud
                if (tarea == null)
                {
                    throw new ArgumentNullException(nameof(tarea), "El objeto de tarea de mantenimiento no puede ser nulo.");
                }

                // Validación de los campos obligatorios
                if (string.IsNullOrWhiteSpace(tarea.tareas) ||
                    tarea.Kilometraje <= 0 ||
                    string.IsNullOrWhiteSpace(tarea.Repuesto))
                {
                    throw new ArgumentException("Todos los campos de la tarea de mantenimiento son requeridos.");
                }

                HttpResponseMessage response = await client.PostAsJsonAsync(apis.CrearMantenimiento, tarea);
                response.EnsureSuccessStatusCode();
                return response.Headers.Location;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Error al crear tarea de mantenimiento: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Error al crear tarea de mantenimiento: {ex.Message}", ex);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al crear tarea de mantenimiento: {ex.Message}");
                throw new Exception($"Error HTTP al crear tarea de mantenimiento: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al crear tarea de mantenimiento: {ex.Message}");
                throw new Exception($"Error general al crear tarea de mantenimiento: {ex.Message}", ex);
            }
        }

        // Método para obtener una tarea de mantenimiento por su Id (clave primaria)
        public static async Task<TareaMantenimiento> GetMantenimientoAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{apis.ListarMantenimiento}/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<TareaMantenimiento>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al obtener tarea de mantenimiento: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al obtener tarea de mantenimiento: {ex.Message}");
                throw;
            }
        }

        // Método para actualizar una tarea de mantenimiento
        public static async Task<TareaMantenimiento> UpdateMantenimientoAsync(TareaMantenimiento tarea)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync($"{apis.ActualizarMantenimiento.Replace("{id}", tarea.id.ToString())}", tarea);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                if (IsJson(responseBody))
                {
                    return await response.Content.ReadFromJsonAsync<TareaMantenimiento>();
                }
                else
                {
                    Console.WriteLine("La respuesta no es un JSON válido.");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al actualizar tarea de mantenimiento: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al actualizar tarea de mantenimiento: {ex.Message}");
                throw;
            }
        }

        // Método para eliminar una tarea de mantenimiento por su Id (clave primaria)
        public static async Task<bool> DeleteMantenimientoAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{apis.EliminarMantenimiento.Replace("{id}", id.ToString())}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al eliminar mantenimiento: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al eliminar mantenimiento: {ex.Message}");
                throw;
            }
        }

        // Método para listar todas las tareas de mantenimiento
        public static async Task<List<TareaMantenimiento>> ListarMantenimientosAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apis.ListarMantenimiento);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<TareaMantenimiento>>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP al listar tareas de mantenimiento: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al listar tareas de mantenimiento: {ex.Message}");
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
