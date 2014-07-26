using System;

namespace ReadKmlFiles
{
    class ClusteringKMeansProgram
    {
        static void ShowMain(Marker[] markers)
        {
            try
            {
                //Console.WriteLine("\nBegin outlier data detection using k-means clustering demo\n");

                //Console.WriteLine("Loading all (height-weight) data into memory");
                //string[] attributes = new string[] { "Height", "Weight" };
                string[] attributes = new string[] { "Longitute", "Latitude", "Altitude", "Speed", "DateTime", "TypeOfMovement" };
                //double[][] rawData = new double[20][];  // in most cases data will be in a text file or SQl table
                Marker[] m = markers;
                //rawData[0] = new double[] { 65.0, 220.0 };  // if data won't fit into memory, stream through external storage
                //rawData[1] = new double[] { 73.0, 160.0 };
                //rawData[2] = new double[] { 59.0, 110.0 };
                //rawData[3] = new double[] { 61.0, 120.0 };
                //rawData[4] = new double[] { 75.0, 150.0 };
                //rawData[5] = new double[] { 67.0, 240.0 };
                //rawData[6] = new double[] { 68.0, 230.0 };
                //rawData[7] = new double[] { 70.0, 220.0 };
                //rawData[8] = new double[] { 62.0, 130.0 };
                //rawData[9] = new double[] { 66.0, 210.0 };
                //rawData[10] = new double[] { 77.0, 190.0 };
                //rawData[11] = new double[] { 75.0, 180.0 };
                //rawData[12] = new double[] { 74.0, 170.0 };
                //rawData[13] = new double[] { 70.0, 210.0 };
                //rawData[14] = new double[] { 61.0, 110.0 };
                //rawData[15] = new double[] { 58.0, 100.0 };
                //rawData[16] = new double[] { 66.0, 230.0 };
                //rawData[17] = new double[] { 59.0, 120.0 };
                //rawData[18] = new double[] { 68.0, 210.0 };
                //rawData[19] = new double[] { 61.0, 130.0 };

                //Console.WriteLine("\nRaw data:\n");
                //ShowMatrix(rawData, rawData.Length, true);

                int numAttributes = attributes.Length;  // 2 in this demo (height,weight)
                int numClusters = 10;  // vary this to experiment (must be between 2 and number data tuples)
                int maxCount = 30;  // trial and error

                //Console.WriteLine("\nBegin clustering data with k = " + numClusters + " and maxCount = " + maxCount);
                //int[] clustering = Cluster(markers, numClusters, numAttributes, maxCount, arraySize);
                //Console.WriteLine("\nClustering complete");

                //Console.WriteLine("\nClustering in internal format: \n");
                //ShowVector(clustering, true);  // true -> newline after display

                //Console.WriteLine("\nClustered data:");
                //ShowClustering(rawData, numClusters, clustering, true);

                //Marker outlier = Outlier(markers, clustering, numClusters, 0);
                //Console.WriteLine("Outlier for cluster 0 is:");
                //ShowVector(outlier, true);

                //Console.WriteLine("\nEnd demo\n");
                //Console.ReadLine();
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
                //Console.ReadLine();
            }
        } // Main


        static int[] InitClustering(int numTuples, int numClusters, int randomSeed)
        {
            // assign each tuple to a random cluster, making sure that there's at least
            // one tuple assigned to every cluster
            Random random = new Random(randomSeed);
            int[] clustering = new int[numTuples];

            // assign first numClusters tuples to clusters 0..k-1
            for (int i = 0; i < numClusters; ++i)
                clustering[i] = i;
            // assign rest randomly
            for (int i = numClusters; i < clustering.Length; ++i)
                clustering[i] = random.Next(0, numClusters);
            return clustering;
        }

        static Marker[] Allocate(int numClusters, int numAttributes)
        {
            // helper allocater for means[][] and centroids[][]
            Marker[] result = new Marker[numClusters];
            for (int k = 0; k < numClusters; ++k)
                result[k] = new Marker();
            return result;
        }

