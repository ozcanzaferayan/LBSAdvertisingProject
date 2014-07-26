using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Maps.MapControl.WPF;

namespace WpfApplication3
{
    public class Address
    {
        public String Country { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String PostalCode { get; set; }
        public String Locality { get; set; }
        public String HouseNumber { get; set; }
        public Location AddressLocation { get; set; }

        public static Address GeocodeResultConverter(List<GeocodeService.GeocodeResult> geocodeResult)
        {
            Address returnedAddress = new Address();
            if (geocodeResult.Count == 0) { return returnedAddress; }
            returnedAddress.Country = geocodeResult[0].Address.CountryRegion;
            returnedAddress.City = geocodeResult[0].Address.AdminDistrict;
            returnedAddress.Street = geocodeResult[0].Address.AddressLine;
            returnedAddress.PostalCode = geocodeResult[0].Address.PostalCode;
            returnedAddress.Locality = geocodeResult[0].Address.Locality;
            returnedAddress.HouseNumber = "";

            if (returnedAddress.Street.Equals("")) { return returnedAddress; }
            
            if (returnedAddress.Country == "Turkey")
            {
                // Aşağıdaki Split(' ') ile String'in sonundaki String'i alıyoruz
                // Eğer son kelimenin ilk karakteri sayı değilse o zaman sokak numarası almıyoruz demektir
                // Örneğin String: Kenar Sokak 19 olursa dönen ifade 19 olacaktır
                String probablyHouseNumber = returnedAddress.Street.Split(' ').Last();
                int testedInteger; // Sadece aşağıdaki fonksiyonda test etmek için
                try
                {
                    if (Int32.TryParse(probablyHouseNumber[0].ToString(), out testedInteger))
                    {
                        returnedAddress.HouseNumber = probablyHouseNumber;
                        // Replace ile son sayıyı atarak asıl kapı numarasını elde ediyoruz
                        returnedAddress.Street = returnedAddress.Street.Replace(" " + returnedAddress.HouseNumber, "");
                    }
                    else
                    {
                        returnedAddress.HouseNumber = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Parse Edilemedi!");
                }
                
            }
            //Diğer ülkelerde Sokak ismini bulma sorun çıkartabilir
            else
            {
                String probablyHouseNumber = returnedAddress.Street.Split(' ').First();
                int testedInteger; // Sadece aşağıdaki fonksiyonda test etmek için
                try
                {
                    if (Int32.TryParse(probablyHouseNumber[0].ToString(), out testedInteger))
                    {
                        returnedAddress.HouseNumber = probablyHouseNumber;
                        // Replace ile son sayıyı atarak asıl kapı numarasını elde ediyoruz
                        returnedAddress.Street = returnedAddress.Street.Replace(returnedAddress.HouseNumber + " ", "");
                    }
                }
                catch (Exception)
                { 
                }
            }
            return returnedAddress;
        }

        public override string ToString()
        {
            if (Object.ReferenceEquals(Country,null))
            {
                return "Gösterilecek sonuç yok!";
            }
            // Bir yerin kapı numarası bulunmayabilir bu yüzden test ediyoruz
            String tempHouseNumber = HouseNumber;
            String tempStreet = Street;
            if (!tempStreet.Equals("")) tempStreet = tempStreet + "  ";
            if (!tempHouseNumber.Equals(""))
            {
                tempHouseNumber = "No: " + tempHouseNumber+ "  ";
            }
            return tempStreet + tempHouseNumber + Locality + "/" + City + "  " + PostalCode + "  " + Country;
                //+ "                                                                                     (" +
                //AddressLocation.Latitude + " , " + AddressLocation.Longitude + ")";
        }
    }
}
