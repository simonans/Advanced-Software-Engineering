using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht
{
    static class PICConstants
    {
        //Bank0
        public const int INDIRECT_REGISTER_ADDR = 0;
        public const int TMR0_REGISTER_ADDR = 1;
        public const int PCL_REGISTER_ADDR = 2;
        public const int STATUS_REGISTER_ADDR = 3;
        public const int FSR_REGISTER_ADDR = 4;
        public const int PORTA_REGISTER_ADDR = 5;
        public const int PORTB_REGISTER_ADDR = 6;
        public const int PCLATH_REGISTER_ADDR = 10;
        public const int INTCON_REGISTER_ADDR = 11;

        //Bank1
        public const int OPTION_REGISTER_ADDRR = 1;
        public const int TRISA_REGISTER_ADDR = 5;
        public const int TRISB_REGISTER_ADDR = 6;
        public const int EECON1_REGISTER_ADDR = 8;
        public const int EECON2_REGISTER_ADDR = 9;

        //Status Register
        public const int CARRY_BIT = 0;
        public const int DC_BIT = 1;
        public const int ZEROFLAG_BIT = 2;
        public const int PD_BIT = 3;
        public const int TO_BIT = 4;
        public const int RP0_BIT = 5;
        public const int RP1_BIT = 6;
        public const int IRP_BIT = 7;
    }
}
