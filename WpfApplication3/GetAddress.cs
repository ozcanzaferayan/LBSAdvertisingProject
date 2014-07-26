using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication3.GeocodeService;

namespace WpfApplication3
{
    public class GetAddress
    {
        public static List<String> MakeGeocodeRequestForString(Microsoft.Maps.MapControl.WPF.Location location)
        {
            List<String> Results = new List<string>();
            try
            {
                // Set a Bing Maps key before making a request
                string key = "AqRg7HvVzL9R8YZBo60ZjfRQh-H7BukL-gbUmF0-bQWGmz7GmguIk2bQ_9nfbL2c";

                GeocodeService.ReverseGeocodeRequest reverseGeocodeRequest = new GeocodeService.ReverseGeocodeRequest();

                // Set the credentials using a valid Bing Maps key
                reverseGeocodeRequest.Credentials = new Microsoft.Maps.MapControl.WPF.Credentials();
                reverseGeocodeRequest.Credentials.ApplicationId = key;

                // Set the point to use to find a matching address
                GeocodeService.Location point = new GeocodeService.Location();
                point.Latitude = location.Latitude;
                point.Longitude = location.Longitude;

                reverseGeocodeRequest.Location = point;

                // Make the reverse geocode request
                GeocodeService.GeocodeServiceClient geocodeService =
                new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                GeocodeService.GeocodeResponse geocodeResponse = geocodeService.ReverseGeocode(reverseGeocodeRequest);
                MessageBox.Show("Test");

                foreach (var result in geocodeResponse.Results)
                {
                    Results.Add(result.DisplayName);
                }
                return Results;

            }
            catch (Exception ex)
            {
                Results[0] = "An exception occurred: " + ex.Message; // BingMap keys might be expired
                return Results;

            }

        }

        public static List<GeocodeResult> MakeGeocodeRequestForAddress(Microsoft.Maps.MapControl.WPF.Location location)
        {
            List<GeocodeResult> Results = new List<GeocodeResult>();
            try
            {
                // Set a Bing Maps key before making a request
                string key = "AsTa4r991xLVZ7qJ9o2mJ374XZZyNxob6tJ2rJkzKqn2xNR7eifHRhsWhckgtkin";

                GeocodeService.ReverseGeocodeRequest reverseGeocodeRequest = new GeocodeService.ReverseGeocodeRequest();

                // Set the credentials using a valid Bing Maps key
                reverseGeocodeRequest.Credentials = new Microsoft.Maps.MapControl.WPF.Credentials();
                reverseGeocodeRequest.Credentials.ApplicationId = key;

                // Set the point to use to find a matching address
                GeocodeService.Location point = new GeocodeService.Location();
                point.Latitude = location.Latitude;
                point.Longitude = location.Longitude;

                reverseGeocodeRequest.Location = point;

                // Make the reverse geocode request
                GeocodeService.GeocodeServiceClient geocodeService =
                new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                GeocodeService.GeocodeResponse geocodeResponse = geocodeService.ReverseGeocode(reverseGeocodeRequest);

                return geocodeResponse.Results.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred");
                return Results;

            }

        }
    }
}
