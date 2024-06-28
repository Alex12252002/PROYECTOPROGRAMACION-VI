using AuthFirebase.Models;
using AuthFirebase.Negocio;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateVehiculoPage : ContentPage
    {
        public CreateVehiculoPage()
        {
            InitializeComponent();
            BindingContext = new Vehiculo(); // Instancia un nuevo vehículo para el contexto de datos
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Obtener el vehículo desde el contexto de enlace
            Vehiculo nuevoVehiculo = (Vehiculo)BindingContext;

            try
            {
                // Llamar al método estático de NegocioVehiculo para crear el vehículo
                var uri = await NegocioVehiculo.CreateVehiculoAsync(nuevoVehiculo);

                // Mostrar mensaje de creación de vehículo exitoso
                await DisplayAlert("Éxito", "Vehículo añadido correctamente", "Aceptar");

                // Limpiar los campos después de añadir el vehículo (opcional)
                nuevoVehiculo.Placa = "";
                nuevoVehiculo.Marca = "";
                nuevoVehiculo.Modelo = "";
                nuevoVehiculo.Anio = 0;

            }
            catch (Exception ex)
            {
                // Manejo de cualquier error durante la creación del vehículo
                await DisplayAlert("Error", $"Error al añadir vehículo: {ex.Message}", "Aceptar");
            }
        }
    }
}
