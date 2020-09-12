using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.Network.Bluetooth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GreentegCoreApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        BluetoothLeDevice dev = null;
        Settings_t settings;
        public SettingsPage(BluetoothLeDevice devi,ref Settings_t set)
        {
            InitializeComponent();
            settings = set;
            CORF.IsToggled = set.f;
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void CORF_Toggled(object sender, ToggledEventArgs e)
        {
            settings.f = !settings.f;
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (dev != null) dev.GattDisconnect();GC.SuppressFinalize(dev);GC.Collect();
            Application.Current.Quit();
        }
    }
}