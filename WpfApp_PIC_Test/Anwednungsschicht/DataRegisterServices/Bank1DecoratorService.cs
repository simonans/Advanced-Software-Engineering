using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DataRegisterServices
{
    class Bank1DecoratorService : Decorator
    {
        public Bank1DecoratorService(ISFRService successor) : base(successor) { }


        public int GetValueTris()
        {
            return _successor.DataRegister.GetValueBank1(3);
        }
        
    }
}
