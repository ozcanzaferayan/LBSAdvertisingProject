using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using SharpKml.Base;
using Microsoft.Maps.MapControl.WPF;

namespace ReadKmlFiles
{

    public class Marker

    {
        #region DBSCAN Properties

        public const int NOISE = -1;
        public const int UNCLASSIFIED = 0;
        public int DBSCANClusterType { get; set; }

        #endregion

        public GeoCoordinate Coordinate { get; set; }
        public DateTime DateTime { get; set; }
        public int ClusterType { get; set; }
        
        public int ClusterTypeSpeed { get; set; }
        public string TypeOfMovement { get; set; }
        public int SpeedClusterDBSCAN;
        public string FileName { get; set; }

        public Marker(Location location)
        {
            Coordinate = new GeoCoordinate();
            this.Coordinate.Latitude = location.Latitude;
            this.Coordinate.Longitude = location.Longitude;
        }

        public Marker(Vector v, string FileName)
        {
            Coordinate = new GeoCoordinate();
            this.Coordinate.Latitude = v.Latitude;
            this.Coordinate.Longitude = v.Longitude;
            this.Coordinate.Altitude = v.Altitude.Value;
            this.FileName = FileName; 
        }
        public Marker(Vector v, string FileName, DateTime DateTime, double Speed)
        {
            Coordinate = new GeoCoordinate();
            this.Coordinate.Latitude = v.Latitude;
            this.Coordinate.Longitude = v.Longitude;
            this.Coordinate.Altitude = v.Altitude.Value;
            this.Coordinate.Speed = Speed;
            this.FileName = FileName;
            this.DateTime = DateTime;
        }
        public Marker()
        {
            this.Coordinate = new GeoCoordinate();
            this.Coordinate.Latitude = 0;
            this.Coordinate.Longitude = 0;
            this.Coordinate.Altitude = 0;
            this.Coordinate.Speed = 0;
            this.DBSCANClusterType = 0;
        }
        public override string ToString()
        {
            return
                "Latitude: " + Coordinate.Latitude + ", " +
                "Longitude: " + Coordinate.Longitude + ", " +
                "Altitude: " + Coordinate.Altitude + ", " +
                "Time: " + DateTime + ", " +
                "ClusterType: " + ClusterType + ", " +
                "Speed: " + Math.Round(Coordinate.Speed, 1).ToString() + " km/sa | ";
        }
        public Location GetLocation()
        {
            return new Location(this.Coordinate.Latitude, this.Coordinate.Longitude, this.Coordinate.Altitude);
        }
    }
}
