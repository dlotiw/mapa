using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using System.Diagnostics;
using GeoCoordinatePortable;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace mapa
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Koordynaty : Page
    {
        
        public Koordynaty()
        {
            this.InitializeComponent();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void szukaj_Click(object sender, RoutedEventArgs e)
        {
            if(adres.Text != null && adres.Text!="")
            {
                var pozycja = await MapLocationFinder.FindLocationsAsync(adres.Text, new Geopoint(Dane.pkStartowy), 3);
                if (pozycja.Status == MapLocationFinderStatus.Success)
                {
                    try
                    {
                        Dane.pktKoncowy = pozycja.Locations[0].Point.Position;
                        szer.Text = Math.Round(pozycja.Locations[0].Point.Position.Latitude,2).ToString();
                        dlug.Text = Math.Round(pozycja.Locations[0].Point.Position.Longitude,2).ToString();
                        Dane.opisCelu = adres.Text;
                        var geo_st = new GeoCoordinate(Dane.pkStartowy.Latitude,Dane.pkStartowy.Longitude);
                        var geo_kn = new GeoCoordinate(Dane.pktKoncowy.Latitude, Dane.pktKoncowy.Longitude);
                        odleghlosc.Text = $"Odległość między celem a obecnym miejscem: {Math.Round(geo_st.GetDistanceTo(geo_kn)/1000,2)} km";


                    }
                    catch(Exception ex)
                    {
                        dlug.Text = "";
                        szer.Text = "Podano zły adres";
                    }
                }
                else
                {
                    dlug.Text = "";
                    szer.Text = "Nie znaleziono podanego adresu";
                }
            }
            
            
        }
        

        public async void gdzieJa()
        {
            Geolocator mojGps = new Geolocator();
            mojGps.DesiredAccuracy = PositionAccuracy.High;
            var z_gps = await mojGps.GetGeopositionAsync();
            BasicGeoposition x = new BasicGeoposition();
            x.Latitude = z_gps.Coordinate.Point.Position.Latitude;
            x.Longitude = z_gps.Coordinate.Point.Position.Longitude;
            gps.Text = $"Twoje położenie geograficzne to: szerokość: {Math.Round(x.Latitude,2)} długość: {Math.Round(x.Longitude,2)}";
            Dane.pkStartowy = x;


        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            gdzieJa();
            
        }
    }
}
