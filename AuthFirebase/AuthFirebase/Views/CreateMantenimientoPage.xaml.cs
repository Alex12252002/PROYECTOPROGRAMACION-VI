using AuthFirebase.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateMantenimientoPage : ContentPage
    {
        public CreateMantenimientoPage()
        {
            InitializeComponent();
            BindingContext = new TareaMantenimiento(); // Establecer una nueva tarea de mantenimiento como contexto de enlace
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            TareaMantenimiento nuevaTarea = (TareaMantenimiento)BindingContext;

            try
            {
                // Llamar al método estático de NegocioMantenimiento para crear la tarea de mantenimiento
                var uri = await Negocio.NegocioMantenimiento.CreateMantenimientoAsync(nuevaTarea);

                // Mostrar mensaje de creación de la tarea de mantenimiento exitoso
                await DisplayAlert("Éxito", "Tarea de mantenimiento añadida correctamente", "Aceptar");

                // Limpiar los campos después de añadir la tarea de mantenimiento
                nuevaTarea.tareas = "";
                nuevaTarea.Kilometraje = 0;
                nuevaTarea.Repuesto = "";
                nuevaTarea.Observacion = "";
            }
            catch (Exception ex)
            {
                // Manejo de cualquier error durante la creación de la tarea de mantenimiento
                await DisplayAlert("Error", $"Error al añadir tarea de mantenimiento: {ex.Message}", "Aceptar");
            }
        }
    }
}
