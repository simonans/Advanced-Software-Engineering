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
        private ProgrammCounterService _programmCounterService;

        public Decoder(Instructions instructions, ProgrammCounterService programmCounterService)
        {
            _instructions = instructions;
            _programmCounterService = programmCounterService;
        }

        public void Decode()
        { }
    }
}
