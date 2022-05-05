using System;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Sensors;
using Windows.UI.Core;

namespace UNO_AccessSensors.Classes
{
    //
    // ref : https://garchiving.com/angle-calculation-from-magnetic-sensor/
    // turn this device 360 degrees to calibrate
    //

    public class MagnetometerController : ISensorController
    {
        public Magnetometer Mag { get; set; }
        public TextBlock ResultTextBlock { get; set; }
        public CoreDispatcher Dispatcher { get; set; }

        #region field

        float xMin;
        float xMax;
        float yMin;
        float yMax;

        float xCenter => (xMin + xMax) / 2;
        float yCenter => (yMin + yMax) / 2;

        float xRange => Math.Abs(xMin - xMax);
        float yRange => Math.Abs(yMin - yMax);
        float rangeMax => Math.Max(xRange, yRange);
        float rangeMin => Math.Min(xRange, yRange);

        #endregion

        public MagnetometerController(TextBlock resultTextBlock, CoreDispatcher dispatcher)
        {
            ResultTextBlock = resultTextBlock;
            Dispatcher = dispatcher;
        }

        public void Start()
        {
            try
            {
                ResultTextBlock.Text = $"Starting...";

                Mag = Magnetometer.GetDefault();

                if (Mag == null)
                    ResultTextBlock.Text = $"No available senser found in this device.";
                else
                {
                    xMin = float.MaxValue;
                    xMax = float.MinValue;
                    yMin = float.MaxValue;
                    yMax = float.MinValue;

                    Mag.ReadingChanged += OnReadingChanged;
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
                Mag.ReadingChanged -= OnReadingChanged;
            }
            catch (Exception)
            {
            }
            ResultTextBlock.Text = $"-";
        }

        public async void OnReadingChanged(object osender, object oe)
        {
            var e = oe as MagnetometerReadingChangedEventArgs;
            var result = "";

            try
            {
                var x = e.Reading.MagneticFieldX;
                var y = e.Reading.MagneticFieldY;
                var z = e.Reading.MagneticFieldZ;

                xMin = Math.Min(xMin, x);
                xMax = Math.Max(xMax, x);
                yMin = Math.Min(yMin, y);
                yMax = Math.Max(yMax, y);

                x -= xCenter;
                y -= yCenter;

                var ori = 180 * Math.Atan2(x, y) / Math.PI;

                //result = $"[X,Y,Z] = [{x,5:F2}, {y,5:F2}, {z,5:F2}] [µT] \r\n";

                result =
                    $"X = {x,5:F2} [µT] \r\n" +
                    $"Y = {y,5:F2} [µT] \r\n" +
                    $"Z = {z,5:F2} [µT] \r\n";

                result += $"{ ori:F0} degrees from magnetic pole \r\n";
                result += $"(turn this device 360 degrees to calibrate) \r\n";
                result += $"（keep magnetic stuff away from this device） \r\n";

                //if (rangeMin < 40)
                //{
                //    // Need to calibrate
                //    result += $"Please turn this device 360 degrees to calibrate. \r\n";
                //}
                //else if (rangeMax > 100)
                //{
                //    // Need to reboot since too much change in magnetic field.
                //    result += $"Too much change in magnetic field. Need to reboot.\r\n";
                //}
                //else
                //{
                //    // Good calibration
                //    result += $"{ori:F0} degrees from magnetic pole\r\n";
                //}

                result += $"Update = {DateTime.Now:HH:mm:ss.fffff}";
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
