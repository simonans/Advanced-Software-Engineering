using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht
{
    public class ProgramCounterService : IProgrammCounterUpdate
    {
        private ProgramCounter _programcounter;

        public ProgramCounterService(ProgramCounter programcounter)
        {
            _programcounter = programcounter;
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

        public int GetPC()
        {
            return _programcounter.GetProgramCounter();
        }

        public void SetPC(int newValue)
        {
            _programcounter.SetProgrammCounter(newValue);
        }
    }

}

