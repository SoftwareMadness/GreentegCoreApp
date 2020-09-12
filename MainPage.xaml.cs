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

namespace GreentegCoreApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : CirclePage
    {
        public Exception exc = null;
       public event EventHandler started_core;
        List<BluetoothLeDevice> asked_devices = new List<BluetoothLeDevice>();
        List<BluetoothLeDevice> devices = new List<BluetoothLeDevice>();
        bool lescan_started = false;
        List<Color> colors = new List<Color>();
        int found_cores = 0;
        public BluetoothLeDevice Device { get; private set; }
        string Status { set { SCNLBL.Text = value; } }
        Task nononononono = null;
        Timer color_timer = null;
        int color = 0;
        
        void SwitchColors(object state)
        {
            if (color < Color.LightBlue.ToNative().B)
            {
                PAGE.BackgroundColor = Color.FromRgb(0,0,color);
            }
            else
            {
                color = 0;
            }
        }

        public MainPage()
        {

            InitializeComponent();
            //color_timer = new Timer(new TimerCallback(SwitchColors), null, 0, 1000 / 30);
            // GenerateColors();
            //nononononono = WavPlayer.StartAsync(ResourcePath.GetPath("music.wav"), new AudioStreamPolicy(AudioStreamType.Media));
            //this.Appearing += MainPage_Appearing;

            if (Variables.debug_mode)
            {
                sdbg.Clicked += Sdbg_Clicked;
            }
            else
            {
                sdbg.IsVisible = false;
                sdbg.IsEnabled = false;
            } 


        }
        

        private void MainPage_Appearing(object sender, EventArgs e)
        {

        }

        void Init(){

            started_core(this, null);
        }

        private void BluetoothAdapter_StateChanged1(object sender, StateChangedEventArgs e)
        {
            Status = e.BTState.ToString();
            Status = e.Result.ToString();
            
        }

        private void Sdbg_Clicked(object sender, EventArgs e)
        {
            if (!Variables.debug_mode)
            {
                throw new NotImplementedException();
            }
            else
            {
                Navigation.PushModalAsync(new CoreView(null));
            }
        }
        void connect(int connected_index)
        {
           
            if (Core_list.SelectedItem != null)
            {
                BluetoothLeDevice core = devices[connected_index];
                try
                {
                    try
                    {
                        BluetoothAdapter.StopLeScan();
                    }
                    catch { }
                    Device = core;
                    Navigation.PushModalAsync(new CoreView(core));
                    if (nononononono != null) nononononono.Dispose();
                }
                catch (Exception ex)
                {
                    exc = ex;
                    DisplayAlert("Error", ex.ToString(), "OK");
                }
            }
        }
        async void GenerateColors()
        {
            for(int b = 0;b < Color.LightBlue.ToNative().B; b++)
            {
                colors.Add(Color.FromRgb(0, 0, b));
            }
            
        }
        private void Core_list_vdm_SelectedIndexChanged(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CoreView(null));
        }
        private void Core_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            GC.Collect();
            if (!Variables.record_mode)
            {
                Core_list.SelectedIndexChanged -= Core_list_SelectedIndexChanged;
                connect(Core_list.SelectedIndex);
                Core_list.SelectedItem = null;
                Core_list.SelectedIndexChanged += Core_list_SelectedIndexChanged;
            }
            else
            {
                Navigation.PushModalAsync(new CoreView(null));
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
                bool accepted = await DisplayAlert("Rssi: "+e.Rssi.ToString(), "Do you want to connect to " + e.DeviceName, "Yes", "No");
                if (accepted)
                {
                    connect(devices.IndexOf(e));
                }
            }
        }

        private void BluetoothAdapter_ScanResultChanged(object sender, AdapterLeScanResultChangedEventArgs e)
        {
            GC.Collect();
            if (e.Result == BluetoothError.None)
            {


                if (e.DeviceData != null)
                {
                    if (!string.IsNullOrWhiteSpace(e.DeviceData.DeviceName))
                    {
                        if (e.DeviceData.DeviceName.ToLower().TrimStart(' ').StartsWith("core"))
                        {
                            Status = e.DeviceData.Rssi.ToString();
                            bool deviceok = false;
                            try
                            {

                                foreach (BluetoothGattService serv in e.DeviceData.GattConnect(false).GetServices())
                                {
                                    foreach (BluetoothGattCharacteristic chara in serv.GetCharacteristics())
                                    {
                                      
                                        if (chara.Uuid.ToLower().StartsWith(Variables.characteristicuuid_start))
                                        {
                                            deviceok = true;
                                           
                                        }
                                        

                                    }
                                }
                            }
                            catch { }
                            if (!devices.Contains(e.DeviceData))
                            {
                                
                                Core_list.Items.Add(e.DeviceData.DeviceName.ToLower()+"  "+ (deviceok ? "OK" : "*"));
                                found_cores++;
                                Core_list.Title = "found " + found_cores + "cores";
                                devices.Add(e.DeviceData);
                                
                            }
                            if (!asked_devices.Contains(e.DeviceData))
                            {

                                Asker(e.DeviceData).Wait();

                            }
                            else
                            {

                            }

                        }

                    }
                }
            }

            async Task WaitTillScan()
            {
                int seconds = 30;
                while (true)
                {
                    if (seconds == 0)
                    {
                        break;
                    }
                    else
                    {
                        seconds--;
                        Status = "Scanning " + seconds.ToString() + "s";
                    }
                    await Task.Delay(1000);
                }
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void SCAN_Clicked(object sender, EventArgs e)
        {
            SCAN.IsVisible = false;
            Core_list.IsVisible = true;
            GC.Collect();
            found_cores = 0;
            if (!Variables.record_mode)
            {
                Status = "Scanning";
                BluetoothAdapter.ScanResultChanged += BluetoothAdapter_ScanResultChanged;
                BluetoothAdapter.StartLeScan();
                //Status = "Started";
                BluetoothAdapter.StateChanged += BluetoothAdapter_StateChanged1;

                Device = null;
                //SBTN.Clicked += SBTN_Clicked;
                ldbg.IsVisible = Variables.debug_mode;
                Core_list.SelectedIndexChanged += Core_list_SelectedIndexChanged;

            }
            else
            {
                Core_list.SelectedIndexChanged += Core_list_vdm_SelectedIndexChanged;
                Core_list.Items.Add(Variables.record_mode_core_name);
                sdbg.IsEnabled = false;
                sdbg.IsVisible = false;
                this.ldbg.Text = "Record mode";
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}