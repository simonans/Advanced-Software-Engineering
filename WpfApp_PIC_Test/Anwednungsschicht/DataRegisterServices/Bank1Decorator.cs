using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Anwednungsschicht.DataRegisterServices
{
    class Bank1Decorator : Decorator
    {
        public Bank1Decorator(ISFR successor) : base(successor) { }

        public int GetValueTris()
        {
            return _successor.DataRegister.GetValueBank1(3);
        }
        
    }
}
