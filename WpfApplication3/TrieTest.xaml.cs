using ReadKmlFiles;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Typocalypse.Trie;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for TrieTest.xaml
    /// </summary>
    public partial class TrieTest : Window
    {
        List<string> times;
        List<SharpKml.Base.Vector> coordinates;
        Trie<SharpKml.Base.Vector> myTrie;
        public MyKml myKmlObject;
        public TrieTest()
        {
            InitializeComponent();
            times = new List<string>();
            coordinates = new List<SharpKml.Base.Vector>();
            myTrie = new Trie<SharpKml.Base.Vector>();
            myKmlObject = MainWindow.OpenKmlFile();
            MainWindow.ReadKmlFile(myKmlObject);
            AddKmlElementsToTrie(myKmlObject, myTrie);
            string a = "2012-04-07T07:11:17.048Z";
            for (int i = 0; i < a.Length; i++)
            {
                myTrie.Matcher.NextMatch(a[i]); 
            }
            if (myTrie.Matcher.IsExactMatch())
            {
                MessageBox.Show(myTrie.Matcher.GetExactMatch().Latitude + " , " + myTrie.Matcher.GetExactMatch().Longitude);
            }
            
        }
        public void AddKmlElementsToTrie(MyKml myKmlObject, Trie<SharpKml.Base.Vector> myTrie)
        {
            myKmlObject.Markers = new List<Marker>();
            for (int i = 0; i < myKmlObject.KmlFiles.Count; i++)
            {
                Kml kml = myKmlObject.KmlFiles[i].Root as Kml;
                foreach (var track in kml.Flatten().OfType<SharpKml.Dom.GX.Track>())
                {
                    String[] times = track.When.ToArray();
                    SharpKml.Base.Vector[] coordinates = track.Coordinates.ToArray();
                    for (int j = 0; j < times.Length; j++)
                    {
                        myTrie.Put(times[j], coordinates[j]);
                    }
                }
            }
        }
        public static double DifferenceTime(DateTime PastTime, DateTime PresentTime)
        {
            return Math.Round((double)(((System.TimeSpan)(PresentTime - PastTime)).TotalSeconds), 1);
        }
        public double DifferenceTimeWithHours(DateTime PastTime, DateTime PresentTime)
        {
            return Math.Round((double)(((System.TimeSpan)(PresentTime - PastTime)).TotalHours), 1);
        }
        public static double CalculateSpeed(Marker m1, Marker m2)
        {
            return ((m1.Coordinate.GetDistanceTo(m2.Coordinate)) * 1000) / ((DifferenceTime(m2.DateTime, m1.DateTime) * 360));
        }

    }
}
