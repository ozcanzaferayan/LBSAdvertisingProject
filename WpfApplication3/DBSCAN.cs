//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Maps.MapControl.WPF;
//using ReadKmlFiles;

//namespace WpfApplication3
//{
//    public class DBSCAN
//    {
//        public const int NOISE = -1;
//        public const int UNCLASSIFIED = 0;
//        public int X, Y, ClusterId;

//        public List<Marker> GetClusters(MyKml myKmlObject, double eps, int minPts)
//        {
//            if (myKmlObject.Markers.Count == 0) return null;
//            List<Marker> clusters = new List<Marker>();
//            eps *= eps; // square eps
//            int clusterId = 1;
//            for (int i = 0; i < myKmlObject.Markers.Count; i++)
//            {
//                Marker m = myKmlObject.Markers[i];
//                if (m.DBSCANClusterType == DBSCAN.UNCLASSIFIED)
//                {
//                    if (ExpandCluster(myKmlObject.Markers, m, clusterId, eps, minPts)) clusterId++;
//                }
//            }
//            // sort out points into their clusters, if any
//            int maxClusterId = myKmlObject.Markers.OrderBy(p => p.DBSCANClusterType).Last().DBSCANClusterType;
//            if (maxClusterId < 1) return null; // no clusters, so list is empty
//            for (int i = 0; i < maxClusterId; i++) clusters.Add(new Marker());
//            foreach (Marker m in myKmlObject.Markers)
//            {
//                if (m.DBSCANClusterType > 0) clusters[m.DBSCANClusterType - 1] = m;
//            }
//            return clusters;
//        }

//        public List<Marker> GetRegion(List<Marker> markers, Marker p, double eps)
//        {
//            List<Marker> region = new List<Marker>();
//            for (int i = 0; i < markers.Count - 1; i++)
//            {
//                double distSquared = markers[i+1].Coordinate.GetDistanceTo(p.Coordinate);
//                if (distSquared <= eps) region.Add(markers[i]);
//            }
//            return region;
//        }
//        public bool ExpandCluster(List<Marker> markers, Marker m, int clusterId, double eps, int minPts)
//        {
//            List<Marker> seeds = GetRegion(markers, m, eps);
//            if (seeds.Count < minPts) // no core point
//            {
//                m.DBSCANClusterType = DBSCAN.NOISE;
//                return false;
//            }
//            else // all points in seeds are density reachable from point 'p'
//            {
//                for (int i = 0; i < seeds.Count; i++) seeds[i].DBSCANClusterType = clusterId;
//                seeds.Remove(m);
//                while (seeds.Count > 0)
//                {
//                    Marker currentMarker = seeds[0];
//                    List<Marker> result = GetRegion(markers, currentMarker, eps);
//                    if (result.Count >= minPts)
//                    {
//                        for (int i = 0; i < result.Count; i++)
//                        {
//                            Marker resultP = result[i];
//                            if (resultP.DBSCANClusterType == DBSCAN.UNCLASSIFIED || resultP.ClusterType == DBSCAN.NOISE)
//                            {
//                                if (resultP.DBSCANClusterType == DBSCAN.UNCLASSIFIED) seeds.Add(resultP);
//                                resultP.DBSCANClusterType = clusterId;
//                            }
//                        }
//                    }
//                    seeds.Remove(currentMarker);
//                }
//                return true;
//            }
//        }
//    }
//}

