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
        string s_BinaryContent = LesenUndKonvertieren(s_Datei);
        string s_KomprimierterInhalt = Komprimieren(s_BinaryContent);
    }

    static string LesenUndKonvertieren(string s_Datei)
    {
        StringBuilder sb_BinaryContent = new StringBuilder();
        using (FileStream fs_Datei = new FileStream(s_Datei, FileMode.Open, FileAccess.Read))
        {
            int i_Byte;
            while ((i_Byte = fs_Datei.ReadByte()) != -1)
            {
                sb_BinaryContent.Append(Convert.ToString(i_Byte, 2).PadLeft(8, '0'));
            }
        }
        return sb_BinaryContent.ToString();
    }

    static string Komprimieren(string s_BinaryContent)
    {
        if (s_BinaryContent.Length == 0) return "";

        StringBuilder sb_KomprimierterInhalt = new StringBuilder();
        char c_LastChar = s_BinaryContent[0];
        int i_Count = 1;

        for (int i = 1; i < s_BinaryContent.Length; i++)
        {
            if (s_BinaryContent[i] == c_LastChar)
            {
                i_Count++;
            }
            else
            {
                sb_KomprimierterInhalt.Append("{").Append(i_Count).Append(c_LastChar);
                c_LastChar = s_BinaryContent[i];
                i_Count = 1;
            }
        }
        sb_KomprimierterInhalt.Append("{").Append(i_Count).Append(c_LastChar);

        return sb_KomprimierterInhalt.ToString();
    }
}