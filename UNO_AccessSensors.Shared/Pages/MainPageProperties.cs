using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using UNO_AccessSensors.Classes;

namespace UNO_AccessSensors
{
    public sealed partial class MainPage : Page
    {
        private List<ISensorController> SensorControllers { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
