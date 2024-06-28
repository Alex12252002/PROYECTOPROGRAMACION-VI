using AuthFirebase.Models;
using AuthFirebase.Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class APIrestM : ContentPage
    {
        private ObservableCollection<TareaMantenimiento> _mantenimientos;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly API apis = new API();

        public Command<TareaMantenimiento> EditCommand { get; }
        public Command<TareaMantenimiento> DeleteCommand { get; }
        public Command CreateCommand { get; }

        public APIrestM()
        {
            InitializeComponent();
            _mantenimientos = new ObservableCollection<TareaMantenimiento>();
            MyListView.ItemsSource = _mantenimientos;

            EditCommand = new Command<TareaMantenimiento>(async (mantenimiento) => await EditMantenimiento(mantenimiento));
            DeleteCommand = new Command<TareaMantenimiento>(async (mantenimiento) => await DeleteMantenimiento(mantenimiento));
            CreateCommand = new Command(async () => await CreateMantenimiento());

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                string content = await _httpClient.GetStringAsync(apis.ListarMantenimiento);
                List<TareaMantenimiento> mantenimientos = JsonConvert.DeserializeObject<List<TareaMantenimiento>>(content);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _mantenimientos.Clear();
                    foreach (var mantenimiento in mantenimientos)
                    {
                        _mantenimientos.Add(mantenimiento);
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar los datos: {ex.Message}", "OK");
            }
        }

        private async Task EditMantenimiento(TareaMantenimiento mantenimiento)
        {
            var editPage = new EditMantenimientoPage(mantenimiento);
            editPage.MantenimientoEditado += async (s, e) =>
            {
                try
                {
                    await NegocioMantenimiento.UpdateMantenimientoAsync(mantenimiento);
                    await LoadData();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo actualizar el mantenimiento: {ex.Message}", "OK");
                }
            };
            await Navigation.PushAsync(editPage);
        }

        private async Task DeleteMantenimiento(TareaMantenimiento mantenimiento)
        {
            bool isConfirmed = await DisplayAlert("Confirmar", $"¿Estás seguro de que deseas eliminar al cliente {mantenimiento.id} {mantenimiento.tareas}?", "Sí", "No");
            if (isConfirmed)
            {
                try
                {
                    bool result = await NegocioMantenimiento.DeleteMantenimientoAsync(mantenimiento.id); // Utilizar Id como clave primaria
                    if (result)
                    {
                        Device.BeginInvokeOnMainThread(() => _mantenimientos.Remove(mantenimiento));
                        await DisplayAlert("Éxito", "Mantenimiento eliminado exitosamente", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar al Mantenimiento", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo eliminar al Mantenimiento: {ex.Message}", "OK");
                }
            }
        }

        private async Task CreateMantenimiento()
        {
            var newMantenimiento = new TareaMantenimiento();
            var editPage = new EditMantenimientoPage(newMantenimiento);

            editPage.MantenimientoEditado += async (s, e) =>
            {
                try
                {
                    await NegocioMantenimiento.CreateMantenimientoAsync(newMantenimiento);
                    await LoadData();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo crear el mantenimiento: {ex.Message}", "OK");
                }
            };

            await Navigation.PushAsync(editPage);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await CreateMantenimiento();
        }
    }
}
