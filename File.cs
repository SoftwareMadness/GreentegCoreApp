using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tizen.System;

namespace GreentegCoreApp1
{
    class FileTemp
    {
        
    }
    class FilesystemHandlerGC
    {
        string fp = "";
        Storage stor = null;
        public FilesystemHandlerGC()
        {
            stor = StorageManager.Storages.FirstOrDefault();
            try
            {
                if (!File.Exists(Path.Combine(stor.RootDirectory, @"gcore\datafile.log")))
                {
                    File.Create(Path.Combine(stor.RootDirectory, @"gcore\datafile.log"));
                }
                else
                {
                    File.Delete(Path.Combine(stor.RootDirectory, @"gcore\datafile.log"));
                    File.Create(Path.Combine(stor.RootDirectory, @"gcore\datafile.log"));
                }
                fp = Path.Combine(stor.RootDirectory, @"gcore\datafile.log");
            }
            catch (Exception ex)
            {
                throw ex;
               
            }


        }
        public void WriteTemp(double temp)
        {
            if (fp == "" || stor == null || stor.AvaliableSpace < 1000000) throw new Exception("Writing Error at WriteTemp()");
            DateTime time = DateTime.Now;
            File.AppendAllText(fp, "&[" + time.ToString("MM/dd/yyyy/HH/mm/ss") + "]" + temp.ToString() + "\n");

        }
    }
}
