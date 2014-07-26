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
using Microsoft.Maps.MapControl.WPF;
using System.Text.RegularExpressions;
using CSharpQuadTree;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for GeoLocationTest.xaml
    /// </summary>
    public partial class GeoLocationTest : Window
    {
        public Location nw = new Location(90.0, 0.0);
        public Location sw = new Location(0.0, 0.0);
        public Location se = new Location(0.0, 180.0);
        public Location ne = new Location(90.0, 180.0);
        CSharpQuadTree.QuadTree<Ellipse> quadTree;
        private List<Address> clickedAddressList = new List<Address>();
        public GeoLocationTest()
        {
            InitializeComponent();
            quadTree = new CSharpQuadTree.QuadTree<Ellipse>(new Size(1, 1), 2, true); // 10 bölünme kriteridir
        }

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition(MyMap);
            Location mousePointLocation = MyMap.ViewportPointToLocation(mousePosition);
            List<WpfApplication3.GeocodeService.GeocodeResult> geocodeResults = 
                GetAddress.MakeGeocodeRequestForAddress(mousePointLocation);
            Address clickedAddress = Address.GeocodeResultConverter(geocodeResults);
            clickedAddress.AddressLocation = mousePointLocation;
            clickedAddressList.Add(clickedAddress);
            //SolidColorBrush itemBrush = Brushes.White;
            SolidColorBrush itemBrush = GenerateRandomColoredBrush(new Random());
            AddAddressItemToListBox(clickedAddress, itemBrush);
            AddPushPinToAddress(clickedAddress, itemBrush);
            // mouseClick(e)
            mouseClick(e);
        }

        private void AddAddressItemToListBox(Address clickedAddress, SolidColorBrush listBoxItemBrush)
        {
            ListBoxItem addressItem = new ListBoxItem();
            addressItem.Content = clickedAddress;
            addressItem.Background = listBoxItemBrush;
            addressItem.Foreground = new SolidColorBrush(Colors.White);
            lstBoxAddresses.Items.Add(addressItem);
        }

        private void AddPushPinToAddress(Address clickedAddress, SolidColorBrush pushpinBrush)
        {
            Pushpin p = new Pushpin();
            p.Content = clickedAddressList.Count;
            Random r = new Random();
            p.Background = pushpinBrush;
            p.Foreground = new SolidColorBrush(Colors.White);
            p.ToolTip = clickedAddress.ToString() + "  (" + 
                Math.Round(clickedAddress.AddressLocation.Latitude , 4).ToString() + ","+
                Math.Round(clickedAddress.AddressLocation.Longitude, 4).ToString() + ")";
            mapPushPinLayer.AddChild(p, clickedAddress.AddressLocation);
        }

        private SolidColorBrush GenerateRandomColoredBrush(Random r)
        {
            SolidColorBrush randomColoredBrush = new SolidColorBrush(Color.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255)));
            return randomColoredBrush;
            //return Brushes.White;
        }

        private SolidColorBrush GenerateInvertedColoredBrush(SolidColorBrush brush)
        {
            const byte RGBMax = 255;
            return new SolidColorBrush(Color.FromRgb((byte)(RGBMax - brush.Color.R), (byte)(RGBMax - brush.Color.G), (byte)(RGBMax - brush.Color.B)));
        }

        public void mouseClick(MouseButtonEventArgs e)
        {
            Point mouseClickPoint = e.GetPosition(MyMap);
            float x = (float)Math.Min(mouseClickPoint.X, mouseClickPoint.X);
            float y = (float)Math.Min(mouseClickPoint.Y, mouseClickPoint.Y);
            var newBounds = new Rect(x, y, 5, 5);
            var ellipse = new Ellipse(mouseClickPoint, newBounds);
            if (quadTree.Root == null)
            {
                var firstBounds = new Rect(0, 0, myCanvas.ActualWidth, myCanvas.ActualHeight);
                ellipse = new Ellipse(mouseClickPoint, firstBounds);
                quadTree.Insert(ellipse);
                ellipse = new Ellipse(mouseClickPoint, newBounds);
            }
            quadTree.Insert(ellipse);
            //createEllipse(ellipse, mouseClickPoint);
            createRectangles();
        }

        public void mapMouseClick(Location e)
        {
            var newBounds = new Rect(e.Latitude, e.Longitude, 180, 90);
            var ellipse = new Ellipse(new Point(e.Latitude, e.Longitude), newBounds);
            quadTree.Insert(ellipse);
            //createEllipse(ellipse, mouseClickPoint);
            createMapRectangles();
        }

        private void createEllipse(Ellipse ellipse, Point ellipseLocation)
        {
            Random r = new Random();
            System.Windows.Shapes.Ellipse e = new System.Windows.Shapes.Ellipse()
            {
                Stroke= Brushes.Black,
                StrokeThickness = 1,
                Width = 10,
                Height = 10,
                Fill = Brushes.Black,
                //Fill = new SolidColorBrush(Color.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255))),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            mapShapeLayer.AddChild(e, MyMap.ViewportPointToLocation(ellipseLocation));
        }

        private void createMapRectangles()
        {
            List<CSharpQuadTree.QuadTree<Ellipse>.QuadNode> nodeList = quadTree.GetAllNodes();
            foreach (CSharpQuadTree.QuadTree<Ellipse>.QuadNode node in nodeList)
            {
                if (nodeList.Count == 1)
                {
                    MapPolygon mp = new MapPolygon();
                    mp.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                    mp.StrokeThickness = 5;
                    mp.Opacity = 1;
                    mp.Locations = new LocationCollection()
                    {
                        nw,sw,se,ne
                    };
                    mapShapeLayer.Children.Add(mp);
                }
                else
                {
                    int a = 2;
                }
            }
        }

        private void createRectangles()
        {
            myCanvas.Children.Clear();
            List<CSharpQuadTree.QuadTree<Ellipse>.QuadNode> nodeList = quadTree.GetAllNodes();
            foreach (CSharpQuadTree.QuadTree<Ellipse>.QuadNode node in nodeList)
                {
                    Rectangle r = new Rectangle()
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Width = node.Bounds.Width,
                        Height = node.Bounds.Height,
                        Margin = new Thickness(node.Bounds.X, node.Bounds.Y, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    myCanvas.Children.Add(r);
                    //mapShapeLayer.AddChild(r, nw);
                    //if (nodeList.Count != 1)
                    //{

                    //}
                    //else
                    //{
                    //    nw = new Location(nw.Latitude / 2, nw.Longitude / 2);
                    //    sw = new Location(sw.Latitude / 2, sw.Longitude / 2);
                    //    se = new Location(se.Latitude / 2, se.Longitude / 2);
                    //    ne = new Location(ne.Latitude / 2, ne.Longitude / 2);
                    //}

                    //MapPolygon r = new MapPolygon()
                    //{
                    //    Locations = new LocationCollection()
                    //{
                    //    nw, sw, se,ne
                    //},
                    //    Stroke = Brushes.Black,
                    //    StrokeThickness = 1
                    //};
                    //mapPolygons.Add(node.Bounds);

                    
                }


        }
    }
    public class Ellipse : IQuadObject
    {

        public Point ellipseLocation;

        public Ellipse(Point ellipseLocation, Rect rect)
        {
            this.Rect = rect;
            this.ellipseLocation = ellipseLocation;
        }

        public Brush Brush;
        public Rect Rect;

        public Rect Bounds
        {
            get { return Rect; }
        }

        private void RaiseBoundsChanged()
        {
            EventHandler handler = BoundsChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }

        public void MoveRight(float dx)
        {
            Rect.X += dx;
            RaiseBoundsChanged();
        }

        public void MoveLeft(float dx)
        {
            Rect.X -= dx;
            RaiseBoundsChanged();
        }

        public void MoveUp(float dy)
        {
            Rect.Y -= dy;
            RaiseBoundsChanged();
        }

        public void MoveDown(float dy)
        {
            Rect.Y += dy;
            RaiseBoundsChanged();
        }

        public event EventHandler BoundsChanged;

        public void Move()
        {
            RaiseBoundsChanged();
        }
    }
}
