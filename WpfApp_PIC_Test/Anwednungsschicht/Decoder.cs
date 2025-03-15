using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Anwednungsschicht
{
    internal class Decoder
    {
        private Instructions _instructions;
        private ProgramCounterService _ProgramCounterService;

        public Decoder(Instructions instructions, ProgramCounterService ProgramCounterService)
        {
            _instructions = instructions;
            _ProgramCounterService = ProgramCounterService;
        }

        public void Decode()
        { }
    }
}
