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
namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for QuadTreeTest.xaml
    /// </summary>
    public partial class QuadTreeTest : Window
    {
        private Point mouseClickPoint;
        QuadTree<Ellipse> quadTree;
        public List<RectangleGeometry> rList;
        public QuadTreeTest()
        {
            InitializeComponent();
            // Buradaki size değeri her hücrenin boundary'nin en fazla alanı oluyor
            quadTree = new QuadTree<Ellipse>(new Size(1, 1), 0, true);
            rList = new List<RectangleGeometry>();
        }

        private void myCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseClickPoint = e.GetPosition(myCanvas);
            float x = (float)Math.Min(mouseClickPoint.X, mouseClickPoint.X);
            float y = (float)Math.Min(mouseClickPoint.Y, mouseClickPoint.Y);
            var newBounds = new Rect(x, y, 5, 5);
            if (quadTree.GetQuadObjectCount() == 0)
            {
                newBounds = new Rect(0, 0, myCanvas.ActualWidth, myCanvas.ActualHeight);
            }
            var ellipse = new Ellipse(mouseClickPoint, newBounds);
            quadTree.Insert(ellipse);
            createEllipse(ellipse, mouseClickPoint);
            createRectangles();
            myListBox.Items.Add("(" + mouseClickPoint.X + "," + mouseClickPoint.Y + ")");
        }

        private void createEllipse(Ellipse ellipse, Point ellipseLocation)
        {
            Random r = new Random();
            System.Windows.Shapes.Ellipse e = new System.Windows.Shapes.Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Color.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255))),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            }; 
            myCanvas.Children.Add(e);
            Canvas.SetLeft(e, ellipseLocation.X);
            Canvas.SetTop(e, ellipseLocation.Y);
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
            }
        }
    }

}
