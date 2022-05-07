using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using UNO_Sensors.Classes;

namespace UNO_Sensors
{
    public sealed partial class MainPage : Page
    {
        private void InitializeSensors()
        {
            Button_FetchData.IsEnabled = false;

            SensorControllers = new List<ISensorController>()
            {
                new AccelerometerController(TextBlock_Acc, Dispatcher),
                new OrientationSensorController(TextBlock_Gyro,Dispatcher),
                new MagnetometerController(TextBlock_Mag,Dispatcher),
                new CompassController(TextBlock_Com,Dispatcher),
                new OrientationSensorController(TextBlock_Ori,Dispatcher),
                new GeolocationController(TextBlock_Geo,Dispatcher),
            };

            Button_FetchData.Content = "Start";
            Button_FetchData.IsEnabled = true;
        }
    }
}
