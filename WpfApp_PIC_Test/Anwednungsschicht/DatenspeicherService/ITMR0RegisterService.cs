using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService
{
    public interface ITMR0RegisterService
    {
        int GetValue();

        void SetValue(int value);

        void increaseValue();
    }
}
