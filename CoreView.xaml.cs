using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Tizen;
using Tizen.Applications;
using Tizen.Multimedia;
using Tizen.Network.Bluetooth;
using Tizen.Network.WiFi;
using Tizen.System;
using Tizen.Uix.Tts;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Xaml;
using LibBLEHTP;

namespace GreentegCoreApp1
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoreView : ContentPage
    {
        SharedStructBetweenPages s;
        Settings_t set = new Settings_t();
        SettingsPage stxpg = null;
        bool started = false;
        double offset = 0.08661417322834646;
        BluetoothGattClient client = null;
        Vibrator vibr = null;
        bool sport_enabled = false;
        public BluetoothGattCharacteristic characteristic = null;
        BluetoothGattCharacteristic battery_chara = null;
        
        int servicesl = 0;
        bool farenheit = false;
        List<BluetoothGattService> services = new List<BluetoothGattService>();
        Timer tmr = null;
        async void BlinkAct()
        {
            DebugBlinkerLED.BackgroundColor = Color.Green;
            Task.Delay(1000);
            DebugBlinkerLED.BackgroundColor = Color.Red;
        }
        void TMR_INTV(object sender,EventArgs e)
        {
            tmr.Interval = 1000;
            tmr.Start();
            this.TimeLBL.Text = DateTime.Now.ToString("HH:mm");
        }
        async void WaiterWatcher()
        {
            int time = 30;
            while (!started)
            {
                Task.Delay(1);
                time--;
                if(time < 0)
                {
                        if
                        (!started) {
                        WaiterReaderConnecter();
                        break;
                    }
                }
            }
        }
       
        

        public CoreView(BluetoothGattClient device, SharedStructBetweenPages ss)
        {
            s = ss;
            s.device = device;
          
            try
            {

                InitializeComponent();
                tmr = new Timer();
                tmr.Interval = 1000;
                tmr.Enabled = true;
                tmr.AutoReset = true;
                
                tmr.Elapsed += TMR_INTV;
                tmr.Start();
                Sport_modebtn.IsVisible = true;//!Variables.debug_mode;
                gtbtn.IsVisible = false;
                //gtbtn.IsVisible = Variables.debug_mode;
                if (Variables.ServerMode)
                {
                    byte[] data = { 50 };
                    WebRequest request = WebRequest.Create(Variables.URL);
                    request.Method = "POST";
                    request.GetRequestStream().Write(data, 0, data.Length);
                    request.GetRequestStream().Close();
                    GC.SuppressFinalize(request);
                    GC.Collect();
                }

                if (device != null)
                {
                    //4WaiterX60(   ;
                    //LookWhatYouMadeMeDo(device);
                    client = device;

                    services.Add(client.GetService(Variables.serviceuuid_start));
                    //services.Add(client.GetService(Variables.batteryserviceuuid));
                    characteristic = client.GetService(Variables.serviceuuid_start).GetCharacteristic(Variables.characteristicuuid_start);
                    //battery_chara = services[1].GetCharacteristic(Variables.batterycharacteristicuuid);
                    //battery_chara.GetValue(0);
                    //throw new Exception(Bytepacker.pack(battery_chara.Value));
                    characteristic.ValueChanged += Characteristic_ValueChanged;
                    this.DataLBL.Text = "Loading...";
                    ComputeTemp(characteristic.Value);
                    //tmr.Start();
                }
                if (Variables.debug_mode) { 
                    Timer tmr = new Timer(3000);
                    tmr.AutoReset = true;
                    tmr.Enabled = true;
                    tmr.Elapsed += Tmr_Elapsed;
                    tmr.Start();
                    //DisplayAlert("Info", "Running in debug mode", "OK");
                }
            }
            catch(Exception ecx)
            {
                DisplayAlert("Error in Main Loop", ecx.Message, "OK");
            }
            if(device == null)
            {
                ComputeTemp(true);
            }
        }
        async void rec_mode()
        {
            await WaitFlag();
            
        }
        private void Tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            ComputeTemp(true);
        }

        private void Sport_modebtn_Clicked(object sender, EventArgs e)
        {
            
        }

        private void Connect_Clicked(object sender, EventArgs e)
        {
            GC.Collect();
            Reader();
        }
        void Panic()
        {
            VIBRATEMOTOR();
            WavPlayer.StartAsync(ResourcePath.GetPath("your_temperature_is_high.wav"), new AudioStreamPolicy(AudioStreamType.Alarm));

        }

        private void Client_StateChanged(object sender, Tizen.Uix.Tts.StateChangedEventArgs e)
        {
            if(e.Current == State.Ready)
            {
                
            }
        }

        async void VIBRATEMOTOR()
        {
            if (vibr != null) vibr.Vibrate(1000, 100);
        }
        async void WaiterReaderConnecter()
        {
            await WaitFlag();
            //WaiterWatcher();
            try
            {
                foreach (BluetoothGattService serv in client.GetServices())
            {
                /*if(serv.Uuid != 0x1809)
                {

                }*/
                services.Add(serv);
                if (/*serv.Uuid.StartsWith(Variables.serviceuuid_start)*/true)
                {
                    foreach (BluetoothGattCharacteristic chara in serv.GetCharacteristics())
                    {
                            Tizen.Log.Info("COREUUID", "COREUUID=" + chara.Uuid);
                        if (chara.Uuid.ToLower().StartsWith(Variables.characteristicuuid_start))
                        {
                                DataLBL.FontSize = 10;
                            DataLBL.Text = "Waiting for response";
                            characteristic = chara;
                            characteristic.ValueChanged += Characteristic_ValueChanged;


                                //DisplayAlert("OK", "OK", "OK");
                        }
                            GC.Collect();
                   
                    }
                }
                
                
               // DataLBL.Text = serv.Uuid;
            }
           
               
            }
            catch {if(characteristic != null) DisplayAlert("Error", "Error", "Error"); else DisplayAlert("Error", "Is Null ?", "Retry");WaiterReaderConnecter(); }
            //DataLBL.Text = "Done";
        }
        async void KeeperUpper(BluetoothGattService srv)
        {
            while (true)
            {
                     }
        }
        async void WaiterX60()
        {
            await Task.Delay(20000);
            try
            {
                if (!started)
                {
                    //client.DisconnectAsync().Wait();
                    //LookWhatYouMadeMeDo(device);
                }
            }
            catch(Exception ex) { DisplayAlert("Error", ex.ToString(),"OK"); }
        }
        private void Characteristic_ValueChanged(object sender, Tizen.Network.Bluetooth.ValueChangedEventArgs e)
        {
            GC.Collect();
            DataLBL.FontSize = 40;
            BlinkAct();
            started = true;
            //filesystemX.Write_Temp(e.Value[1]);
            Tizen.Log.Info("CORE", "Core_Val=" + Bytepacker.pack(e.Value));
            //DisplayAlert("OKKKKKK", "OKKKKKKKKKK", "OK");
            ComputeTemp(e.Value);
            if (Variables.ServerMode)
            {
                byte[] data = { e.Value[1] };
                WebRequest request = WebRequest.Create(Variables.URL);
                request.Method = "POST";
                request.GetRequestStream().Write(data, 0, data.Length);
                request.GetRequestStream().Close();
                GC.SuppressFinalize(request);
                GC.Collect();
            }
        }
        public void ComputeTemp(byte[] temp)
        {

            this.DataLBL.FontSize = 20;
            float temp_calc = HTPDecoder.decode(new byte[] { temp[4],temp[3],temp[2],temp[1]});
            if (stxpg != null)
            {
                if(stxpg.link != null)
                {
                    stxpg.link.UpdateTemp(temp_calc);
                }
            }
            //handler.WriteTemp((double)temp_calc);
            float disptemp = temp_calc;
            if (Preference.Get<bool>("IsFarenheit"))
            {
                disptemp = ((temp_calc*9)/5)+32;
            }
            this.DataLBL.Text = disptemp.ToString() + (Preference.Get<bool>("IsFarenheit") ? "F°" : "C°");
            if (!sport_enabled && temp_calc > 38)
            {
                this.DataLBL.TextColor = Color.Red;
                Panic();
            }
            else if (sport_enabled && temp_calc > 40)
            {
                this.DataLBL.TextColor = Color.Red;
                Panic();
            }
            else
            {
                this.DataLBL.TextColor = Color.Green;
            }
            
            GC.Collect();
        }
        void ComputeTemp(bool placeholder)
        {

            this.DataLBL.FontSize = 20;
            float temp_calc = 36+((float)new Random().NextDouble() * 3);
            if (stxpg != null)
            {
                if (stxpg.link != null)
                {
                    stxpg.link.UpdateTemp(temp_calc);
                }
            }
            //handler.WriteTemp((double)temp_calc);
            if(Preference.Get<bool>("IsFarenheit"))
            {
                temp_calc = temp_calc * (9 / 5);
            }
            this.DataLBL.Text = temp_calc.ToString() + (Preference.Get<bool>("IsFarenheit") ? "F°" : "C°");
            if (!sport_enabled && temp_calc > 38)
            {
                this.DataLBL.TextColor = Color.Red;
                Panic();
            }
            else if (sport_enabled && temp_calc > 40)
            {
                this.DataLBL.TextColor = Color.Red;
                Panic();
            }
            else
            {
                this.DataLBL.TextColor = Color.Green;
            }

            GC.Collect();
        }
        async void Reader()
        {
        }
        async Task WaitFlag()
        {
            int sec = 10;
            while (true)
            {
                await Task.Delay(1000);
                sec--;
                this.DataLBL.FontSize = 10;
                this.DataLBL.Text = "Waiting "+sec.ToString();
                if (sec == 0)
                {
                    
                    break;
                }
                
            }
        }
        private void Disconnect_Clicked(object sender, EventArgs e)
        {
            GC.Collect();
            client.DisconnectAsync().Wait();
            DisplayAlert("Disconnected", "", "OK");
        }

        private void SETTTBTN_Clicked(object sender, EventArgs e)
        {
            GC.Collect();
            if(stxpg == null)
            {
                stxpg = new SettingsPage(client, ref set,this);
            }
            Navigation.PushModalAsync(stxpg);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void gtbtn_Clicked(object sender, EventArgs e)
        {
            ComputeTemp(true);
        }

        private void Sport_modebtn_Clicked_1(object sender, EventArgs e)
        {
            sport_enabled = !sport_enabled;
            if (!sport_enabled)
            {
                Sport_modebtn.BackgroundColor = Color.Green;

            }
            else
            {
                Sport_modebtn.BackgroundColor = Color.LightGreen;

            }
            ComputeTemp(characteristic.Value);
        }
    }
}