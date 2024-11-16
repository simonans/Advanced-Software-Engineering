using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht
{
    public interface IProgrammCounterUpdate
    {
        public void PCLUpdate(int value);

        public void PCLATHUpdate(int value);
    }
}
