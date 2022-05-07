using System;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Sensors;
using Windows.UI.Core;

namespace UNO_Sensors.Classes
{
    public class AccelerometerController : ISensorController
    {
        public Accelerometer Acc { get; set; }
        public TextBlock ResultTextBlock { get; set; }
        public CoreDispatcher Dispatcher { get; set; }

        public AccelerometerController(TextBlock resultTextBlock, CoreDispatcher dispatcher)
        {
            ResultTextBlock = resultTextBlock;
            Dispatcher = dispatcher;
        }

        public void Start()
        {
            try
            {
                ResultTextBlock.Text = $"Starting...";

                Acc = Accelerometer.GetDefault();

                if (Acc == null)
                    ResultTextBlock.Text = $"No available senser found in this device.";
                else
                    Acc.ReadingChanged += OnReadingChanged;
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
                Acc.ReadingChanged -= OnReadingChanged;
            }
            catch (Exception)
            {
            }

            ResultTextBlock.Text = $"-";
        }

        public async void OnReadingChanged(object osender, object oe)
        {
            var e = oe as AccelerometerReadingChangedEventArgs;
            var result = "";

            try
            {
                var x = e.Reading.AccelerationX;
                var y = e.Reading.AccelerationY;
                var z = e.Reading.AccelerationZ;
            
                //result =
                //    $"[X,Y,Z] = [{x,5:F2}, {y,5:F2}, {z,5:F2}] [gal] \r\n" +
                //    $"Update = {DateTime.Now:HH:mm:ss.fffff}";

                result =
                    $"X = {x,5:F2} [gal] \r\n" +
                    $"Y = {y,5:F2} [gal] \r\n" +
                    $"Z = {z,5:F2} [gal] \r\n" +
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
