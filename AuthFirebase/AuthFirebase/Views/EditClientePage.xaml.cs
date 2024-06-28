using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AuthFirebase.Models;

namespace AuthFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditClientePage : ContentPage
    {
        public event EventHandler ClienteEditado;
        private Clientes _cliente;

        public EditClientePage(Clientes cliente)
        {
            InitializeComponent();
            _cliente = cliente;
            BindingContext = _cliente;
        }

        // Método para guardar los cambios del cliente
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Invocar el evento para notificar que el cliente ha sido editado
            ClienteEditado?.Invoke(this, EventArgs.Empty);

            // Cerrar la página actual después de guardar
            await Navigation.PopAsync();
        }
    }
}
