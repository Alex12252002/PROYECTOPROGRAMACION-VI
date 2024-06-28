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
    public partial class APIrest : ContentPage
    {
        private ObservableCollection<Clientes> _clientes;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly API apis = new API();

        public Command<Clientes> EditCommand { get; }
        public Command<Clientes> DeleteCommand { get; }
        public Command CreateCommand { get; }

        public APIrest()
        {
            InitializeComponent();
            _clientes = new ObservableCollection<Clientes>();
            MyListView.ItemsSource = _clientes;

            EditCommand = new Command<Clientes>(async (cliente) => await EditCliente(cliente));
            DeleteCommand = new Command<Clientes>(async (cliente) => await DeleteCliente(cliente));
            CreateCommand = new Command(async () => await CreateCliente());

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
                string content = await _httpClient.GetStringAsync(apis.ListarCliente);
                List<Clientes> clientes = JsonConvert.DeserializeObject<List<Clientes>>(content);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _clientes.Clear();
                    foreach (var cliente in clientes)
                    {
                        _clientes.Add(cliente);
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar los datos: {ex.Message}", "OK");
            }
        }

        private async Task EditCliente(Clientes cliente)
        {
            var editPage = new EditClientePage(cliente);
            editPage.ClienteEditado += async (s, e) =>
            {
                try
                {
                    await NegocioCliente.UpdateClienteAsync(cliente);
                    await LoadData();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo actualizar al cliente: {ex.Message}", "OK");
                }
            };
            await Navigation.PushAsync(editPage);
        }

        private async Task DeleteCliente(Clientes cliente)
        {
            bool isConfirmed = await DisplayAlert("Confirmar", $"¿Estás seguro de que deseas eliminar al cliente {cliente.Nombres} {cliente.Apellidos}?", "Sí", "No");
            if (isConfirmed)
            {
                try
                {
                    bool result = await NegocioCliente.DeleteClienteAsync(cliente.Id); // Utilizar Id como clave primaria
                    if (result)
                    {
                        Device.BeginInvokeOnMainThread(() => _clientes.Remove(cliente));
                        await DisplayAlert("Éxito", "Cliente eliminado exitosamente", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar al cliente", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo eliminar al cliente: {ex.Message}", "OK");
                }
            }
        }

        private async Task CreateCliente()
        {
            var newCliente = new Clientes();
            var editPage = new EditClientePage(newCliente);

            editPage.ClienteEditado += async (s, e) =>
            {
                try
                {
                    await NegocioCliente.CreateClienteAsync(newCliente);
                    await LoadData();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo crear al cliente: {ex.Message}", "OK");
                }
            };

            await Navigation.PushAsync(editPage);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await CreateCliente();
        }
    }
}
