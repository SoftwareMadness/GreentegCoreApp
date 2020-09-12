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
    public partial class Errorx : ContentPage
    {
        public Errorx(Exception ex)
        {
            InitializeComponent();
            if(ex != null)
            {
                this.Error.Text = ex.ToString();
            }
        }
    }
}