//©PaperTobi
using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string s_Datei = "Datei.txt";
        string s_DateiKomprimiert = "komprimierteDatei.txt";
        string s_DateiDekomprimiert = "dekomprimierteDatei.txt";

        // Create the file
        DateiErstellen(s_Datei);

        // Read and convert to binary
        string binaryContent = LesenUndKonvertieren(s_Datei);
        Console.WriteLine("Binary Content: " + binaryContent);

        // Compress the binary content
        string komprimierterInhalt = Komprimieren(binaryContent);
        Console.WriteLine("Compressed Content: " + komprimierterInhalt);

        // Save the compressed content
        DateiSpeichern(s_DateiKomprimiert, komprimierterInhalt);

        // Read the compressed content
        string komprimierterInhaltGelesen = DateiLesen(s_DateiKomprimiert);

        // Decompress the content
        string dekomprimierterInhalt = Dekomprimieren(komprimierterInhaltGelesen);
        Console.WriteLine("Decompressed Content: " + dekomprimierterInhalt);

        // Save the decompressed content
        DateiSpeichern(s_DateiDekomprimiert, dekomprimierterInhalt);
    }

    static void DateiErstellen(string s_Datei)
    {
        string s_Text = "Hallo World!!!!";
        File.Delete(s_Datei);
        File.Create(s_Datei).Close();
        Console.WriteLine("Datei erstellt");
        StreamWriter o_SW = new StreamWriter(s_Datei);
        o_SW.WriteLine(s_Text);
        o_SW.Close();
        Console.WriteLine("Sollte geschrieben sein");
    }

    static string LesenUndKonvertieren(string s_Datei)
    {
        StringBuilder binaryContent = new StringBuilder();
        using (FileStream fs = new FileStream(s_Datei, FileMode.Open, FileAccess.Read))
        {
            int b;
            while ((b = fs.ReadByte()) != -1)
            {
                binaryContent.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
        }
        return binaryContent.ToString();
    }

    static string Komprimieren(string binaryContent)
    {
        if (binaryContent.Length == 0) return "";

        StringBuilder komprimierterInhalt = new StringBuilder();
        char lastChar = binaryContent[0];
        int count = 1;

        for (int i = 1; i < binaryContent.Length; i++)
        {
            if (binaryContent[i] == lastChar)
            {
                count++;
            }
            else
            {
                komprimierterInhalt.Append("{").Append(count).Append(lastChar);
                lastChar = binaryContent[i];
                count = 1;
            }
        }
        komprimierterInhalt.Append("{").Append(count).Append(lastChar);

        return komprimierterInhalt.ToString();
    }

    static string DateiLesen(string s_Datei)
    {
        using (StreamReader sr = new StreamReader(s_Datei))
        {
            return sr.ReadToEnd();
        }
    }

    static string Dekomprimieren(string komprimierterInhalt)
    {
        if (komprimierterInhalt.Length == 0) return "";

        StringBuilder dekomprimierterInhalt = new StringBuilder();
        int i = 0;

        while (i < komprimierterInhalt.Length)
        {
            if (komprimierterInhalt[i] == '{')
            {
                int j = i + 1;
                while (j < komprimierterInhalt.Length && komprimierterInhalt[j] != '{')
                {
                    j++;
                }
                if (j < komprimierterInhalt.Length)
                {
                    int count = int.Parse(komprimierterInhalt.Substring(i + 1, j - i - 1));
                    char value = komprimierterInhalt[j];
                    dekomprimierterInhalt.Append(new string(value, count));
                    i = j + 1;
                }
                else
                {
                    throw new FormatException("Invalid format in compressed string.");
                }
            }
            else
            {
                i++;
            }
        }

        return dekomprimierterInhalt.ToString();
    }

    static void DateiSpeichern(string s_Datei, string inhalt)
    {
        using (StreamWriter sw = new StreamWriter(s_Datei))
        {
            sw.Write(inhalt);
        }
    }
}