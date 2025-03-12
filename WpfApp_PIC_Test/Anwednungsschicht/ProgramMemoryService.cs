using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht;

public class ProgramMemoryService 
{
    private readonly ProgramMemory _ProgramMemory;

    public ProgramMemoryService(ProgramMemory ProgramMemory)
    {
        _ProgramMemory = ProgramMemory;
    }

    public int GetNumberOfValuesInRegister()
    {
        return _ProgramMemory.GetNumberOfValuesInRegister();
    }

    public int GetValue(int index)
    {
        return _ProgramMemory.GetValue(index);
    }

    public void SetValue(int index, int value)
    {
        _ProgramMemory.SetValue(index, value);
    }
}

