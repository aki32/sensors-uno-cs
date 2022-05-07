using System;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Sensors;
using Windows.UI.Core;

namespace UNO_Sensors.Classes
{
    public class GyrometerController : ISensorController
    {
        public Gyrometer Gyro { get; set; }
        public TextBlock ResultTextBlock { get; set; }
        public CoreDispatcher Dispatcher { get; set; }

        public GyrometerController(TextBlock resultTextBlock, CoreDispatcher dispatcher)
        {
            ResultTextBlock = resultTextBlock;
            Dispatcher = dispatcher;
        }

        public void Start()
        {
            try
            {
                ResultTextBlock.Text = $"Starting...";

                Gyro = Gyrometer.GetDefault();

                if (Gyro == null)
                    ResultTextBlock.Text = $"No available senser found in this device.";
                else
                    Gyro.ReadingChanged += OnReadingChanged;
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
                Gyro.ReadingChanged -= OnReadingChanged;
            }
            catch (Exception)
            {
            }

            ResultTextBlock.Text = $"-";
        }

        public async void OnReadingChanged(object osender, object oe)
        {
            var e = oe as GyrometerReadingChangedEventArgs;
            var result = "";

            try
            {
                var x = e.Reading.AngularVelocityX;
                var y = e.Reading.AngularVelocityY;
                var z = e.Reading.AngularVelocityZ;

                result =
                    $"X = {x,5:F2} [rad/s] \r\n" +
                    $"Y = {y,5:F2} [rad/s] \r\n" +
                    $"Z = {z,5:F2} [rad/s] \r\n" +
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
