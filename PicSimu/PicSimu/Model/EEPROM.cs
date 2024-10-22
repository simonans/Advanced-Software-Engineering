using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace PicSimu.Model
{
    
    class EEPROM
    {
        private string eepromFilePath = "D:\\Git\\source\\repos\\PIC-Simulator\\EEPROM\\eeprom.txt";

        private int[] storageField;
        public EEPROM() 
        {
            storageField = new int[64];

            //for (int j = 0; j < storageField.Length; j++)
            //{
            //    storageField[j] = j;
            //}

            //using (StreamWriter writer = new StreamWriter(eepromFilePath))
            //{
            //    foreach (int num in storageField)
            //    {
            //        writer.WriteLine(num);
            //    }
            //}

            if (File.Exists(eepromFilePath))
            {
                using (StreamReader reader = new StreamReader(eepromFilePath))
                {
                    for (int i = 0; i < storageField.Length; i++)
                    {
                        string line = reader.ReadLine();
                        storageField[i] = int.Parse(line);
                    }
                }
            }
            else
            {
                //initial leer
            }
            
        }

        public void writeIn(int adress, int data)
        {
            storageField[adress] = data;

            using (StreamWriter writer = new StreamWriter(eepromFilePath))
            {
                foreach (int num in storageField)
                {
                    writer.WriteLine(num);
                }
            }

        }

        public int read(int adress) 
        { 
            return storageField[adress];
        }
    }
}
