using System;
using System.IO;

class BinaryCompress
{
    static void Main()
    {
        string s_Datei = "Datei.txt";
        string s_DateiKomprimiert = "komprimierteDatei.txt";
        DateiErstellen(s_Datei);
        string content = LesenUndKonvertieren(s_Datei);
        string komprimierterInhalt = Komprimieren(content);
        DateiSpeichern(s_DateiKomprimiert, komprimierterInhalt);
        Console.WriteLine("Datei komprimiert und gespeichert als " + s_DateiKomprimiert);
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
        string content = "";
        using (FileStream fs = new FileStream(s_Datei, FileMode.Open, FileAccess.Read))
        {
            int b;
            while ((b = fs.ReadByte()) != -1)
            {
                content += (char)b;
            }
        }
        return content;
    }

    static string Komprimieren(string content)
    {
        if (content.Length == 0) return "";

        string komprimierterInhalt = "";
        foreach (char c in content)
        {
            komprimierterInhalt += "{1" + c;
        }
        return komprimierterInhalt;
    }

    static void DateiSpeichern(string s_DateiKomprimiert, string inhalt)
    {
        StreamWriter o_SW = new StreamWriter(s_DateiKomprimiert);
        o_SW.Write(inhalt);
        o_SW.Close();
    }
}