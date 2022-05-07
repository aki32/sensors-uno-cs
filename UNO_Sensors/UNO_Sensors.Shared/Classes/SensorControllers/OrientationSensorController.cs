using System;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Sensors;
using Windows.UI.Core;

namespace UNO_Sensors.Classes
{
    public class OrientationSensorController : ISensorController
    {
        public OrientationSensor Ori { get; set; }
        public TextBlock ResultTextBlock { get; set; }
        public CoreDispatcher Dispatcher { get; set; }

        public OrientationSensorController(TextBlock resultTextBlock, CoreDispatcher dispatcher)
        {
            ResultTextBlock = resultTextBlock;
            Dispatcher = dispatcher;
        }

        public void Start()
        {
            try
            {
                ResultTextBlock.Text = $"Starting...";

                Ori = OrientationSensor.GetDefault();

                if (Ori == null)
                    ResultTextBlock.Text = $"No available senser found in this device.";
                else
                    Ori.ReadingChanged += OnReadingChanged;
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
                Ori.ReadingChanged -= OnReadingChanged;
            }
            catch (Exception)
            {
            }
            ResultTextBlock.Text = $"-";
        }

        public async void OnReadingChanged(object osender, object oe)
        {
            var e = oe as OrientationSensorReadingChangedEventArgs;
            var result = "";

            try
            {
                var data = e.Reading;
                var x = data.Quaternion.X;
                var y = data.Quaternion.Y;
                var z = data.Quaternion.Z;
                var w = data.Quaternion.W;

                //result =
                //    $"Reading: X: {x,5:F2}, Y: {y,5:F2}, Z: {z,5:F2}, W: {w,5:F2} \r\n" +
                //    $"Update = {DateTime.Now:HH:mm:ss.fffff}";

                result =
                 $"X = {x,5:F2} \r\n" +
                 $"Y = {y,5:F2} \r\n" +
                 $"Z = {z,5:F2} \r\n" +
                 $"W = {w,5:F2} \r\n" +
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
