using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht;

public class ProgramMemory
{
    private int[] _register;
    private int numberOfValuesInRegister;

    public ProgramMemory()
    {
        _register = new int[1024];
    }

    public int GetValue(int index)
    {
        return _register[index];
    }

    public void SetValue(int index, int value)
    {
        _register[index] = value;
    }

    public int GetNumberOfValuesInRegister()
    {
        return numberOfValuesInRegister;
    }


}
