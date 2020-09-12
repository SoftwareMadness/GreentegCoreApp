using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GreentegCoreApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LauncherPage : ContentPage
    {
        public LauncherPage()
        {
            InitializeComponent();
            MainPage page = new MainPage();
            try
            {
                SettingsManager.Default();
            }
            catch(Exception ex) { DisplayAlert("SystemLauncherError", ex.ToString(), "OK"); }
            Navigation.PushModalAsync(page,true);

        }
    }
}