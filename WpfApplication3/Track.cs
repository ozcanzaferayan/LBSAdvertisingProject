using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace ReadKmlFiles
{
    public class Track
    {
        public List<Marker> Markers { get; set; }
        public List<string> FileNames { get; set; }
    }
}
