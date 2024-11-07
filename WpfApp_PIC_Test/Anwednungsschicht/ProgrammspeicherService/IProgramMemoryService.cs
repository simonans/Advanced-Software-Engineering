using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Anwednungsschicht.ProgrammspeicherService;
    public interface IProgramMemoryService
    {
        int GetNumberOfValuesInRegister();
        int GetValue(int index);
        void SetValue(int index, int value);
    }

