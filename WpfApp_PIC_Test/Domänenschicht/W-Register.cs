using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht;

public class W_Register
{
    private int _value;

    public W_Register()
    {
        _value = 0;
    }

    public void SetValue(int value)
    {
        _value = value;
    }

    public int GetValue()
    {
        return _value;
    }

}
