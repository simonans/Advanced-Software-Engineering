using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PicSimu.Model
{
    class PIC
    {
        public EEPROM eeprom;
        public Programmspeicher programmspeicher; //////////////////////////auf public für Tests an der Struktur des ViewModels//////////////////////////////////////////////// 
        public Datenspeicher datenspeicher;
        public Stack stack;
        public Parser parser;
        public W_Register w_Register;
        private Decoder decoder;


        public PIC(string filePath)
        {
            eeprom = new EEPROM();
            programmspeicher = new Programmspeicher();
            datenspeicher = new Datenspeicher();
            stack = new Stack();
            parser = new Parser();
            decoder = new Decoder();
            w_Register = new W_Register();

            this.parser.ReadLstFile(filePath, this.programmspeicher);
        }

        public PIC()
        {
            eeprom = new EEPROM();
            programmspeicher = new Programmspeicher();
            datenspeicher = new Datenspeicher();
            stack = new Stack();
            parser = new Parser();
            decoder = new Decoder();
            w_Register = new W_Register();
        }


        public void commandCycle()
        {
            decoder.selctor(programmspeicher, w_Register, datenspeicher, stack, eeprom);
            datenspeicher.updateClockAfterCycle(stack, eeprom);
        }
    }
}