using ReadKmlFiles;
using System;
using System.Collections.Generic;
using System.Linq;
    public class DBSCAN
    {
        public List<List<Marker>> GetClusters(List<Marker> points, double eps, int minPts)
        {
            if (points == null) return null;
            List<List<Marker>> clusters = new List<List<Marker>>();
            eps *= eps;
            int clusterID = 1;
            for (int i = 0; i < points.Count; i++)
            {
                Marker p = points[i];
                if (p.DBSCANClusterType == Marker.UNCLASSIFIED)
                {
                    if (ExpandCluster(points, p, clusterID, eps, minPts)) clusterID++;
                }
            }
                // sort out points into their clusters, if any
                int maxClusterId = points.OrderBy(p => p.DBSCANClusterType).Last().DBSCANClusterType;
                if (maxClusterId < 1) return clusters; // no clusters, so list is empty
                for (int i = 0; i < maxClusterId; i++) clusters.Add(new List<Marker>());
                foreach (Marker p in points)
                {
                    if (p.DBSCANClusterType > 0) clusters[p.DBSCANClusterType - 1].Add(p);
                }
                return clusters;
        }
        public static double DistanceSquared(Marker p1, Marker p2)
        {
            double diffX = p2.Coordinate.Latitude - p1.Coordinate.Latitude;
            double diffY = p2.Coordinate.Longitude - p1.Coordinate.Longitude;
            return diffX * diffX + diffY * diffY;
        }
        static List<Marker> GetRegion(List<Marker> points, Marker p, double eps)
        {
            List<Marker> region = new List<Marker>();
            for (int i = 0; i < points.Count; i++)
            {
                double distSquared = DBSCAN.DistanceSquared(p, points[i]);
                if (distSquared <= eps) region.Add(points[i]);
            }
            return region;
        }
        static bool ExpandCluster(List<Marker> points, Marker p, int clusterId, double eps, int minPts)
        {
            List<Marker> seeds = GetRegion(points, p, eps);
            if (seeds.Count < minPts) // no core point
            {
                p.DBSCANClusterType = Marker.NOISE;
                return false;
            }
            else // all points in seeds are density reachable from point 'p'
            {
                for (int i = 0; i < seeds.Count; i++) seeds[i].DBSCANClusterType = clusterId;
                seeds.Remove(p);
                while (seeds.Count > 0)
                {
                    Marker currentP = seeds[0];
                    List<Marker> result = GetRegion(points, currentP, eps);
                    if (result.Count >= minPts)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            Marker resultP = result[i];
                            if (resultP.DBSCANClusterType == Marker.UNCLASSIFIED || resultP.DBSCANClusterType == Marker.NOISE)
                            {
                                if (resultP.DBSCANClusterType == Marker.UNCLASSIFIED) seeds.Add(resultP);
                                resultP.DBSCANClusterType = clusterId;
                            }
                        }
                    }
                    seeds.Remove(currentP);
                }
                return true;
            }
    }

//public class DBSCAN
//{
//    public List<List<Point>> GetClusters(List<Point> points, double eps, int minPts)
//    {
//        if (points == null) return null;
//        List<List<Point>> clusters = new List<List<Point>>();
//        eps *= eps; // square eps
//        int clusterId = 1;
//        for (int i = 0; i < points.Count; i++)
//        {
//            Point p = points[i];
//            if (p.ClusterId == Point.UNCLASSIFIED)
//            {
//                if (ExpandCluster(points, p, clusterId, eps, minPts)) clusterId++;
//            }
//        }
//        // sort out points into their clusters, if any
//        int maxClusterId = points.OrderBy(p => p.ClusterId).Last().ClusterId;
//        if (maxClusterId < 1) return clusters; // no clusters, so list is empty
//        for (int i = 0; i < maxClusterId; i++) clusters.Add(new List<Point>());
//        foreach (Point p in points)
//        {
//            if (p.ClusterId > 0) clusters[p.ClusterId - 1].Add(p);
//        }
//        return clusters;
//    }
//    static List<Point> GetRegion(List<Point> points, Point p, double eps)
//    {
//        List<Point> region = new List<Point>();
//        for (int i = 0; i < points.Count; i++)
//        {
//            double distSquared = Point.DistanceSquared(p, points[i]);
//            if (distSquared <= eps) region.Add(points[i]);
//        }
//        return region;
//    }
//    static bool ExpandCluster(List<Point> points, Point p, int clusterId, double eps, int minPts)
//    {
//        List<Point> seeds = GetRegion(points, p, eps);
//        if (seeds.Count < minPts) // no core point
//        {
//            p.ClusterId = Point.NOISE;
//            return false;
//        }
//        else // all points in seeds are density reachable from point 'p'
//        {
//            for (int i = 0; i < seeds.Count; i++) seeds[i].ClusterId = clusterId;
//            seeds.Remove(p);
//            while (seeds.Count > 0)
//            {
//                Point currentP = seeds[0];
//                List<Point> result = GetRegion(points, currentP, eps);
//                if (result.Count >= minPts)
//                {
//                    for (int i = 0; i < result.Count; i++)
//                    {
//                        Point resultP = result[i];
//                        if (resultP.ClusterId == Point.UNCLASSIFIED || resultP.ClusterId == Point.NOISE)
//                        {
//                            if (resultP.ClusterId == Point.UNCLASSIFIED) seeds.Add(resultP);
//                            resultP.ClusterId = clusterId;
//                        }
//                    }
//                }
//                seeds.Remove(currentP);
//            }
//            return true;
//        }
//    }
    //    public Point(double x, double y)
    //    {
    //        this.X = x;
    //        this.Y = y;
    //    }
    //    public static double DistanceSquared(Point p1, Point p2)
    //    {
    //        double diffX = p2.X - p1.X;
    //        double diffY = p2.Y - p1.Y;
    //        return diffX * diffX + diffY * diffY;
    //    }
    //    public override string ToString()
    //    {
    //        return this.ClusterId + " " + this.X + " " + this.Y;
    //    }
}
