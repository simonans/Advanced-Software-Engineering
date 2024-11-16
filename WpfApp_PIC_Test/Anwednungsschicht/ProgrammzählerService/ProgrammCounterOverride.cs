using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.ProgrammzählerService
{
    public class ProgrammCounterOverride : IProgrammCounterUpdate
    {
        private Programmzähler _programcounter;

        public ProgrammCounterOverride(Programmzähler programcounter)
        {
            this._programcounter = programcounter;
        }

        public void PCLUpdate(int value)
        {
            int tmp = _programcounter.GetProgramCounter();
            tmp &= 0xFF00;  //Set Lowbyte to zero
            value &= 0xFF;
            tmp |= value;
            _programcounter.SetProgrammCounter(tmp);
        }

        public void PCLATHUpdate(int value)
        {
            int tmp = _programcounter.GetProgramCounter();
            tmp &= 0xE0FF;  //Set Upper Five Bits to zero
            value = value << 8;
            tmp |= value;
            _programcounter.SetProgrammCounter(tmp);
            
        }
    }
    
}

