using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht___Datenmodel
{
    public class Programmzähler
    {
        private int _programcounter;

        public Programmzähler()
        {
            _programcounter = 0;
        }

        public void ResetProgramCounter()
        {
            _programcounter = 0;
        }

        public void IncreaseProgramCounter()
        {
            _programcounter++;
        }

        public void SetProgrammCounter(int newValue)
        {
            _programcounter = newValue;
        }
    }
}
