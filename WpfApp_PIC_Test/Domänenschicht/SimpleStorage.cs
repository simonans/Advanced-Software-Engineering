using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht;
public class SimpleStorage
{
    protected int[] _register;

    public SimpleStorage(int size)
    {  
        _register = new int[size];
    }

    public virtual int GetValue(int index)
    {
        return _register[index];
    }

    public virtual void SetValue(int index, int value)
    {
        _register[index] = value;
    }
}

