using AuthFirebase.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMantenimientoPage : ContentPage
    {
        public event EventHandler MantenimientoEditado;
        private TareaMantenimiento _mantenimiento;

        public EditMantenimientoPage(TareaMantenimiento mantenimiento)
        {
            InitializeComponent();
            _mantenimiento = mantenimiento;
            BindingContext = _mantenimiento;
        }

        // Método para guardar los cambios del mantenimiento
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Invocar el evento para notificar que el mantenimiento ha sido editado
            MantenimientoEditado?.Invoke(this, EventArgs.Empty);

            // Cerrar la página actual después de guardar
            await Navigation.PopAsync();
        }
    }
}
