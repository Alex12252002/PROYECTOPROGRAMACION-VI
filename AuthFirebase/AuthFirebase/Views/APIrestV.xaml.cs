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
    public partial class APIrestV : ContentPage
    {
        private ObservableCollection<Vehiculo> _vehiculos;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly API apis = new API();

        public Command<Vehiculo> EditCommand { get; }
        public Command<Vehiculo> DeleteCommand { get; }
        public Command CreateCommand { get; }

        public APIrestV()
        {
            InitializeComponent();
            _vehiculos = new ObservableCollection<Vehiculo>();
            MyListView.ItemsSource = _vehiculos;

            EditCommand = new Command<Vehiculo>(async (vehiculo) => await EditVehiculo(vehiculo));
            DeleteCommand = new Command<Vehiculo>(async (vehiculo) => await DeleteVehiculo(vehiculo));
            CreateCommand = new Command(async () => await CreateVehiculo());

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
                string content = await _httpClient.GetStringAsync(apis.ListarVehiculo);
                List<Vehiculo> vehiculos = JsonConvert.DeserializeObject<List<Vehiculo>>(content);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _vehiculos.Clear();
                    foreach (var vehiculo in vehiculos)
                    {
                        _vehiculos.Add(vehiculo);
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar los datos: {ex.Message}", "OK");
            }
        }

        private async Task EditVehiculo(Vehiculo vehiculo)
        {
            var editPage = new EditVehiculoPage(vehiculo);
            editPage.VehiculoEditado += async (s, e) =>
            {
                try
                {
                    await NegocioVehiculo.UpdateVehiculoAsync(vehiculo); // Utilizar el objeto vehiculo actualizado
                    await LoadData();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo actualizar el vehículo: {ex.Message}", "OK");
                }
            };
            await Navigation.PushAsync(editPage);
        }

        private async Task DeleteVehiculo(Vehiculo vehiculo)
        {
            bool isConfirmed = await DisplayAlert("Confirmar", $"¿Estás seguro de que deseas eliminar al cliente {vehiculo.Id} {vehiculo.Placa}?", "Sí", "No");
            if (isConfirmed)
            {
                try
                {
                    bool result = await NegocioCliente.DeleteClienteAsync(vehiculo.Id); // Utilizar Id como clave primaria
                    if (result)
                    {
                        Device.BeginInvokeOnMainThread(() => _vehiculos.Remove(vehiculo));
                        await DisplayAlert("Éxito", "Vehiculo eliminado exitosamente", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar al vehiculo", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo eliminar al vehiculo: {ex.Message}", "OK");
                }
            }
        }
        private async Task CreateVehiculo()
        {
            var newVehiculo = new Vehiculo();
            var editPage = new EditVehiculoPage(newVehiculo);

            editPage.VehiculoEditado += async (s, e) =>
            {
                try
                {
                    await NegocioVehiculo.CreateVehiculoAsync(newVehiculo);
                    await LoadData();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo crear el vehículo: {ex.Message}", "OK");
                }
            };

            await Navigation.PushAsync(editPage);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await CreateVehiculo();
        }
    }
}
