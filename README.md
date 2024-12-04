# FBI (File Bitmap Integrator)

## Was ist FBI?

**FBI** ist ein innovativer Textkompressor, der Textdaten zuerst komprimiert und anschließend in einer Bitmap-Datei speichert. Die Idee hinter FBI ist es, Text effizient zu komprimieren und ihn in einem Bildformat zu speichern, um Platz zu sparen und eine alternative Methode zur Speicherung von Textdaten zu bieten.

## Funktionsweise

1. **Textkomprimierung**:  
   FBI komprimiert den eingegebenen Text, um die Daten zu reduzieren und redundante Informationen zu eliminieren. Dieser Komprimierungsprozess sorgt dafür, dass der Text in einer kleineren Dateigröße gespeichert wird.

2. **Speicherung in einer Bitmap**:  
   Nachdem der Text komprimiert wurde, wird er in einem Bitmap-Format abgelegt. Eine Bitmap ist hier ein Bildformat, das die komprimierten Daten visuell darstellt, wodurch die Textdaten in Form von Pixeln gespeichert werden.

3. **Rückgewinnung des Textes**:  
   FBI ermöglicht es, den komprimierten Text aus der Bitmap wiederherzustellen. Der ursprüngliche Text kann aus der gespeicherten Bitmap-Datei extrahiert und dekodiert werden, sodass keine Daten verloren gehen.

## Anwendungsbeispiele

- **Datenkompression**: Speichern Sie große Textmengen in einem komprimierten Format, das weniger Speicher benötigt.
- **Verborgene Nachrichten**: Da die Daten in einem Bitmap-Bild gespeichert werden, können sie unauffällig verborgen und übertragen werden.
- **Speicheroptimierung**: Ideal für Umgebungen mit begrenztem Speicherplatz, da Textdaten effizient in einem Bildformat gespeichert werden können.

## Installation

### 1. Repository klonen

Zuerst müssen Sie das Repository auf Ihren lokalen Computer klonen. Öffnen Sie ein Terminal und führen Sie folgenden Befehl aus:

git clone https://github.com/PaperTobi/fbi.git

2. In das Verzeichnis wechseln

Wechseln Sie in das Verzeichnis des geklonten Repositories:

cd fbi

3. Abhängigkeiten installieren

Installieren Sie die erforderlichen Abhängigkeiten für das Projekt. Wenn Sie mit .NET arbeiten, verwenden Sie den folgenden Befehl:

dotnet restore

4. Projekt kompilieren

Kompilieren Sie das Projekt mit:

dotnet build

5. FBI ausführen

Führen Sie das Projekt aus, um FBI zu starten:

dotnet run
