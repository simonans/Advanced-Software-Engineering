using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_PIC.Anwendungsschicht.Parser;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Pluginschicht.LST_File_Reader
{

    public class LST_File_Reader : ILST_File_Reader
    {

        public LST_File_Reader() { }

        public void ReadFile(string filePath, ProgramMemory programmspeicher)
        {
            if (File.Exists(filePath))
            {
                MessageBox.Show("Die Datei existiert.");

                try
                {

                    using (StreamReader sr = new StreamReader(filePath))
                    {

                        string line;
                        int decimalNumber = 0;
                        int index = 0;

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (IsWhiteSpace(line, 0) == false)
                            {
                                // Befehlsnummer aus LST Datei als Index im Programmspeicher verwenden
                                string hexNumber1 = line.Substring(0, 4);
                                index = Convert.ToInt32(hexNumber1, 16);
                                // Befehlscode in int umwandeln 
                                string hexNumber2 = line.Substring(5, 4);
                                decimalNumber = Convert.ToInt32(hexNumber2, 16);
                                // Befehlscode an jeweiligem Index speichern (nach Reihenfolge des Einelesens)
                                programmspeicher.SetValue(index, decimalNumber);
                            }
                        }
                    }
                }


                catch (Exception e)
                {
                    Console.WriteLine("Fehler beim Lesen der Datei: " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("Die Datei existiert nicht.");
            }
        }
        private bool IsWhiteSpace(string line, int currentIndex)
        {
            bool isWhiteSpace = false;
            if (line.ElementAt(currentIndex) == ' ')
            {
                isWhiteSpace = true;
            }
            return isWhiteSpace;
        }
    }
}
