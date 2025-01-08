using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UserInterfaceProject
{
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MainWindow mainWindow;

        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;  // Referenz auf MainWindow speichern
        }

        private void ToggleModeButton_Click(object sender, RoutedEventArgs e)
        {
            // Umschalten der Anzeige im SettingsWindow
            isDarkMode = !isDarkMode;

            if (isDarkMode)
            {
                ToggleModeButton.Content = "Wechsel zu Light Mode";
                ApplyDarkMode();
            }
            else
            {
                ToggleModeButton.Content = "Wechsel zu Dark Mode";
                ApplyLightMode();
            }

            // Aufrufen der Methode im MainWindow, um den Modus zu wechseln
            mainWindow.SwitchMainWindowMode();
        }

        private bool isDarkMode = false;  // Variabel für Dark/Light Mode

        private void ApplyDarkMode()
        {
            // Farben im SettingsWindow auf Dark Mode anpassen
            this.Background = new SolidColorBrush(Color.FromRgb(34, 34, 34));  // Dunkler Hintergrund
            ToggleModeButton.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));  // Dunkler Button
            ToggleModeButton.Foreground = new SolidColorBrush(Colors.White);  // Heller Text
        }

        private void ApplyLightMode()
        {
            // Farben im SettingsWindow auf Light Mode anpassen
            this.Background = new SolidColorBrush(Colors.White);  // Heller Hintergrund
            ToggleModeButton.Background = new SolidColorBrush(Colors.LightGray);  // Heller Button
            ToggleModeButton.Foreground = new SolidColorBrush(Colors.Black);  // Dunkler Text
        }

        
    }
}
