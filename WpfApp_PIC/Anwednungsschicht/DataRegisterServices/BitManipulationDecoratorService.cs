using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DataRegisterServices
{
    class BitManipulationDecoratorService : Decorator
    {
        public BitManipulationDecoratorService(ISFRService successor) : base(successor) { }

        public void SetBit(int BitNumber)
        {
            _successor.DataRegister.SetBit(_successor.Address, BitNumber, true);
        }

        public void ResetBit(int BitNumber)
        {
            _successor.DataRegister.SetBit(_successor.Address, BitNumber, false);
        }

        public int GetBit(int BitNumber)
        {
            return _successor.DataRegister.GetBit(_successor.Address, BitNumber);
        }
    }
}
