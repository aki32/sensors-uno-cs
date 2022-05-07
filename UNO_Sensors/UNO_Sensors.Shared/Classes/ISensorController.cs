using Windows.UI.Xaml.Controls;
using Windows.UI.Core;

namespace UNO_Sensors.Classes
{
  
    /// <summary>
    /// Interface to on-off sensors
    /// </summary>
    interface ISensorController
    {
        CoreDispatcher Dispatcher { get; set; }
        TextBlock ResultTextBlock { get; set; }
        void Start();
        void Stop();
        void OnReadingChanged(object sender, object e);
    }
  // uno platform support range
    // https://platform.uno/docs/articles/features/windows-devices-sensors.html
    // 
    // xamarin support range
    // https://docs.microsoft.com/en-us/xamarin/essentials/
    // 

}