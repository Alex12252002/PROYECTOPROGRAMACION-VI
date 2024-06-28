using AuthFirebase.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateClientePage : ContentPage
    {
        public CreateClientePage()
        {
            InitializeComponent();
            BindingContext = new Clientes(); // Establecer un nuevo cliente como contexto de enlace
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Clientes nuevoCliente = (Clientes)BindingContext;

            try
            {
                // Llamar al método estático de NegocioCliente para crear el cliente
                var uri = await Negocio.NegocioCliente.CreateClienteAsync(nuevoCliente);

                // Mostrar mensaje de creación de cliente exitoso
                await DisplayAlert("Éxito", "Cliente añadido correctamente", "Aceptar");

                // Limpiar los campos después de añadir el cliente
                nuevoCliente.Cedula = "";
                nuevoCliente.Nombres = "";
                nuevoCliente.Apellidos = "";
                nuevoCliente.Telefono = "";
                nuevoCliente.Ubicacion = new Ubicacion(); // Limpiar también la ubicación si es necesario
            }
            catch (Exception ex)
            {
                // Manejo de cualquier error durante la creación del cliente
                await DisplayAlert("Error", $"Error al añadir cliente: {ex.Message}", "Aceptar");
            }
        }
    }
}
