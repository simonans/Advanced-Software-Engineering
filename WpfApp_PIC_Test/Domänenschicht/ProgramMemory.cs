using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht;

public class ProgramMemory : SimpleStorage
{
    private const int PROGRAM_MEMORY_SIZE = 1024;
    private int _numberOfValuesInRegister;

    public ProgramMemory() : base(PROGRAM_MEMORY_SIZE)
    {
        _numberOfValuesInRegister = 0;
    }

    public override void SetValue(int index, int value)
    {
        base.SetValue(index, value);
        _numberOfValuesInRegister++;
    }

    public int GetNumberOfValuesInRegister()
    {
        return _numberOfValuesInRegister;
    }
}
