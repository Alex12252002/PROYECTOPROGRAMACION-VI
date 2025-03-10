﻿using AuthFirebase.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthFirebase
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
