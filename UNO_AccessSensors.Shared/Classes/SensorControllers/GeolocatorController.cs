using System;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

namespace UNO_AccessSensors.Classes
{
    public class GeolocationController : ISensorController
    {
        public Geolocator Geo { get; set; }
        public TextBlock ResultTextBlock { get; set; }
        public CoreDispatcher Dispatcher { get; set; }

        public GeolocationController(TextBlock resultTextBlock, CoreDispatcher dispatcher)
        {
            ResultTextBlock = resultTextBlock;
            Dispatcher = dispatcher;
        }

        public async void Start()
        {
            try
            {
                ResultTextBlock.Text = $"Starting...";

                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:
                        {
                            Geo = new Geolocator { ReportInterval = 1000 };

                            if (Geo == null)
                                ResultTextBlock.Text = $"No available senser found in this device.";
                            else
                            {
                                if (Geo.LocationStatus == PositionStatus.Ready)
                                    Geo.PositionChanged += OnReadingChanged;

                                Geo.StatusChanged += OnStatusChanged;
                            }
                        }
                        break;
                    case GeolocationAccessStatus.Denied:
                        ResultTextBlock.Text = $"Access to Location Sensor Denied.";
                        break;
                    case GeolocationAccessStatus.Unspecified:
                    default:
                        ResultTextBlock.Text = $"Access to Location Sensor Failed";
                        break;
                }
            }
            catch (Exception)
            {
                ResultTextBlock.Text = $"Sonsor not supported by software.";
            }
        }

        public void Stop()
        {
            try
            {
                Geo.StatusChanged -= OnStatusChanged;
                Geo.PositionChanged -= OnReadingChanged;
            }
            catch (Exception)
            {
            }

            ResultTextBlock.Text = $"-";
        }

        private async void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                switch (e.Status)
                {
                    case PositionStatus.Ready:
                        Geo.PositionChanged += OnReadingChanged;
                        break;

                    case PositionStatus.Disabled:
                        {
                            try
                            {
                                Geo.StatusChanged -= OnStatusChanged;
                                Geo.PositionChanged -= OnReadingChanged;
                            }
                            catch (Exception)
                            {
                            }
                        }
                        break;

                    case PositionStatus.Initializing:
                    case PositionStatus.NoData:
                    case PositionStatus.NotInitialized:
                    case PositionStatus.NotAvailable:
                    default:
                        break;

                }
            });
        }

        public async void OnReadingChanged(object osender, object oe)
        {
            var e = oe as PositionChangedEventArgs;

            var result = "";

            try
            {
                var gp = e?.Position;
                var p = gp.Coordinate.Point.Position;

                result =
                    $"Altitude = {p.Altitude:F3} [m] \r\n" +
                    $"Latitude = {p.Latitude:F5} [deg] \r\n" +
                    $"Longitude = {p.Longitude:F5} [deg] \r\n" +
                    $"Update = {DateTime.Now:HH:mm:ss.fffff}";

            }
            catch (Exception ex)
            {
                result = $"Failed to fetch（{ex.Message}）";
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ResultTextBlock.Text = result;
            });
        }

    }
}