        static void UpdateMeans(Marker[] marker, int[] clustering, Marker[] means, int arraySize)
        {
            //double var1 = 0, var2 = 0, var3 = 0, var4 = 0;
            double[] v1 = new double[arraySize];
            double[] v2 = new double[arraySize];
            double[] v3 = new double[arraySize];
            // assumes means[][] exists. consider making means[][] a ref parameter
            int numClusters = means.Length;
            // zero-out means[][]
            for (int k = 0; k < means.Length; ++k)
                for (int j = 0; j < means.Length; ++j)
                    means[k] = new Marker();


            // make an array to hold cluster counts
            int[] clusterCounts = new int[numClusters];

            // walk through each tuple, accumulate sum for each attribute, update cluster count
            for (int i = 0; i < marker.Length; ++i)
            {
                int cluster = clustering[i];
                ++clusterCounts[cluster];

                //for (int j = 0; j < marker.Length; ++j)
                //{
                    v1[cluster] += Convert.ToDouble(marker[i].Coordinate.Latitude.ToString());
                    v2[cluster] += Convert.ToDouble(marker[i].Coordinate.Longitude.ToString());
                    v3[cluster] += Convert.ToDouble(marker[i].Coordinate.Altitude.ToString());
                //}

                    
                    //means[cluster] = Marker.Add(marker[j], means[cluster]);
            }

            // divide each attribute sum by cluster count to get average (mean)
            for (int k = 0; k < means.Length; ++k)
            {
                v1[k] /= clusterCounts[k];  // will throw if count is 0. consider an error-check
                v2[k] /= clusterCounts[k];
                v3[k] /= clusterCounts[k];
            }
            for (int k = 0; k < means.Length; ++k)
            {
                means[k] = new Marker();
                means[k].Coordinate.Latitude = v1[k];
                means[k].Coordinate.Longitude = v2[k];
                means[k].Coordinate.Altitude = v3[k];  // will throw if count is 0. consider an error-check
            }   
            return;
        } // UpdateMeans

        static Marker ComputeCentroid(Marker[] markers, int[] clustering, int cluster, Marker[] means)
        {
            // the centroid is the actual tuple values that are closest to the cluster mean
            int numAttributes = means.Length;
            Marker centroid = new Marker();
            double minDist = double.MaxValue;
            for (int i = 0; i < markers.Length; ++i) // walk thru each data tuple
            {
                int c = clustering[i];  // if curr tuple isn't in the cluster we're computing for, continue on
                if (c != cluster) continue;

                double currDist = Distance(markers[i], means[cluster]);  // call helper
                if (currDist < minDist)
                {
                    minDist = currDist;
                    centroid = markers[i];
                    //for (int j = 0; j < cen; ++j)
                    //  centroid[j] = markers[i];
                }
            }
            return centroid;
        }

        static void UpdateCentroids(Marker[] markers, int[] clustering, Marker[] means, Marker[] centroids)
        {
            // updates all centroids by calling helper that updates one centroid
            for (int k = 0; k < centroids.Length; ++k)
            {
                Marker centroid = ComputeCentroid(markers, clustering, k, means);
                centroids[k] = centroid;
            }
        }

        static double Distance(Marker m1, Marker m2)
        {
            return m1.Coordinate.GetDistanceTo(m2.Coordinate);

            //// Euclidean distance between an actual data tuple and a cluster mean or centroid
            //double sumSquaredDiffs = 0.0;
            //for (int j = 0; j < tuple.Length; ++j)
            //  sumSquaredDiffs += Math.Pow((tuple[j] - vector[j]), 2);
            //return Math.Sqrt(sumSquaredDiffs); 
        }

