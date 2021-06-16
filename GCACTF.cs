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
                if(!string.IsNullOrEmpty(indata))
            {
                float flt = 0;
                if(float.TryParse(indata,out flt)&&flt != temperature)
                {
                    outdata += time.ToString("yyyy/M/dd-HH:m:s") + "," + temperature.ToString() + ";";
                }
                else
                {
                    outdata = temperature.ToString() + "&" + time.ToString("yyyy/M/dd-HH:m:s") + "," + temperature.ToString() + ";";
                }
            }
            return outdata;
     
        }
    }
}
