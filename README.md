
# FBI (File Bitmap Integrator)

## Was ist FBI?

**FBI** ist ein innovativer Textkompressor, der Textdaten zunächst komprimiert und anschließend in einer Bitmap-Datei speichert. Ziel ist es, Speicherplatz zu sparen und eine alternative Methode zur Speicherung von Textdaten zu bieten.

## Funktionsweise

1. **Textkomprimierung**  
   - Der eingegebene Text wird komprimiert, um redundante Informationen zu eliminieren.  
   - Dies reduziert die Dateigröße und optimiert die Speicherung.

2. **Speicherung in einer Bitmap**  
   - Die komprimierten Daten werden in einer Bitmap-Datei gespeichert.  
   - Die Pixelwerte der Bitmap repräsentieren die komprimierten Textdaten.

3. **Rückgewinnung des Textes**  
   - Der ursprüngliche Text kann aus der Bitmap wieder extrahiert werden.  
   - Dekodierung und Dekomprimierung stellen sicher, dass keine Daten verloren gehen.

## Features der GUI

1. **Drag-and-Drop-Unterstützung**  
   - Benutzer können Dateien einfach in das Fenster ziehen, um sie zu laden.

2. **"Verstecken"-Funktion**  
   - Ermöglicht das Einbetten eines Textes in eine Bitmap.  
   - Der Text wird in Binärdaten umgewandelt, komprimiert und in einer Bitmap-Datei gespeichert.

3. **"Extrahieren"-Funktion**  
   - Entschlüsselt und dekomprimiert versteckten Text aus einer Bitmap.  
   - Zeigt den Originaltext in der Benutzeroberfläche an.

4. **Vorschau**  
   - Zeigt Original- und extrahierte Inhalte an.  
   - Vorschau der Bitmap mit eingebettetem Text.

5. **Statusleiste**  
   - Zeigt Informationen wie Dateigröße und Komprimierungsfortschritt.

## Anwendungsbeispiele

- **Datenkompression**  
  Speichern großer Textmengen in einem komprimierten und platzsparenden Format.
  
- **Verborgene Nachrichten**  
  Verstecken von Informationen in einem unauffälligen Bitmap-Bild.

- **Speicheroptimierung**  
  Effektiv für Systeme mit begrenztem Speicherplatz.

## Installation

### 1. Repository klonen
```bash
git clone https://github.com/PaperTobi/fbi.git
```

### 2. In das Verzeichnis wechseln
```bash
cd fbi
```

### 3. Abhängigkeiten installieren
Falls .NET verwendet wird:
```bash
dotnet restore
```

### 4. Projekt kompilieren
```bash
dotnet build
```

### 5. FBI ausführen
```bash
dotnet run
```

## Technische Details

1. **Komprimierung**  
   Verwendet effiziente Algorithmen wie Huffman-Kodierung, um die Textdaten zu reduzieren.

2. **Bitmap-Speicherung**  
   - Komprimierte Daten werden in Pixeln der Bitmap gespeichert.  
   - Dies geschieht oft über die LSB-Methode (Least Significant Bit), um die visuelle Integrität des Bildes zu wahren.

3. **Dekodierung**  
   - Die Daten werden aus der Bitmap extrahiert und dekomprimiert.  
   - Der ursprüngliche Text wird wiederhergestellt.

---

## Lizenz
Dieses Projekt steht unter der [MIT-Lizenz](LICENSE).

## Autor
Ein Typ und nochn Typ und noch Typ und @ThomasMouraBachmann
