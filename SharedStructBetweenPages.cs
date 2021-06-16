using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.Network.Bluetooth;

namespace GreentegCoreApp1
{
    public class SharedStructBetweenPages
    {
       public BluetoothGattClient device { get; set; }
    }
}
