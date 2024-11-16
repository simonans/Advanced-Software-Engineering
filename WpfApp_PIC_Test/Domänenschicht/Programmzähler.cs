using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht
{
    public class Programmzähler
    {
        private int _programcounter;
        private bool _programmCounterExternSet;
        //Wenn PC extern gesetzt wurde (Call, Goto, PCL, PCLATH) muss er nicht nach dem Befehl erhöht werden

        public Programmzähler()
        {
            _programcounter = 0;
            _programmCounterExternSet = false;
        }

        public int GetProgramCounter()
        {
            return _programcounter;
        }

        public void ResetProgramCounter()
        {
            _programcounter = 0;
        }

        public void IncreaseProgramCounter()
        {
            if (_programmCounterExternSet)
                _programmCounterExternSet = false;
            else
                _programcounter++;
        }

        public void SetProgrammCounter(int newValue)
        {
            _programcounter = newValue;
            _programmCounterExternSet = true;
        }
    }
}
