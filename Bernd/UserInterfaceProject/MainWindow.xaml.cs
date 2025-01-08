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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace UserInterfaceProject
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Liste zum Speichern der vollständigen Dateipfade
        private List<string> validFilePaths = new List<string>();
        private List<string> resultPaths;

        public MainWindow()
        {
            InitializeComponent();
            resultPaths = new List<string>();
            EnableResultButton();
        }

        // PreviewDragOver: Vorabbehandlung von Drag&Drop
        private void SourcePreviewOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;  // Erlaubt das Kopieren von Dateien
            }
            else
            {
                e.Effects = DragDropEffects.None;  // Wenn es keine Dateien sind, ist Drag&Drop nicht erlaubt
            }
            e.Handled = true;  // Markiert das Ereignis als behandelt
        }


        // Beim DragEnter erlauben, dass Dateien reingezogen werden
        private void SourceEnter(object sender, DragEventArgs e)
        {
            // Wenn Dateien vorhanden sind, ist das Ziehen erlaubt
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy; // Erlaube nur Kopieren
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        // Beim DragOver weiterhin erlauben
        private void SourceOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy; // Erlaube nur Kopieren
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        // In PreviewDrop prüfen wir nur, ob der Drop zulässig ist (frühe Überprüfung)
        private void SourcePreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Wir überprüfen die Dateien
                foreach (string file in files)
                {
                    if (IsValidFileType(file)) // Nur gültige Dateien akzeptieren
                    {
                        e.Effects = DragDropEffects.Copy;  // Erlauben der Kopie
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None; // Verhindern des Doppelklicks
                    }
                }
            }
        }

        // Endgültiges Hinzufügen der Dateien in SourceDrop
        private void SourceDrop(object sender, DragEventArgs e)
        {
            // Überprüfen, ob der gezogene Inhalt Dateien enthält
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Hole die Dateien (es können mehrere sein)
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Liste für gültige Dateien zurücksetzen
                List<string> validFiles = new List<string>();

                foreach (string file in files)
                {
                    if (IsImageFile(file) || IsTextFile(file)) // Nur gültige Dateien akzeptieren
                    {
                        validFiles.Add(System.IO.Path.GetFileName(file));  // Nur Dateiname
                        PreviewFile(file);  // Datei im PreviewSource anzeigen
                    }
                    else
                    {
                        MessageBox.Show($"Die Datei '{System.IO.Path.GetFileName(file)}' ist ungültig. Bitte ziehen Sie nur .bmp- oder .txt-Dateien.");
                    }
                }

                // Wenn gültige Dateien gefunden wurden, hängen wir sie an den bestehenden Text der TextBox an
                if (validFiles.Count > 0)
                {
                    // Der bestehende Text wird beibehalten und die neuen Dateinamen werden angehängt
                    if (!string.IsNullOrEmpty(Source.Text))
                    {
                        Source.Text += ", "; // Komma und Leerzeichen zwischen den Dateinamen
                    }
                    Source.Text += string.Join(", ", validFiles); // Füge die neuen Dateinamen an

                    // Die resultPaths-Liste wird mit den vollständigen Pfaden der Dateien aktualisiert
                    resultPaths.AddRange(validFiles.Select(file => System.IO.Path.Combine("C:\\Path\\To\\Files", file))); // Beispielhafte Pfade
                }
                else
                {
                    Source.Clear();  // Wenn keine gültigen Dateien, leere die TextBox
                    resultPaths.Clear(); // Setze die Liste zurück
                }

                // Nach dem Drop sicherstellen, dass der Result-Button aktualisiert wird
                EnableResultButton();
            }
        }



        // The file preview logic for .bmp files
        private void PreviewFile(string filePath)
        {
            if (IsImageFile(filePath))
            {
                // Create a BitmapImage to load the .bmp file
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Ensure it is fully loaded immediately
                    bitmap.EndInit();

                    // Optional: Use a separate Image control instead of PreviewSource
                    Image previewImage = new Image
                    {
                        Source = bitmap,
                        Stretch = Stretch.Uniform,
                        Width = PreviewSourceImage.Width,
                        Height = PreviewSourceImage.Height
                    };

                    // Add this image dynamically to a container in the UI (if needed)
                    PreviewSourceImage.Source = bitmap;

                    // Alternatively, update a dedicated image control if available
                    // E.g., if you have an Image control named "ImagePreview", you can:
                    // ImagePreview.Source = bitmap;
                }
                catch (Exception ex)
                {
                    PreviewSource.Text = $"Fehler beim Laden des Bildes: {ex.Message}";
                }
            }
            else if (IsTextFile(filePath))
            {
                // Existing logic for .txt files
                try
                {
                    string content = File.ReadAllText(filePath);
                    PreviewSource.Text = content;
                }
                catch (Exception ex)
                {
                    PreviewSource.Text = $"Fehler beim Laden der Datei: {ex.Message}";
                }
            }
            else
            {
                PreviewSource.Text = "Ungültiger Dateityp.";
            }
        }

        private bool IsImageFile(string filePath)
        {
            string[] validImageExtensions = { ".bmp" };
            PreviewSource.Visibility = Visibility.Hidden;
            PreviewSourceImage.Visibility = Visibility.Visible;
            string extension = System.IO.Path.GetExtension(filePath).ToLower();  // Hier verwenden wir Path.GetExtension
            return validImageExtensions.Contains(extension);
        }

        private bool IsTextFile(string filePath)
        {
            string[] validTextExtensions = { ".txt" };
            PreviewSource.Visibility = Visibility.Visible;
            PreviewSourceImage.Visibility = Visibility.Hidden;
            string extension = System.IO.Path.GetExtension(filePath).ToLower();  // Hier verwenden wir Path.GetExtension
            return validTextExtensions.Contains(extension);
        }

        private bool IsValidFileType(string filePath)
        {
            string[] validExtensions = { ".bmp", ".txt" };
            string extension = System.IO.Path.GetExtension(filePath).ToLower();
            return validExtensions.Contains(extension);
        }

        // Zugriff auf die Dateipfade für weitere Verwendung (z. B. Öffnen der Dateien)
        private void ProcessValidFiles()
        {
            foreach (var filePath in validFilePaths)
            {
                // Beispiel: Hier kannst du die Pfade verwenden, um mit den Dateien zu arbeiten
                MessageBox.Show($"Verarbeiteter Pfad: {filePath}");
            }
        }



        //Clear and Execute - Middle
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            Source.Text = null;
            PreviewSource.Text = null;
            Result.Content = "";
            PreviewResult.Text = null;
            resultPaths.Clear();
            EnableResultButton();
        }




        //Dummy for Testing Result     
        private string dummyFolderPath = @"C:\Users\ultra\Downloads";
        private void ExecuteClick(object sender, RoutedEventArgs e)
        {

            if (Source.Text != "")
            {

                resultPaths = Source.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(path => path.Trim()) // Entferne Leerzeichen
                                      .ToList();


                //Stuff for Execution
                // Setze den Dummy-Inhalt
                ProcessValidFiles();

                // Button aktivieren oder deaktivieren
                UpdateButtonState();

                // Dummy-Pfad (Kann angepasst werden)

                // Setze den Pfad im Result-Button
                Result.Content = dummyFolderPath;  // Setze den Dummy-Pfad als Button-Inhalt

                // Stelle sicher, dass der Button nun klickbar ist
                Result.IsEnabled = true;

                EnableResultButton();
            }
            else 
            {
                MessageBox.Show("Keine Dateien gefunden", "Warnung", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }




        //Result - Right Half
        private void ResultClick(object sender, RoutedEventArgs e)
        {
            // Pfad aus dem Button-Content abrufen
            string folderPath = dummyFolderPath;

            // Öffne den Ordner des ersten Pfades (z.B. der erste Pfad ist der Dummy-Pfad)
            if (resultPaths.Count > 0 && Directory.Exists(dummyFolderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = dummyFolderPath,  // Öffne den Ordner des ersten Pfades
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Der Ordner existiert nicht.");
            }

            try
            {
                if (IsTextFile(Source.Text))
                {
                    // File is a .txt file
                    PreviewResult.Visibility = Visibility.Visible;
                    PreviewResultImage.Visibility = Visibility.Hidden;

                    // Display the content of the text file in the TextBox
                    PreviewResult.Text = File.ReadAllText(Source.Text);
                }
                else if (IsImageFile(Source.Text))
                {
                    // File is a .bmp file
                    PreviewResult.Visibility = Visibility.Hidden;
                    PreviewResultImage.Visibility = Visibility.Visible;

                    // Display the .bmp file in the Image control
                    PreviewResultImage.Source = new BitmapImage(new Uri(Source.Text));
                }
                else
                {
                    MessageBox.Show("Ungültiger Dateityp. Unterstützt werden nur .txt und .bmp.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Verarbeiten der Datei: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Methode zum Aktivieren/Deaktivieren des Result-Buttons
        private void EnableResultButton()
        {
            // Wenn der resultPath leer ist, deaktivieren wir den Button
            Result.IsEnabled = resultPaths.Any() && !string.IsNullOrEmpty(resultPaths[0]); ;
        }

        //Button-Stuff
        private void UpdateButtonState()
        {
            // Aktivieren oder deaktivieren basierend auf dem Button-Inhalt
            Result.IsEnabled = !string.IsNullOrEmpty(Result.Content?.ToString());
        }

        private void ClearButtonContent(object sender, RoutedEventArgs e)
        {
            Result.Content = string.Empty; // Button leeren
            UpdateButtonState();
        }



        //Help-Button
        private void HelpClick(object sender, RoutedEventArgs e)
        {
            string helpText = "Dies ist die Hilfe:\n\n" +
                      "1. Ziehen Sie eine Datei in die Eingabetextbox.\n" +
                      "2. Unterstützte Formate: .txt und .bmp.\n" +
                      "3. Klicken Sie auf 'Ausführen', um den Ordnerpfad anzuzeigen.\n" +
                      "4. Klicken Sie auf den 'Result'-Button, um den Ordner zu öffnen.\n" +
                      "5. Zum Leeren des Programmes einfach 'Leeren' drücken.\n";
                

            MessageBox.Show(helpText, "Hilfe", MessageBoxButton.OK, MessageBoxImage.Information);
        }




        //Settings-Button
        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(this);  // Hauptfenster übergeben
            settingsWindow.ShowDialog(); // Modal, blockiert das Hauptfenster, bis es geschlossen wird
        }

        private bool isMainWindowDarkMode = false;

        private void ApplyDarkModeToMainWindow()
        {
            // Hintergrundfarbe des Fensters ändern
            MainGrid.Background = new SolidColorBrush(Color.FromRgb(34, 34, 34));

            // TextBoxen und andere UI-Elemente anpassen
            foreach (var child in MainGrid.Children)
            {
                if (child is Control control)
                {
                    control.Foreground = new SolidColorBrush(Colors.White);  // Textfarbe weiß für Dark Mode
                    if (control is TextBox textBox)
                    {
                        textBox.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));  // Dunkler Hintergrund für TextBox
                    }
                    else if (control is Button button)
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));  // Dunkler Hintergrund für Button
                    }
                }
            }
        }

        private void ApplyLightModeToMainWindow()
        {
            // Hintergrundfarbe des Fensters ändern
            MainGrid.Background = new SolidColorBrush(Colors.White);

            // TextBoxen und andere UI-Elemente anpassen
            foreach (var child in MainGrid.Children)
            {
                if (child is Control control)
                {
                    control.Foreground = new SolidColorBrush(Colors.Black);  // Textfarbe schwarz für Light Mode
                    if (control is TextBox textBox)
                    {
                        textBox.Background = new SolidColorBrush(Colors.White);  // Weißer Hintergrund für TextBox
                    }
                    else if (control is Button button)
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));  // Heller Hintergrund für Button
                    }
                }
            }
        }

        public void SwitchMainWindowMode()
        {
            if (isDarkMode)
            {
                ApplyLightModeToMainWindow();
            }
            else
            {
                ApplyDarkModeToMainWindow();
            }

            isDarkMode = !isDarkMode;
        }
        private bool isDarkMode = false;  // Variabel, um den Modus zu speichern

    }
    
}
