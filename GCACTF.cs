using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tizen;

namespace GreentegCoreApp1
{
    public class GCACTFEncoder
    {
        public string DATATEMP = "";
        public string AppendData(float temperature,DateTime time,string indata)
        {
         
                string outdata = "";
                try
                {
                if (!string.IsNullOrEmpty(indata))
                {
                    outdata = indata.Split("&")[1];
                    if (float.Parse(indata.Split(';').Last().Split(',')[1]) != temperature)
                    {
                        outdata += time.ToString("yyyy/M/dd-HH:m:s") + "," + temperature.ToString() + ";";
                    }
                    outdata = temperature.ToString() + outdata;
                    return outdata;
                }
                else
                {
                    outdata = temperature.ToString() + "&" + time.ToString("yyyy/M/dd-HH:m:s") + "," + temperature.ToString() + ";";
                    return outdata;
                }
                }
                catch(Exception ex)
                {
                Log.Info("GCACTFEncoder Error", ex.Message);
                    outdata = temperature.ToString() + "&" + time.ToString("yyyy/M/dd-HH:m:s") + "," + temperature.ToString() + ";";
                    return outdata;
                }
     
        }
    }
}
