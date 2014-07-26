using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadKmlFiles
{
    class LocationType
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public LocationType(double Latitude, double Longitude, double Altitude)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Altitude = Altitude;
        }

        public static LocationType AddLocations(LocationType l1, LocationType l2)
        {
            return new LocationType(l1.Latitude + l2.Latitude, l1.Longitude + l2.Longitude, l1.Altitude + l2.Altitude);
        }
    }
}
