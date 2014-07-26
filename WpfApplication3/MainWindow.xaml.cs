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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System.IO;
using Microsoft.Win32;
using Microsoft.Maps.MapControl.WPF;
using System.Globalization;
using Accord;
using Accord.MachineLearning;
using Accord.Imaging;
using Accord.Math;
using Accord.Statistics;
using Accord.Imaging.Converters;
using System.Reflection;
using Emgu.CV;
using Emgu.Util;
using Kalman_Filter;
using ReadKmlFiles;


namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyKml myKmlObject;
        List<Marker> markerList;
        public Kalman kal;
        public SyntheticData syntheticData;
        private bool expanderExpanded = false;
        public string[] ColorValues = new string[] { 
                "FF0000", "00FF00", 
                "FFFF00", "FF00FF", "00FFFF", "000000", 
                "800000", "008000", "000080", "808000", "800080", "008080", "808080", 
                "C00000", "00C000", "0000C0", "C0C000", "C000C0", "00C0C0", "C0C0C0", 
                "400000", "004000", "000040", "404000", "400040", "004040", "404040", 
                "200000", "002000", "000020", "202000", "200020", "002020", "202020", 
                "600000", "006000", "000060", "606000", "600060", "006060", "606060", 
                "A00000", "00A000", "0000A0", "A0A000", "A000A0", "00A0A0", "A0A0A0", 
                "E00000", "00E000", "0000E0", "E0E000", "E000E0", "00E0E0", "E0E0E0", 
            };
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            tbStatus.Text = "Dosyalar okunuyor..";
            myKmlObject = OpenKmlFile();
            DateTime pastTime = DateTime.Now;
            ReadKmlFile(myKmlObject);
            myKmlObject = ReadKmlElements(myKmlObject);
            AddFileNamesToList(myKmlObject);
            DateTime presentTime = DateTime.Now;
            String totalTime = DifferenceTime(pastTime, presentTime).ToString();
            tbStatus.Text = "Dosya okuma tamamlandı. (" + totalTime + " saniye)";
        }
        public static MyKml OpenKmlFile()
        {
            List<string> fileNames = new List<string>();
            List<string> filePaths = new List<string>();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "KML Files (*.kml)|*.kml|All Files (*.*)|*.*";
            dialog.DefaultExt = ".kml";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true && dialog.FileName != "")
            {
                for (int i = 0; i < dialog.FileNames.Length; i++)
                {
                    FileInfo fInfo = new FileInfo(dialog.FileNames[i]);
                    if (fInfo.Extension == ".kml")
                    {
                        filePaths.Add(dialog.FileNames[i]);
                        fileNames.Add(dialog.SafeFileNames[i]);
                    }
                    else
                    {
                        MessageBox.Show("Incorrect File Type!");
                    }
                }
            }
            fileNames.Add("Hepsini Göster");
            MyKml myKmlObject = new MyKml(fileNames, filePaths);
            return myKmlObject;
        }
        public static void ReadKmlFile(MyKml UsersKmlFile)
        {
            UsersKmlFile.KmlFiles = new List<KmlFile>();
            KmlFile file;
            for (int i = 0; i < UsersKmlFile.filePaths.Count; i++)
            {
                file = KmlFile.Load(UsersKmlFile.filePaths[i]);
                UsersKmlFile.KmlFiles.Add(file);
            }
        }
        public static MyKml ReadKmlElements(MyKml myKmlObject)
        {
            double maxSpeed = 0;
            int j = 0;
            myKmlObject.Markers = new List<Marker>();
            for (int i = 0; i < myKmlObject.KmlFiles.Count; i++)
            {
                Kml kml = myKmlObject.KmlFiles[i].Root as Kml;
                foreach (var track in kml.Flatten().OfType<SharpKml.Dom.GX.Track>())
                {
                    double speed = 0;
                    string[] times = track.When.ToArray();
                    SharpKml.Base.Vector[] coordinates = track.Coordinates.ToArray();
                    DateTime myDateTime = DateTime.Now;
                    for (int k = 0; true; k++)
                    {
                        try
                        {
                            if (myDateTime == DateTime.Parse(times[k]))
                            {
                                continue;
                            }
                            myDateTime = DateTime.Parse(times[k]);
                        }
                        catch (Exception)
                        {
                            break;
                        }
                        Marker m = new Marker(coordinates[k], myKmlObject.fileNames[i], myDateTime, speed);
                        if (j > 0)
                        {
                            m.Coordinate.Speed = CalculateSpeed(m, myKmlObject.Markers[j - 1]);
                            if (((m.Coordinate.Speed - myKmlObject.Markers[j - 1].Coordinate.Speed) > 25 && j != 1) || ((k!=0)&&(m.Coordinate.Speed ==0 ))) continue; // Lokasyon hatasını Hız farkı ile hesaplamak için
                        }
                        if (m.Coordinate.Speed > maxSpeed) maxSpeed = m.Coordinate.Speed;
                        myKmlObject.Locations.Add(m.GetLocation());
                        myKmlObject.Markers.Add(m);
                        j++;
                    }
                }
            }
            return myKmlObject;
        }
        public void AddFileNamesToList(MyKml myKmlObject)
        {
            foreach (var fileName in myKmlObject.fileNames)
            {
                lbFiles.Items.Add(fileName);
            }
        }
        private void lbFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            myKmlObject.ActiveFile = lbFiles.SelectedItem.ToString();
            ShowTracks(myKmlObject);
            SetSliderMaximumValue();
        }
        public void ShowTracks(MyKml myKmlObject)
        {
            mapTracksLayer.Children.Clear();
            MapPolyline polyline = new MapPolyline();
            LocationCollection polylineLocations = new LocationCollection();
            polyline.Opacity = 1;
            polyline.StrokeThickness = 2;
            polyline.Stroke = new SolidColorBrush(Colors.Blue);
            foreach (var marker in myKmlObject.Markers)
            {
                if (marker.FileName != myKmlObject.ActiveFile && myKmlObject.ActiveFile != "Hepsini Göster")
                {
                    continue;
                }
                polylineLocations.Add(marker.GetLocation());
            }
            polyline.Locations = polylineLocations;
            mapTracksLayer.Children.Add(polyline);
        }
        public void ShowTracks(MyKml myKmlObject, SolidColorBrush color)
        {
            MapPolyline polyline = new MapPolyline();
            LocationCollection polylineLocations = new LocationCollection();
            polyline.StrokeThickness = 2;
            polyline.Stroke = color;
            polyline.Opacity = 0.5;
            foreach (var marker in myKmlObject.Markers)
            {
                if (marker.FileName != myKmlObject.ActiveFile && myKmlObject.ActiveFile != "Hepsini Göster")
                {
                    continue;
                }
                polylineLocations.Add(marker.GetLocation());
            }
            polyline.Locations = polylineLocations;
            mapTracksLayer.Children.Add(polyline);
        }
        public System.Windows.Shapes.Rectangle MakeRectangle(Color color, Marker marker)
        {
            System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
            r.Width = 2;
            r.Height = 2;
            r.Fill = new SolidColorBrush(color);
            r.ToolTip = marker.ToString();
            r.Opacity = 1;
            return r;
        }
        public System.Windows.Shapes.Rectangle MakeRectangle(Color color)
        {
            System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
            r.Width = 5;
            r.Height = 5;
            r.Fill = new SolidColorBrush(color);
            r.Opacity = 1;
            return r;
        }
        public System.Windows.Shapes.Rectangle MakeRectangle(Color color, Marker marker, double size)
        {
            System.Windows.Shapes.Rectangle r = MakeRectangle(color, marker);
            r.Width = size;
            r.Height = size;
            r.Opacity = 1;
            return r;
        }
        private void btnCluster_Click(object sender, RoutedEventArgs e)
        {
            tbStatus.Text = "Clusterlar hesaplanıyor..";
            DateTime pastTime = DateTime.Now;
            int numClusters = Convert.ToInt32(txtClusterCount.Text);
            int numAttributes = 6;
            int maxCount = 30;
            int[] clustering = ClusteringKMeansProgram.Cluster(myKmlObject.Markers.ToArray(), numClusters, numAttributes, maxCount, numClusters);
            int currentCluster = 0;
            DateTime currentDateTime = DateTime.Now;
            List<KeyValuePair<Marker, double>> waitingTimes = new List<KeyValuePair<Marker, double>>();
            for (int i = 0; i < myKmlObject.Markers.Count; i++)
            {
                myKmlObject.Markers[i].ClusterType = clustering[i];
                if (i == 0)
                {
                    currentCluster = myKmlObject.Markers[i].ClusterType;
                    currentDateTime = myKmlObject.Markers[i].DateTime;
                }
                else if (currentCluster != myKmlObject.Markers[i].ClusterType)
                {
                    double waitingTime = DifferenceTimeMinutes(currentDateTime, myKmlObject.Markers[i].DateTime);
                    waitingTimes.Add(new KeyValuePair<Marker, double>(myKmlObject.Markers[i], waitingTime));
                    if (waitingTime > 15)
                    {
                        Pushpin p = new Pushpin();
                        p.Location = myKmlObject.Markers[i - 50].GetLocation();
                        List<String> address = GetAddress.MakeGeocodeRequestForString(p.Location);
                        string placeAddress;
                        if (address.Count==0)
                        {
                            placeAddress = "Yer bulunamadı";    
                        }
                        else
                        {
                            placeAddress = address[0];
                        }
                        if (waitingTime>60)
                        {
                            waitingTime /= 60;
                            waitingTime = Math.Round(waitingTime, 1);
                            p.Content = waitingTime + "h";
                            p.Background = new SolidColorBrush(Colors.Green);
                            lbWaitingClusters.Items.Add(myKmlObject.Markers[i].ClusterType + " " + waitingTime + "h " + placeAddress);
                        }
                        else
                        {
                            waitingTime = Math.Round(waitingTime, 1);
                            p.Content = waitingTime.ToString();
                            lbWaitingClusters.Items.Add(myKmlObject.Markers[i].ClusterType + " " + waitingTime + "min " + placeAddress);
                        }
                        mapPushPinLayer.Children.Add(p);
                        
                    }
                    currentDateTime = myKmlObject.Markers[i].DateTime;
                    currentCluster = myKmlObject.Markers[i].ClusterType;
                }
                lbCluster.Items.Add(i.ToString() + " " + myKmlObject.Markers[i]);
            }
            for (int i = 0; i < numClusters; i++)
            {
                lbClusterGroups.Items.Add(GenerateRandomColoredListboxItem(i));
            }
            GenerateListBoxContent(numClusters, lbClusterGroups);
            DateTime presentTime = DateTime.Now;
            tbStatus.Text = "Clusterlar oluşturuldu. (" + DifferenceTime(pastTime, presentTime) + " saniye)";
        }

        private void GenerateMatrix()
        {

        }

        private void GetClusterWaitingTime(Marker marker, int i)
        {
            int firstCluster;
            if (i == 0)
            {
                firstCluster = marker.ClusterType;
            }

        }
        private void lbSpeedGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mapTracksLayer.Children.Clear();
            foreach (var marker in myKmlObject.Markers)
            {
                if (marker.FileName != myKmlObject.ActiveFile && myKmlObject.ActiveFile != "Hepsini Göster")
                {
                    continue;
                }
                if (0 <= marker.Coordinate.Speed && marker.Coordinate.Speed < 1) // 0 - 1 m/s
                {
                    if (lbSpeedGroups.SelectedIndex != 0 && lbSpeedGroups.SelectedIndex != 4) continue;
                    mapTracksLayer.AddChild(MakeRectangle(Colors.Green, marker), new Microsoft.Maps.MapControl.WPF.Location(marker.Coordinate.Latitude, marker.Coordinate.Longitude, marker.Coordinate.Altitude));
                }
                else if (1 < marker.Coordinate.Speed && marker.Coordinate.Speed < 4) // 1 - 4 m/s
                {
                    if (lbSpeedGroups.SelectedIndex != 1 && lbSpeedGroups.SelectedIndex != 4) continue;
                    mapTracksLayer.AddChild(MakeRectangle(Colors.Blue, marker), new Microsoft.Maps.MapControl.WPF.Location(marker.Coordinate.Latitude, marker.Coordinate.Longitude, marker.Coordinate.Altitude));
                }
                else if (4 < marker.Coordinate.Speed && marker.Coordinate.Speed < 15) // 4 - 15 m/s
                {
                    if (lbSpeedGroups.SelectedIndex != 2 && lbSpeedGroups.SelectedIndex != 4) continue;
                    mapTracksLayer.AddChild(MakeRectangle(Colors.Yellow, marker), new Microsoft.Maps.MapControl.WPF.Location(marker.Coordinate.Latitude, marker.Coordinate.Longitude, marker.Coordinate.Altitude));
                }
                else                                                                   // 15 - Infinity m/s
                {
                    if (lbSpeedGroups.SelectedIndex != 3 && lbSpeedGroups.SelectedIndex != 4) continue;
                    mapTracksLayer.AddChild(MakeRectangle(Colors.Red, marker), new Microsoft.Maps.MapControl.WPF.Location(marker.Coordinate.Latitude, marker.Coordinate.Longitude, marker.Coordinate.Altitude));
                }
            }
            //SetSliderMaximumValue();
        }
        private void lbClusterGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mapTracksLayer.Children.Clear();
            if (lbClusterGroups.SelectedIndex == lbClusterGroups.Items.Count - 1)
            {
                CreateAllClusterPath(myKmlObject);
                SetSliderMaximumValue();
                return;
            }
            MapPolyline polyline = new MapPolyline();
            polyline.Locations = new LocationCollection();
            polyline.StrokeThickness = 2;
            foreach (var marker in myKmlObject.Markers)
            {
                if (marker.FileName != myKmlObject.ActiveFile && myKmlObject.ActiveFile != "Hepsini Göster")
                {
                    continue;
                }
                if (marker.ClusterType == lbClusterGroups.SelectedIndex)
                {
                    polyline.Locations.Add(marker.GetLocation());
                }

            }
            polyline.Stroke = ((ListBoxItem)lbClusterGroups.SelectedItem).Background;
            mapTracksLayer.Children.Add(polyline);
            SetSliderMaximumValue();
        }
        public void CreateAllClusterPath(MyKml myKmlObject)
        {
            for (int i = 0; i < (Convert.ToInt32(txtClusterCount.Text)); i++)
            {
                MapPolyline polyline = new MapPolyline();
                polyline.Locations = new LocationCollection();
                polyline.StrokeThickness = 2;
                polyline.Stroke = ((ListBoxItem)lbClusterGroups.Items[i]).Background;
                foreach (var marker in myKmlObject.Markers)
                {
                    if (i == marker.ClusterType)
                    {
                        polyline.Locations.Add(marker.GetLocation());
                    }
                }
                mapTracksLayer.Children.Add(polyline);
            }
        }
        private void btnClusterBySpeed_Click(object sender, RoutedEventArgs e)
        {
            double[] maxSpeed = new double[4];
            double[] avgSpeed = new double[4];
            double[] minSpeed = new double[4];
            int speedClusterCount = Convert.ToInt32(txtSpeedClusterCount.Text);
            KMeans kmeans = new KMeans(speedClusterCount);
            double[][] speeds = new double[myKmlObject.Markers.Count][];
            for (int i = 0; i < myKmlObject.Markers.Count; i++)
            {
                speeds[i] = new double[speedClusterCount - 1];
                speeds[i][0] = myKmlObject.Markers[i].Coordinate.Speed;
                for (int j = 0; j < speedClusterCount - 2; j++)
                {
                    speeds[i][j + 1] = 0;
                }
            }
            int[] labels = kmeans.Compute(speeds, 50);
            for (int i = 0; i < myKmlObject.Markers.Count; i++)
            {
                
                myKmlObject.Markers[i].ClusterTypeSpeed = labels[i];
                if (i == 0)
                {
                    minSpeed[0] = minSpeed[1] = minSpeed[2] = minSpeed[3] = myKmlObject.Markers[i].Coordinate.Speed;
                    maxSpeed[0] = maxSpeed[1] = maxSpeed[2] = maxSpeed[3] = myKmlObject.Markers[i].Coordinate.Speed;
                }
                if (double.IsNaN(myKmlObject.Markers[i].Coordinate.Speed)) continue;
                if (maxSpeed[labels[i]] < myKmlObject.Markers[i].Coordinate.Speed)
                    maxSpeed[labels[i]] = myKmlObject.Markers[i].Coordinate.Speed;
                if (minSpeed[labels[i]] > myKmlObject.Markers[i].Coordinate.Speed)
                    minSpeed[labels[i]] = myKmlObject.Markers[i].Coordinate.Speed;
                avgSpeed[labels[i]] += myKmlObject.Markers[i].Coordinate.Speed;
            }
            for (int i = 0; i < avgSpeed.Length; i++)
            {
                avgSpeed[i] /= myKmlObject.Markers.Count;
            }
            GenerateListBoxContent(speedClusterCount, lbClusterSpeedGroups);
        }
        private void lbClusterSpeedGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mapTracksLayer.Children.Clear();
            foreach (var marker in myKmlObject.Markers)
            {
                if (marker.FileName != myKmlObject.ActiveFile && myKmlObject.ActiveFile != "Hepsini Göster")
                {
                    continue;
                }
                System.Windows.Shapes.Rectangle r = MakeRectangle(((SolidColorBrush)((ListBoxItem)lbClusterSpeedGroups.Items[marker.ClusterTypeSpeed]).Background).Color, marker, 3);
                if (marker.ClusterTypeSpeed == lbClusterSpeedGroups.SelectedIndex || lbClusterSpeedGroups.SelectedIndex == lbClusterSpeedGroups.Items.Count - 1)
                {
                    mapTracksLayer.AddChild(r, marker.GetLocation());
                }


            }
        }
        public double DifferenceTimeMinutes(DateTime PastTime, DateTime PresentTime)
        {
            return Math.Round((double)(((System.TimeSpan)(PresentTime - PastTime)).TotalMinutes), 1);
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
            return ((m1.Coordinate.GetDistanceTo(m2.Coordinate))*1000) / ((DifferenceTime(m2.DateTime, m1.DateTime)*360));
        }
        #region Expander Collapsed - Expanded Methods

        private void Expander_Collapsed_1(object sender, RoutedEventArgs e)
        {
            txtExpanderHeader.Text = "Genişlet";
        }

        private void Expander_Expanded_1(object sender, RoutedEventArgs e)
        {
            if (expanderExpanded == false)
            {
                expanderExpanded = true;
            }
            else
            {
                txtExpanderHeader.Text = "Daralt";
            }

        }

        #endregion
        private void sliPath_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mapShapeLayer.Children.Clear();
            System.Windows.Shapes.Rectangle r = MakeRectangle(Colors.Red);
            r.ToolTip = myKmlObject.Locations[Convert.ToInt32(sliPath.Value)].ToString();
            mapShapeLayer.AddChild(r, myKmlObject.Locations[Convert.ToInt32(sliPath.Value)]);
            MyMap.SetView(myKmlObject.Locations[Convert.ToInt32(sliPath.Value)], MyMap.ZoomLevel);
        }
        public void SetSliderMaximumValue()
        {
            sliPath.Maximum = 0;
            foreach (MapPolyline polyline in mapTracksLayer.Children)
            {
                sliPath.Maximum += polyline.Locations.Count - 1;
            }
        }
        public ListBoxItem GenerateRandomColoredListboxItem(int Index)
        {
           
            ColorConverter converter = new ColorConverter();
            Color randomColor = (Color)ColorConverter.ConvertFromString("#" + ColorValues[Index]);
            ListBoxItem lbi = new ListBoxItem();
            lbi.Background = new SolidColorBrush(randomColor);
            lbi.Content = "ClusterType " + Index.ToString();
            return lbi;
        }
        public void GenerateListBoxContent(int clusterCount, ListBox listBoxName)
        {
            for (int i = 0; i < clusterCount; i++)
            {
                listBoxName.Items.Add(GenerateRandomColoredListboxItem(i));
            }
            ListBoxItem hepsiLbi = new ListBoxItem();
            hepsiLbi.Content = "Hepsi";
            LinearGradientBrush buttonBrush = new LinearGradientBrush();
            buttonBrush.StartPoint = new System.Windows.Point(0, 0);
            buttonBrush.EndPoint = new System.Windows.Point(0, 1);
            for (int i = 0; i < clusterCount; i++)
            {
                GradientStop gs = new GradientStop(((SolidColorBrush)(((ListBoxItem)(listBoxName.Items[i])).Background)).Color, ((Convert.ToDouble(i) * clusterCount) / 100));
                GradientStop gs2 = new GradientStop(((SolidColorBrush)(((ListBoxItem)(listBoxName.Items[i])).Background)).Color, (((Convert.ToDouble(i) * clusterCount) / 100) + 0.1));
                buttonBrush.GradientStops.Add(gs);
                buttonBrush.GradientStops.Add(gs2);
            }
            hepsiLbi.Background = buttonBrush;
            listBoxName.Items.Add(hepsiLbi);
        }
        public void GenerateTextBoxContent(List<Marker> markers)
        {
           
        }

        public void KalmanFilter()
        {
            kal = new Kalman(4, 2, 0);
            syntheticData = new SyntheticData();
            Matrix<float> state = new Matrix<float>(new float[]
            {
                0.0f, 0.0f, 0.0f, 0.0f
            });
            kal.CorrectedState = state;
            kal.TransitionMatrix = syntheticData.transitionMatrix;
            kal.MeasurementNoiseCovariance = syntheticData.measurementNoise;
            kal.ProcessNoiseCovariance = syntheticData.processNoise;
            kal.ErrorCovariancePost = syntheticData.errorCovariancePost;
            kal.MeasurementMatrix = syntheticData.measurementMatrix;
        }

        public Marker filterPoints(Marker marker, int i)
        {
            syntheticData.state[0, 0] = (float)marker.GetLocation().Latitude;
            syntheticData.state[1, 0] = (float)marker.GetLocation().Longitude;
            Matrix<float> prediction = kal.Predict();
            System.Drawing.PointF predictPoint = new System.Drawing.PointF(prediction[0, 0], prediction[1, 0]);
            System.Drawing.PointF measurePoint = new System.Drawing.PointF(syntheticData.GetMeasurement()[0, 0],
                syntheticData.GetMeasurement()[1, 0]);
            Matrix<float> estimated = kal.Correct(syntheticData.GetMeasurement());
            System.Drawing.PointF estimatedPoint = new System.Drawing.PointF(estimated[0, 0], estimated[1, 0]);
            syntheticData.GoToNextState();
            System.Drawing.PointF[] results = new System.Drawing.PointF[2];
            if (i < 100) return marker;
            marker.Coordinate.Latitude = estimatedPoint.X;
            marker.Coordinate.Longitude = estimatedPoint.Y;
            return marker;
        }

        public MyKml FilterMarkers(MyKml myKmlObject)
        {
            KalmanFilter();
            for (int i = 0; i < myKmlObject.Markers.Count; i++)
            {
                Marker m = filterPoints(myKmlObject.Markers[i], i);
            }
            return myKmlObject;
        }

        private void btnFilterPoints_Click(object sender, RoutedEventArgs e)
        {
            myKmlObject = FilterMarkers(myKmlObject);
            ShowTracks(myKmlObject, new SolidColorBrush(Colors.Yellow));
        }
        private void btnDBSCAN_Click(object sender, RoutedEventArgs e)
        {
            DBSCAN d = new DBSCAN();
            d.GetClusters(myKmlObject.Markers, 0.000300, 40);
            //FillPushpinLayerWithPoints(myKmlObject.Markers);
            foreach (var point in myKmlObject.Markers)
            {
                Microsoft.Maps.MapControl.WPF.Location l = new Microsoft.Maps.MapControl.WPF.Location(point.Coordinate.Latitude, point.Coordinate.Longitude);
                switch (point.DBSCANClusterType)
                {
                    case 1:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[1])),
                            l);
                        break;
                    case 2:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[2])),
                            l);
                        break;
                    case 3:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[3])),
                            l);
                        break;
                    case 4:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[4])),
                            l);
                        break;
                    case 5:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[5])),
                            l);
                        break;
                    case 6:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[6])),
                            l);
                        break;
                    case 7:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[7])),
                            l);
                        break;
                    case 8:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[8])),
                            l);
                        break;
                    case 9:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[9])),
                            l);
                        break;
                    case 10:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[10])),
                            l);
                        break;
                    case 11:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[11])),
                            l);
                        break;
                    case 12:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[12])),
                            l);
                        break;
                    case 13:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[13])),
                            l);
                        break;
                    case 14:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[14])),
                            l);
                        break;
                    case 15:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[15])),
                            l);
                        break;
                    case 16:
                        mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[16])),
                            l);
                        break;
                    default:
                        break;
                }
            }
        }
        //private void btnDBSCAN_Click(object sender, RoutedEventArgs e)
        //{
        //    LocationCollection lCollection = new LocationCollection();
        //    DBSCAN d = new DBSCAN();
        //    List<Point> points = new List<Point>();
        //    for (int i = 0; i < myKmlObject.Markers.Count; i++)
        //    {
        //        points.Add(new Point(myKmlObject.Markers[i].Coordinate.Latitude, myKmlObject.Markers[i].Coordinate.Longitude));
        //    }
        //    d.GetClusters(points, 0.000300, 50);
        //    foreach (var point in points)
        //    {
        //        Microsoft.Maps.MapControl.WPF.Location l = new Microsoft.Maps.MapControl.WPF.Location(point.X, point.Y);
        //        switch (point.ClusterId)
        //        {
        //            case 1:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[1])),
        //                    l);
        //                break;
        //            case 2:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[2])),
        //                    l);
        //                break;
        //            case 3:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[3])),
        //                    l);
        //                break;
        //            case 4:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[4])),
        //                    l);
        //                break;
        //            case 5:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[5])),
        //                    l);
        //                break;
        //            case 6:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[6])),
        //                    l);
        //                break;
        //            case 7:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[7])),
        //                    l);
        //                break;
        //            case 8:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[8])),
        //                    l);
        //                break;
        //            case 9:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[9])),
        //                    l);
        //                break;
        //            case 10:
        //                mapTracksLayer.AddChild(MakeRectangle((Color)ColorConverter.ConvertFromString("#" + ColorValues[10])),
        //                    l);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        public Marker MyMedianFilter(List<Marker> markers)
        {
            double centerLat = 0, centerLong = 0;
            double stdLat = 0, stdLong = 0, stdDist = 0;
            foreach (var marker in markers)
            {
                centerLat += marker.Coordinate.Latitude;
                centerLong += marker.Coordinate.Longitude;
            }
            int a = 120;
            centerLat /= markers.Count;
            centerLong /= markers.Count;
            Marker centerMarker = new Marker();
            centerMarker.Coordinate.Latitude = centerLat;
            centerMarker.Coordinate.Longitude = centerLong;
            for (int i = 0; i < markers.Count; i++)
            {
                stdDist += markers[i].Coordinate.GetDistanceTo(centerMarker.Coordinate);
            }
            stdDist = Math.Sqrt(stdDist);
            int temp = 0;
            double lat = 0, lng = 0;
            double tempDistanceTotal = 0;
            for (int i = 0; i < markers.Count; i++)
            {
                double markerDistance = markers[i].Coordinate.GetDistanceTo(centerMarker.Coordinate);
                if (true) //markerDistance <= stdDist / 2)
                {
                    lat += markers[i].Coordinate.Latitude;
                    lng += markers[i].Coordinate.Longitude;
                    temp++;
                }
            }
            lat /= temp;
            lng /= temp;
            Marker returnedMarker = new Marker();
            returnedMarker.Coordinate.Latitude = lat;
            if (double.IsNaN(lat))
            {
                //int a = 2;
            }
            returnedMarker.Coordinate.Longitude = lng;
            return returnedMarker;
        }
        public void FillPushpinLayerWithPoints(List<Marker> points)
        {
            int currentDBSCANCluster = 0;
            DateTime currentDateTime = DateTime.Now;
            List<KeyValuePair<Marker, double>> waitingTimes = new List<KeyValuePair<Marker, double>>();
            for (int i = 0; i < myKmlObject.Markers.Count; i++)
            {
                //points[i].CurrentDBSCANCluster = clustering[i];
                if (i == 0)
                {
                    currentDBSCANCluster = points[i].DBSCANClusterType;
                    currentDateTime = points[i].DateTime;
                }
                else if (currentDBSCANCluster != myKmlObject.Markers[i].DBSCANClusterType)
                {
                    double waitingTime = DifferenceTimeMinutes(currentDateTime, myKmlObject.Markers[i].DateTime);
                    waitingTimes.Add(new KeyValuePair<Marker, double>(myKmlObject.Markers[i], waitingTime));
                    if (waitingTime > 15)
                    {
                        Pushpin p = new Pushpin();
                        p.Location = myKmlObject.Markers[i - 50].GetLocation();
                        p.Content = waitingTime.ToString();
                        mapPushPinLayer.Children.Add(p);
                    }
                    currentDateTime = myKmlObject.Markers[i].DateTime;
                    currentDBSCANCluster = myKmlObject.Markers[i].DBSCANClusterType;
                }
            }
        }
        private void btnMedian_Click(object sender, RoutedEventArgs e)
        {
            
            markerList = new List<Marker>();
            List<Marker> tempMarkerList = new List<Marker>();
            //tempMarkerList = myKmlObject.Markers;
            for (int i = 0; i < myKmlObject.Markers.Count; i++)
            {
                if (i < Convert.ToInt32(txtMedian.Text)) 
                {
                    for (int j = 0; j < Convert.ToInt32(txtMedian.Text); j++)
                    {
                        tempMarkerList.Add(myKmlObject.Markers[j]);
                    }
                }
                else if ((myKmlObject.Markers.Count - i) <= Convert.ToInt32(txtMedian.Text))
                {
                    for (int j = myKmlObject.Markers.Count; (myKmlObject.Markers.Count - j )> Convert.ToInt32(txtMedian.Text); j--)
                    {
                        tempMarkerList.Add(myKmlObject.Markers[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < Convert.ToInt32(txtMedian.Text); j++)
                    {
                        tempMarkerList.Add(myKmlObject.Markers[i+j]);
                    }
                }
                markerList.Add(MyMedianFilter(tempMarkerList));
                tempMarkerList.Clear();

            }
            LocationCollection polyLineLocations = new LocationCollection();
            foreach (var marker in markerList)
            {
                if (double.IsNaN(marker.Coordinate.Latitude))
                {
                    continue;
                }
                polyLineLocations.Add(new Microsoft.Maps.MapControl.WPF.Location(marker.Coordinate.Latitude, marker.Coordinate.Longitude, marker.Coordinate.Altitude));
            }
            MapPolyline polyline = new MapPolyline();
            polyline.Locations = polyLineLocations;
            polyline.Stroke = new SolidColorBrush(Colors.Red);
            polyline.Opacity = 1;
            polyline.StrokeThickness = 1;
            mapTracksLayer.Children.Add(polyline);
        }
        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            //System.Windows.Point mousePosition = e.GetPosition(this);
            //Microsoft.Maps.MapControl.WPF.Location pointLocation = MyMap.ViewportPointToLocation(
            //        mousePosition);
            //string Results = "";
            //try
            //{
            //    // Set a Bing Maps key before making a request
            //    string key = "Bing Maps Key";

            //    GeocodeService.ReverseGeocodeRequest reverseGeocodeRequest = new GeocodeService.ReverseGeocodeRequest();

            //    // Set the credentials using a valid Bing Maps key
            //    reverseGeocodeRequest.Credentials = new GeocodeService.Credentials();
            //    reverseGeocodeRequest.Credentials.ApplicationId = key;

            //    // Set the point to use to find a matching address
            //    GeocodeService.Location point = new GeocodeService.Location();
            //    point.Latitude = 47.608;
            //    point.Longitude = -122.337;

            //    reverseGeocodeRequest.Location = point;

            //    // Make the reverse geocode request
            //    GeocodeService.GeocodeServiceClient geocodeService =
            //    new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            //    GeocodeService.GeocodeResponse geocodeResponse = geocodeService.ReverseGeocode(reverseGeocodeRequest);

            //    Results = geocodeResponse.Results[0].DisplayName;

            //}
            //catch (Exception ex)
            //{
            //    Results = "An exception occurred: " + ex.Message;

            //}

        }
        private void btnRemoveNoisies_Click(object sender, RoutedEventArgs e)
        {
            List<Marker> markerList = new List<Marker>();
            markerList.Add(new Marker(myKmlObject.Markers[0].GetLocation()));
            markerList.Add(new Marker(myKmlObject.Markers[1].GetLocation()));
            markerList.Add(new Marker(myKmlObject.Markers[2].GetLocation()));
            markerList.Add(new Marker(myKmlObject.Markers[3].GetLocation()));
            markerList.Add(new Marker(myKmlObject.Markers[4].GetLocation()));
            for (int i = 0; i < myKmlObject.Markers.Count - 5; i++)
            {
                double avgLat = 0;
                double avgLong = 0;
                #region Gettting five places lat and long

                avgLat = markerList[markerList.Count - 5].Coordinate.Latitude +
                    markerList[markerList.Count - 4].Coordinate.Latitude +
                    markerList[markerList.Count - 3].Coordinate.Latitude +
                    markerList[markerList.Count - 2].Coordinate.Latitude +
                    markerList[markerList.Count - 1].Coordinate.Latitude;

                avgLong = markerList[markerList.Count - 5].Coordinate.Longitude +
                    markerList[markerList.Count - 4].Coordinate.Longitude +
                    markerList[markerList.Count - 3].Coordinate.Longitude +
                    markerList[markerList.Count - 2].Coordinate.Longitude +
                    markerList[markerList.Count - 1].Coordinate.Longitude;

                Microsoft.Maps.MapControl.WPF.Location centerLocation = 
                    new Microsoft.Maps.MapControl.WPF.Location(avgLat, avgLong);

                double d1 = GetDistance(markerList[markerList.Count - 5].GetLocation(), centerLocation);
                double d2 = GetDistance(markerList[markerList.Count - 4].GetLocation(), centerLocation);
                double d3 = GetDistance(markerList[markerList.Count - 3].GetLocation(), centerLocation);
                double d4 = GetDistance(markerList[markerList.Count - 2].GetLocation(), centerLocation);
                double d5 = GetDistance(markerList[markerList.Count - 1].GetLocation(), centerLocation);

                double stdVarience = (d1 + d2 + d3 + d4 + d5) / 5;

                double newLocationDistance = GetDistance(myKmlObject.Markers[i+5].GetLocation(), centerLocation);

                if (newLocationDistance > (stdVarience*1.000005))
                {
                    continue;
                }
                markerList.Add(new Marker(myKmlObject.Markers[i + 5].GetLocation()));
                #endregion
            }
            MyKml tempKmlObject = new MyKml();
            tempKmlObject.Markers = markerList;
            ShowTracks(tempKmlObject, new SolidColorBrush(Colors.Yellow));
        }

        public double GetDistance(Microsoft.Maps.MapControl.WPF.Location location1, Microsoft.Maps.MapControl.WPF.Location location2)
        {
            double latitudeDifference = Math.Abs(location1.Latitude - location2.Latitude);
            double longitudeDifference = Math.Abs(location1.Longitude - location2.Longitude);
            return Math.Sqrt(latitudeDifference * latitudeDifference + longitudeDifference * longitudeDifference);
        }

        private void MedianBySpeed_Click(object sender, RoutedEventArgs e)
        {
            ////myKmlObject = MedianSpeed(myKmlObject);
            ////ShowTracks(myKmlObject);
            //for (int i = 5; i < myKmlObject.Markers.Count; i++)
            //{
            //    double avgSpeedChanging = (myKmlObject.Markers[i - 1].Coordinate.Speed - myKmlObject.Markers[i - 5].Coordinate.Speed) / 4;
            //    if (double.IsNaN((myKmlObject.Markers[i].Coordinate.Speed)))
            //    {
            //        myKmlObject.Markers.RemoveAt(i);
            //        continue;
            //    }
            //    if ((myKmlObject.Markers[i].Coordinate.Speed - myKmlObject.Markers[i - 1].Coordinate.Speed) > avgSpeedChanging * 2)
            //    {
            //        List<Marker> tempMarkerList = new List<Marker>();
            //        tempMarkerList.Add(myKmlObject.Markers[i - 1]);
            //        tempMarkerList.Add(myKmlObject.Markers[i - 2]);
            //        tempMarkerList.Add(myKmlObject.Markers[i - 3]);
            //        tempMarkerList.Add(myKmlObject.Markers[i - 4]);
            //        tempMarkerList.Add(myKmlObject.Markers[i - 5]);
            //        double newSpeedValue = LRLFS(tempMarkerList);

            //        Marker newMarker = new Marker();
            //        newMarker.Coordinate.Latitude = myKmlObject.Markers[i].Coordinate.Latitude;
            //        newMarker.Coordinate.Longitude = myKmlObject.Markers[i].Coordinate.Longitude;
            //        newMarker.Coordinate.Altitude = myKmlObject.Markers[i].Coordinate.Altitude;
            //        newMarker.Coordinate.Speed = Math.Abs(newSpeedValue);
            //        myKmlObject.Markers[i] = newMarker;
            //    }
            //}

            //ShowTracks(myKmlObject, new SolidColorBrush(Colors.Red));
            //TextWriter tw = new StreamWriter("speed.txt");
            //for (int i = 0; i < myKmlObject.Markers.Count; i++)
            //{
            //    tw.WriteLine(myKmlObject.Markers[i].Coordinate.Speed);
            //}
            //tw.Close();
        }
        public double LRLFS(MyKml myKmlObject)
        {
            double[] lrlfs = new double[myKmlObject.Markers.Count];
            double a1 = 0;
            double a2 = 0;
            double a3 = 0;
            double a4 = 0;
            double a5 = 0;
            double a6 = 0;
            for (int i = 0; i < myKmlObject.Markers.Count; i++)
            {
                lrlfs[i] = myKmlObject.Markers[i].Coordinate.Speed;
                a1 += lrlfs[i];
                a2 += i*i;
                a3 += i;
                a4 += i*lrlfs[i];
                a5 = a2;
                a6 += i*i;
            }
            double a = (a1*a2 - a3*a4) / (lrlfs.Length*a5 - a6);
            double b = (lrlfs.Length * a4 - a3 * a1) / (lrlfs.Length * a5 - a6);
            double s = a * lrlfs.Length + b;
            return s;
        }
        public MyKml MedianSpeed(MyKml myKmlObject)
        {
            double avgSpeedTolerance = 2;
            double avgSpeedMultiplier = 0.5;
            for (int i = 5; i < myKmlObject.Markers.Count; i++)
            {
                double avgSpeed = (myKmlObject.Markers[i - 1].Coordinate.Speed - myKmlObject.Markers[i - 5].Coordinate.Speed) / 4;
                double speedChanging = myKmlObject.Markers[i].Coordinate.Speed - myKmlObject.Markers[i - 1].Coordinate.Speed;
                if ((speedChanging > 0 && avgSpeed > 0 && speedChanging > avgSpeed * avgSpeedTolerance))
                {
                    myKmlObject.Markers[i].Coordinate.Speed = myKmlObject.Markers[i - 1].Coordinate.Speed + avgSpeed * avgSpeedMultiplier;
                }
                else if ((speedChanging > 0 && avgSpeed > 0 && speedChanging < avgSpeed * avgSpeedTolerance))
                {
                    myKmlObject.Markers[i].Coordinate.Speed = myKmlObject.Markers[i - 1].Coordinate.Speed - avgSpeed * avgSpeedMultiplier;
                }
                else if (speedChanging > 0 && avgSpeed < 0)
                {
                    myKmlObject.Markers[i].Coordinate.Speed = myKmlObject.Markers[i - 1].Coordinate.Speed + Math.Abs(avgSpeed * avgSpeedMultiplier);
                }
                else if (speedChanging < 0 && avgSpeed > 0)
                {
                    myKmlObject.Markers[i].Coordinate.Speed = myKmlObject.Markers[i - 1].Coordinate.Speed + Math.Abs(avgSpeed * avgSpeedMultiplier);
                }
            }
            TextWriter tw = new StreamWriter("speed.txt");
            double max = 0;
            for (int i = 0; i < myKmlObject.Markers.Count; i++)
            {
                tw.WriteLine(myKmlObject.Markers[i].Coordinate.Speed);
                if (max < myKmlObject.Markers[i].Coordinate.Speed)
                {
                    max = myKmlObject.Markers[i].Coordinate.Speed;
                }
            }
            MessageBox.Show(max + "");
            tw.Close();
            return myKmlObject;
        }
        
    }
}
