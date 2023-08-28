using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using System.Diagnostics;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace mapa
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private void pow_Click(object sender, RoutedEventArgs e)
        {
            if(mojaMapa.ZoomLevel < 20)
            {
                mojaMapa.ZoomLevel += 0.5;
            }
            
        }

        private void pom_Click(object sender, RoutedEventArgs e)
        {
            if (mojaMapa.ZoomLevel > 2)
            {
                mojaMapa.ZoomLevel -= 0.5;
            }
            
        }

        private void tryb_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBar = new AppBarButton();
            appBar=sender as AppBarButton;
            FontIcon icon = new FontIcon();
            icon.FontFamily = FontFamily.XamlAutoFontFamily;
            if(mojaMapa.Style == MapStyle.Road)
            {
                mojaMapa.Style = MapStyle.AerialWithRoads;
                appBar.Label = "Satelita";
                icon.Glyph = "S";
                appBar.Icon = icon;
            }
            else
            {
                mojaMapa.Style= MapStyle.Road;
                appBar.Label = "Drogowa";
                icon.Glyph = "D";
                appBar.Icon = icon;
            }
        }

        private void koor_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(Koordynaty));
        }
        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Dane.pkStartowy.Latitude > 0)
            {
                var MyLandmarks = new List<MapElement>();
                
                var znacznikStart = new MapIcon
                {
                    Location = new Geopoint(Dane.pkStartowy),
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 0,
                    Title = "Tu jestem"
                };
                var znacznikEnd = new MapIcon
                {
                    Location = new Geopoint(Dane.pktKoncowy),
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 1,
                    Title = Dane.opisCelu
                };
                if(Dane.pktKoncowy.Longitude > 0)
                {
                    var liniaLotem = new MapPolyline
                    {
                        StrokeColor = Windows.UI.Colors.Red,
                        StrokeThickness = 2,
                        StrokeDashed = true,
                        Path = new Geopath(new List<BasicGeoposition>
                        {
                            Dane.pkStartowy,
                            Dane.pktKoncowy
                        })
                    };
                    MyLandmarks.Add(liniaLotem);

                }


                MyLandmarks.Add(znacznikStart);
                MyLandmarks.Add(znacznikEnd);
                

                var LandmarksLayer = new MapElementsLayer
                {
                    ZIndex = 1,
                    MapElements = MyLandmarks
                };

                mojaMapa.Layers.Add(LandmarksLayer);

                mojaMapa.Center = new Geopoint(Dane.pkStartowy);
                mojaMapa.ZoomLevel = 8;
                
            }
            
            
        }

        private async void trasa_Click(object sender, RoutedEventArgs e)
        {
            

            
            MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(Dane.pkStartowy),
                  new Geopoint(Dane.pktKoncowy),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
               
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

               
                mojaMapa.Routes.Add(viewOfRoute);

                
                await mojaMapa.TrySetViewBoundsAsync(
                      routeResult.Route.BoundingBox,
                      null,
                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);


            }
            
        }
    }
    
}
