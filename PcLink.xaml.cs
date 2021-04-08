using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GreentegCoreApp1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PcLink : ContentPage
    {
        WebClient wc = new WebClient();
        string token = "";
        public float Temperature { get; set; }
        public GCACTFEncoder encoder = new GCACTFEncoder();
        public PcLink()
        {
            InitializeComponent();
        
        }
        void GenerateQR(string codeValue)
        {
     
            
            QrCode.Source = StreamImageSource.FromStream(() => new MemoryStream(wc.DownloadData(Variables.BASEPcLinkURL+"/phpqrcode/gengrapi.php?data=" + Convert.ToBase64String(Encoding.ASCII.GetBytes(codeValue)))));
            QrCode.WidthRequest = Application.Current.MainPage.Width;
            QrCode.HeightRequest = Application.Current.MainPage.Height;
        }
        public void UpdateTemp(float temp)
        {
            encoder.DATATEMP = encoder.AppendData(temp, DateTime.Now, encoder.DATATEMP);
            if (token != "")
            {
                try
                {
                    wc.DownloadString(Variables.BASEPcLinkURL + "/actions.php?ACTION=UpdateData&AuthTKN=" + token + "&Data=" + encoder.DATATEMP);
                }
                catch(Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }
        private void strtbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                
                token = wc.DownloadString(Variables.BASEPcLinkURL+"/actions.php?ACTION=genToken");
               GenerateQR(Variables.BASEPcLinkURL+"/?Token=" + token);
       
                this.BackgroundColor = Color.White;
             
            }catch(Exception exc)
            {
                DisplayAlert("Critical Error", exc.Message, "OK");
            }
            strtbtn.IsVisible = false;
        }
    }
}
