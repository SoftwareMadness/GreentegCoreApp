using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tizen.Wearable.CircularUI.Forms;
using System.Globalization;
using Tizen.Network.Bluetooth;
using Tizen.Applications;
using Tizen.Multimedia;
using Xamarin.Forms.Platform.Tizen;
using Tizen.System;
using Xamarin.Forms.Platform.Tizen.Native;
using System.Threading;
using System.Diagnostics;

namespace GreentegCoreApp1
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : CirclePage
    {
        SharedStructBetweenPages s;
        public bool devmode = true;
        public Exception exc = null;
       public event EventHandler started_core;
        List<BluetoothLeDevice> asked_devices = new List<BluetoothLeDevice>();
        List<BluetoothLeDevice> devices = new List<BluetoothLeDevice>();
        bool lescan_started = false;
        List<Color> colors = new List<Color>();
        BluetoothGattClient clnt;
        int found_cores = 0;
        public BluetoothLeDevice Device { get; private set; }
        string Status { set { SCNLBL.Text = value; } }
        Task nononononono = null;
        Timer color_timer = null;
        bool music = false;
        int color = 0;

        #region ColorBackgroundAnimation
        void SwitchColors(object state)
        {
            if (color < Color.LightBlue.ToNative().B)
            {
                PAGE.BackgroundColor = Color.FromRgb(0, 0, color);
            }
            else
            {
                color = 0;
            }
        }

        private async void StartAnimation()
        {
   
            while (true)
            {
                for (int i = 0; i < 200; i++)
                {
                    PAGE.BackgroundColor = new Color(0, ((double)i) / 200, 0);
                    await Task.Delay(2000 / 200);
                }
                for (int i = 200; i >= 0; i--)
                {
                    PAGE.BackgroundColor = new Color(0, ((double)i)/200, 0);
                    await Task.Delay(2000 / 200);
                }
            }
        }
        #endregion
        #region Initialization
        public MainPage(SharedStructBetweenPages ss)
        {
            s = ss;

            InitializeComponent();
            if (music)
            {
                //color_timer = new Timer(new TimerCallback(SwitchColors), null, 0, 1000 / 30);
                nononononono = WavPlayer.StartAsync(ResourcePath.GetPath("music.wav"), new AudioStreamPolicy(AudioStreamType.Media));
            }
            //this.Appearing += MainPage_Appearing;

            if (Variables.debug_mode)
            {
                /*sdbg.Clicked += Sdbg_Clicked;
                SCAN.IsVisible = false;*/
                devices.Add(null);
                Core_list.Items.Add("Test Device");
            }
            else
            {
                sdbg.IsVisible = false;
                sdbg.IsEnabled = false;
                
            }

            StartAnimation();
        }

        #endregion
        #region Miscellaneous
        //Unused
        private void MainPage_Appearing(object sender, EventArgs e)
        {

        }

        //Unused
        void Init(){

            started_core(this, null);
        }
        //Unused
        private void BluetoothAdapter_StateChanged1(object sender, StateChangedEventArgs e)
        {
            Status = e.BTState.ToString();
            Status = e.Result.ToString();
            
        }
        //Debug mode Start Button
        private void Sdbg_Clicked(object sender, EventArgs e)
        {
            if (!Variables.debug_mode)
            {
                throw new NotImplementedException();
            }
            else
            {
                Navigation.PushModalAsync(new CoreView(null,s));
            }
        }   
        //Color fading animation
        async void GenerateColors()
        {
            for(int b = 0;b < Color.LightBlue.ToNative().B; b++)
            {
                colors.Add(Color.FromRgb(0, 0, b));
            }
            
        }
        //Only show loading animation
        void clean()
        {
            foreach(View itm in PAGE.Children)
            {
                itm.IsVisible = false;
            }
            PRG_BR.IsVisible = true;
            
        }
        #endregion
        #region CORE Selection
        private void Core_list_vdm_SelectedIndexChanged(object sender, EventArgs e)
        {
            clean();
            Thread.Sleep(5000);
            Navigation.PushModalAsync(new CoreView(null,s));
        }
        private void Core_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            clean();
            GC.Collect();
            if (!Variables.record_mode)
            {
                Core_list.SelectedIndexChanged -= Core_list_SelectedIndexChanged;
                BluetoothGattClient client = BluetoothGattClient.CreateClient((string)Core_list.SelectedItem);
                clnt = client;
                client.ConnectionStateChanged += Client_ConnectionStateChanged;
                client.ConnectAsync(true);
            }
            else
            {
                Navigation.PushModalAsync(new CoreView(null,s));
            }


        }
        #endregion
        private void Client_ConnectionStateChanged(object sender, GattConnectionStateChangedEventArgs e)
        {
            if (e.IsConnected)
            {
                Navigation.PushModalAsync(new CoreView(clnt,s));
                Core_list.SelectedItem = null;
                Core_list.SelectedIndexChanged += Core_list_SelectedIndexChanged;
            }
        }

        private void SBTN_Clicked(object sender, EventArgs e)
        {
        }


        private void BluetoothAdapter_StateChanged(object sender, StateChangedEventArgs e)
        {

        }

        async Task Asker(BluetoothLeDevice e)
        {
            if (e.Rssi < -71 || e.Rssi > 71)
            {
                asked_devices.Add(e);
                bool accepted = await DisplayAlert("Rssi: "+e.Rssi.ToString(), "Do you want to connect to " + e.GetDeviceName(BluetoothLePacketType.BluetoothLeAdvertisingPacket), "Yes", "No");
                if (accepted)
                {
                    //connect(devices.IndexOf(e));
                }
            }
        }

     

        private void Gc_ConnectionStateChanged(object sender, GattConnectionStateChangedEventArgs e)
        {
            bool deviceok = false;
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void SCAN_Clicked(object sender, EventArgs e)
        {
            if (!Variables.debug_mode) {
                SCAN.IsVisible = false;
                Core_list.IsVisible = true;
                GC.Collect();
                found_cores = 0;
                if (!Variables.record_mode)
                {
                    BluetoothAdapter.ScanResultChanged += BluetoothAdapter_ScanResultChanged;
                    
                    BluetoothAdapter.StartLeScan();

                    Device = null;
                    //SBTN.Clicked += SBTN_Clicked;
                    //ldbg.IsVisible = Variables.debug_mode;
                    Core_list.SelectedIndexChanged += Core_list_SelectedIndexChanged;

                }
                else
                {
                    Core_list.SelectedIndexChanged += Core_list_vdm_SelectedIndexChanged;
                    Core_list.Items.Add(Variables.record_mode_core_name);
                    sdbg.IsEnabled = false;
                    sdbg.IsVisible = false;
                    //this.ldbg.Text = "Record mode";
                }
            }
            else
            {
                SCAN.IsVisible = false;
                Core_list.SelectedIndexChanged += Core_list_vdm_SelectedIndexChanged;
                Core_list.IsVisible = true;
            }
        }

        private void BluetoothAdapter_ScanResultChanged(object sender, AdapterLeScanResultChangedEventArgs e)
        {
            if(!Core_list.Items.Contains(e.DeviceData.RemoteAddress)&&!string.IsNullOrEmpty(e.DeviceData.GetDeviceName(BluetoothLePacketType.BluetoothLeAdvertisingPacket)))
            {
                Core_list.Items.Add(e.DeviceData.RemoteAddress);
            }
            Core_list.FontSize = 10;
            Core_list.Title = Core_list.Items.Count + " Cores Found !";
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}