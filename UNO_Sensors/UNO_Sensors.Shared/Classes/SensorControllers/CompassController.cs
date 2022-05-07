using System;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Sensors;
using Windows.UI.Core;

namespace UNO_Sensors.Classes
{
    public class CompassController : ISensorController
    {
        public Compass Com { get; set; }
        public TextBlock ResultTextBlock { get; set; }
        public CoreDispatcher Dispatcher { get; set; }

        public CompassController(TextBlock resultTextBlock, CoreDispatcher dispatcher)
        {
            ResultTextBlock = resultTextBlock;
            Dispatcher = dispatcher;
        }

        public void Start()
        {
            try
            {
                ResultTextBlock.Text = $"Starting...";

                Com = Compass.GetDefault();

                if (Com == null)
                    ResultTextBlock.Text = $"No available senser found in this device.";
                else
                    Com.ReadingChanged += OnReadingChanged;
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
                Com.ReadingChanged -= OnReadingChanged;
            }
            catch (Exception)
            {
            }
            ResultTextBlock.Text = $"-";
        }

        public async void OnReadingChanged(object osender, object oe)
        {
            var e = oe as CompassReadingChangedEventArgs;
            var result = "";

            try
            {
                var ori = e.Reading.HeadingMagneticNorth;
                result =
                    $"{ ori:F0} degrees from magnetic pole \r\n" +
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
