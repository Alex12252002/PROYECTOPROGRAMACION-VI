using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioPage : ContentPage
    {
        public string WebAPIKey = "\r\nAIzaSyBY5idtAP-cLygfnsJyTTYWYjSoyEn80v8";
        public InicioPage()
        {
            InitializeComponent();
            GetProfileInformationAndRefreshToken();
        }

        private async void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));

            try
            {
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);

                MyUsername.Text = savedfirebaseauth.User.Email;

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alerta", "El token no es valido ", "Ok");
            }
        }

        private async void btnLogout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());

        }

        private async void btnClientes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new APIrest());
        }

        private async void btnVehiculos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new APIrestV());

        }

        private async void  btnMantenimientos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new APIrestM());

        }
    }
}