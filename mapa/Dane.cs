using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Pwm;

namespace mapa
{
    internal class Dane
    {
        
        public static BasicGeoposition pktKoncowy { get;internal set; }
        public static string opisCelu { get; set; } = null;
        public static BasicGeoposition pkStartowy { get; internal set; }
    }
}