        static int MinIndex(double[] distances)
        {
            // index of smallest value in distances[]
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k]; indexOfMin = k;
                }
            }
            return indexOfMin;
        }

        static bool Assign(Marker[] rawData, int[] clustering, Marker[] centroids)
        {
            // assign each tuple to best cluster (closest to cluster centroid)
            // return true if any new cluster assignment is different from old/curr cluster
            // does not prevent a state where a cluster has no tuples assigned. see article for details
            int numClusters = centroids.Length;
            bool changed = false;

            double[] distances = new double[numClusters]; // distance from curr tuple to each cluster mean
            for (int i = 0; i < rawData.Length; ++i)      // walk thru each tuple
            {
                for (int k = 0; k < numClusters; ++k)       // compute distances to all centroids
                    distances[k] = Distance(rawData[i], centroids[k]);

                int newCluster = MinIndex(distances);  // find the index == custerID of closest 
                if (newCluster != clustering[i]) // different cluster assignment?
                {
                    changed = true;
                    clustering[i] = newCluster;
                } // else no change
            }
            return changed; // was there any change in clustering?
        } // Assign

        public static int[] Cluster(Marker[] marker, int numClusters, int numAttributes, int maxCount, int arraySize)
        {
            bool changed = true;
            int ct = 0;

            int numTuples = marker.Length;
            int[] clustering = InitClustering(numTuples, numClusters, 0);  // 0 is a seed for random
            Marker[] means = Allocate(numClusters, numAttributes);       // just makes things a bit cleaner
            Marker[] centroids = Allocate(numClusters, numAttributes);
            UpdateMeans(marker, clustering, means, arraySize);                       // could call this inside UpdateCentroids instead
            UpdateCentroids(marker, clustering, means, centroids);

            while (changed == true && ct < maxCount)
            {
                ++ct;
                changed = Assign(marker, clustering, centroids); // use centroids to update cluster assignment
                UpdateMeans(marker, clustering, means, arraySize);  // use new clustering to update cluster means
                UpdateCentroids(marker, clustering, means, centroids);  // use new means to update centroids
            }
            //ShowMatrix(centroids, centroids.Length, true);  // show the final centroids for each cluster
            return clustering;
        }

        static Marker Outlier(Marker[] markers, int[] clustering, int numClusters, int cluster, int arraySize)
        {
            // return the tuple values in cluster that is farthest from cluster centroid
            int numAttributes = 6;

            Marker outlier = new Marker();
            double maxDist = 0.0;

            Marker[] means = Allocate(numClusters, numAttributes);
            Marker[] centroids = Allocate(numClusters, numAttributes);
            UpdateMeans(markers, clustering, means, arraySize);
            UpdateCentroids(markers, clustering, means, centroids);

            for (int i = 0; i < markers.Length; ++i)
            {
                int c = clustering[i];
                if (c != cluster) continue;
                double dist = Distance(markers[i], centroids[cluster]);
                if (dist > maxDist)
                {
                    maxDist = dist;  // might also want to return (as an out param) the index of rawData
                    outlier = markers[i];
                }
            }
            return outlier;
        }

        // display routines below

        //static void ShowMatrix(double[][] matrix, int numRows, bool newLine)
        //{
        //    for (int i = 0; i < numRows; ++i)
        //    {
        //        Console.Write("[" + i.ToString().PadLeft(2) + "]  ");
        //        for (int j = 0; j < matrix[i].Length; ++j)
        //            Console.Write(matrix[i][j].ToString("F1") + "  ");
        //        Console.WriteLine("");
        //    }
        //    if (newLine == true) Console.WriteLine("");
        //} // ShowMatrix

        //static void ShowVector(int[] vector, bool newLine)
        //{
        //    for (int i = 0; i < vector.Length; ++i)
        //        Console.Write(vector[i] + " ");
        //    Console.WriteLine("");
        //    if (newLine == true) Console.WriteLine("");
        //}

        //static void ShowVector(double[] vector, bool newLine)
        //{
        //    for (int i = 0; i < vector.Length; ++i)
        //        Console.Write(vector[i].ToString("F1") + " ");
        //    Console.WriteLine("");
        //    if (newLine == true) Console.WriteLine("");
        //}

        //static void ShowClustering(double[][] rawData, int numClusters, int[] clustering, bool newLine)
        //{
        //    Console.WriteLine("-----------------");
        //    for (int k = 0; k < numClusters; ++k) // display by cluster
        //    {
        //        for (int i = 0; i < rawData.Length; ++i) // each tuple
        //        {
        //            if (clustering[i] == k) // curr tuple i belongs to curr cluster k.
        //            {
        //                Console.Write("[" + i.ToString().PadLeft(2) + "]");
        //                for (int j = 0; j < rawData[i].Length; ++j)
        //                    Console.Write(rawData[i][j].ToString("F1").PadLeft(6) + " ");
        //                Console.WriteLine("");
        //            }
        //        }
        //        Console.WriteLine("-----------------");
        //    }
        //    if (newLine == true) Console.WriteLine("");
        //}

    } // class
} // ns
