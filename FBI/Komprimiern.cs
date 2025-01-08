//©PaperTobi
using System;
using System.IO;
using System.Text;

class Program
{
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
    static string Entkomprimieren(string s_KomprimierterInhalt)
    {
        StringBuilder sb_BinaryContent = new StringBuilder();
        int i = 0;

        while (i < s_KomprimierterInhalt.Length)
        {
            if (s_KomprimierterInhalt[i] == '{')
            {
                int j = i + 1;
                while (s_KomprimierterInhalt[j] != '}')
                {
                    j++;
                }
                int i_Count = int.Parse(s_KomprimierterInhalt.Substring(i + 1, j - i - 1));
                char c_Char = s_KomprimierterInhalt[j + 1];
                sb_BinaryContent.Append(new string(c_Char, i_Count));
                i = j + 2;
            }
            else
            {
                i++;
            }
        }

        return sb_BinaryContent.ToString();
    }
}