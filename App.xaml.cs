using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GreentegCoreApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new GreentegCoreApp1.MainPage();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
            
        }

        protected override void OnSleep()


        {
          // MainPage.Navigation.PopModalAsync();
            /*if(mp != null)
            {
                mp.Device.GattDisconnect();
                
            }*/
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
           // MainPage.Navigation.PushModalAsync(new CoreView(mp.Device));
        }
    }
}

