using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UNO_Sensors
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeSensors();
        }

        public void Button_FetchData_Tapped(object sender__, TappedRoutedEventArgs e_)
        {
            Button_FetchData.IsEnabled = false;

            IsActive = !IsActive;
            if (IsActive)
            {
                // Start!
                Button_FetchData.Content = "Starting...";
                SensorControllers.ForEach(s => s.Start());
                Button_FetchData.Content = "Stop";
            }
            else
            {
                // Stop!
                Button_FetchData.Content = "Stopping...";
                SensorControllers.ForEach(s => s.Stop());
                Button_FetchData.Content = "Start";
            }

            Button_FetchData.IsEnabled = true;
        }

    }
}
