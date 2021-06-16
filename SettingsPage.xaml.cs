using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.Applications;
using Tizen.Network.Bluetooth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GreentegCoreApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        BluetoothGattClient dev = null;
        Settings_t settings;
        public PcLink link = null;
        CoreView c;
        public SettingsPage(BluetoothGattClient devi,ref Settings_t set,CoreView cv)
        {
            InitializeComponent();
            c = cv;
            settings = set;
            if(!Preference.Contains("IsFarenheit"))
            {
                Preference.Set("IsFarenheit", false);
            }
            CORF.IsToggled= Preference.Get<bool>("IsFarenheit");
            CORF.Toggled += CORF_Toggled;

            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void CORF_Toggled(object sender, ToggledEventArgs e)
        {
            settings.f = CORF.IsToggled;
            Preference.Set("IsFarenheit", settings.f);
            c.ComputeTemp(c.characteristic.Value);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (dev != null) dev.DisconnectAsync().Wait();GC.Collect();
            Xamarin.Forms.Application.Current.Quit();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            if(link == null)
            {
                link = new PcLink();
            }
            Navigation.PushModalAsync(link);
        }
    }
}