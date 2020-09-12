using System;
using Xamarin.Forms;

namespace GreentegCoreApp1
{
    class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        [assembly: ExportFont("Lato-Regular.ttf", Alias = "Lato")]
        protected override void OnCreate()
        {
            base.OnCreate();

            LoadApplication(new App());
        }

        static void Main(string[] args)
        {
            var app = new Program();
            Forms.Init(app);
            global::Tizen.Wearable.CircularUI.Forms.Renderer.FormsCircularUI.Init();
            app.Run(args);
        }
    }
}
