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
        string binaryContent = LesenUndKonvertieren(s_Datei);
        string komprimierterInhalt = Komprimieren(binaryContent);
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
    
}