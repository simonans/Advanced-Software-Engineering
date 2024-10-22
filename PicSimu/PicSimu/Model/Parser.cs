using PicSimu.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;


namespace PicSimu.Model
{
    public class LstFileLine : BindingMechanismBase
    {
        private bool _breakpoint;
        private string _line;

        public bool Breakpoint
        {
            get { return _breakpoint; }
            set
            {
                if (_breakpoint != value)
                {
                    _breakpoint = value;
                    OnPropertyChanged(nameof(Breakpoint));
                }
            }
        }

        public string Line
        {
            get { return _line; }
            set
            {
                if (_line != value)
                {
                    _line = value;
                    OnPropertyChanged(nameof(Line));
                }
            }
        }
    }
    
    class Parser
    {
        public List<LstFileLine>? LSTFileLinePerLine { get; set; }

        public Parser()
        {
            LSTFileLinePerLine = new List<LstFileLine>();
        }

        private bool isWhiteSpace(string line, int currentIndex)
        {
            bool isWhiteSpace = false;
            if (line.ElementAt(currentIndex) == ' ')
            {
                isWhiteSpace = true;
            }
            return isWhiteSpace;
        }



        public void ReadLstFile(string filePath, Programmspeicher programmspeicher)
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
                            LSTFileLinePerLine.Add(new LstFileLine() { Line = line });
                            //LSTFileLinePerLine.Add(line);

                            if (isWhiteSpace(line, 0) == false)
                            {
                                // Befehlsnummer aus LST Datei als Index im Programmspeicher verwenden
                                string hexNumber1 = line.Substring(0, 4);
                                index = Convert.ToInt32(hexNumber1, 16);
                                // Befehlscode in int umwandeln 
                                string hexNumber2 = line.Substring(5, 4);
                                decimalNumber = Convert.ToInt32(hexNumber2, 16);
                                // Befehlscode an jeweiligem Index speichern (nach Reihenfolge des Einelesens)
                                programmspeicher.newValueInProgramStorage(decimalNumber, index);
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
    }
}
