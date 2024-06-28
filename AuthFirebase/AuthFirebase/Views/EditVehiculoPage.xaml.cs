using AuthFirebase.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditVehiculoPage : ContentPage
    {
        public event EventHandler VehiculoEditado;
        private Vehiculo _vehiculo;

        public EditVehiculoPage(Vehiculo vehiculo)
        {
            InitializeComponent();
            _vehiculo = vehiculo;
            BindingContext = _vehiculo;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Invocar el evento para notificar que el vehículo ha sido editado
            VehiculoEditado?.Invoke(this, EventArgs.Empty);

            // Cerrar la página actual después de guardar
            await Navigation.PopAsync();
        }
    }
}
