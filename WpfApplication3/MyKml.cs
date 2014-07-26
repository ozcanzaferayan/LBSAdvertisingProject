using Microsoft.Maps.MapControl.WPF;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadKmlFiles
{
    public class MyKml
    {
        public List<string> fileNames { get; set; }
        public List<string> filePaths { get; set; }
        public List<KmlFile> KmlFiles { get; set; }
        public string ActiveFile { get; set; }
        public List<Track> Tracks { get; set; }
        public List<Marker> Markers { get; set; }
        public LocationCollection Locations { get; set; }
        public MyKml() 
        {
            this.ActiveFile = "Hepsini Göster";
            this.Locations = new LocationCollection();
        }
        public MyKml(List<string> fileNames, List<string> filePaths) 
        {
            this.Markers = new List<Marker>();
            this.Locations = new LocationCollection();
            this.fileNames = fileNames;
            this.filePaths = filePaths;
            this.ActiveFile = "Hepsini Göster";
        }
       
    }
}
