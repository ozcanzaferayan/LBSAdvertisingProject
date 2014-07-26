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
using CSharpQuadTree;
using Microsoft.Maps.MapControl.WPF;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for QuadTreeMap.xaml
    /// </summary>
    public partial class QuadTreeMap : Window
    {
        Location nw=null;
        Location sw=null;
        Location se=null;
        Location ne = null;
        CSharpQuadTree.QuadTree<Ellipse> quadTree;
        List<Rect> mapPolygons;
        public QuadTreeMap()
        {
            InitializeComponent();
            quadTree = new CSharpQuadTree.QuadTree<Ellipse>(new Size(1, 1), 1, true);
            mapPolygons = new List<Rect>();
            nw = new Location(90, -180);
            sw = new Location(-90, -180);
            se = new Location(-90, 180);
            ne = new Location(90, 180);
        }

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mouseClickPoint = e.GetPosition(MyMap);
            float x = (float)Math.Min(mouseClickPoint.X, mouseClickPoint.X);
            float y = (float)Math.Min(mouseClickPoint.Y, mouseClickPoint.Y);
            var newBounds = new Rect(x, y, 5, 5);
            if (quadTree.GetQuadObjectCount() == 0)
            {
                newBounds = new Rect( 0, 0, myCanvas.ActualWidth, myCanvas.ActualHeight);
            }
            var ellipse = new Ellipse(mouseClickPoint, newBounds);
            quadTree.Insert(ellipse);
            createEllipse(ellipse, mouseClickPoint);
            createRectangles();
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
                Fill = new SolidColorBrush(Color.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255))),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            mapShapeLayer.AddChild(e, MyMap.ViewportPointToLocation(ellipseLocation));
        }

        private void createRectangles()
        {
            List<QuadTree<Ellipse>.QuadNode> nodeList = quadTree.GetAllNodes();
                foreach (QuadTree<Ellipse>.QuadNode node in nodeList)
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
}
