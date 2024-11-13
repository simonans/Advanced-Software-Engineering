using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService.StatusRegisterService
{
    public interface ITMR0RegisterService
    {
        int GetValue();

        void SetValue(int value);

        void increaseValue();
    }
}
