using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht;
public class W_RegisterService
{
    private readonly W_Register _w_Register;
    public event EventHandler ValueChanged;

    public W_RegisterService(W_Register w_Register)
    {
        _w_Register = w_Register;
    }

    public void SetValue(int vlaue)
    {
        _w_Register.SetValue(vlaue);
        OnValueChanged();
    }

    public int GetValue()
    {
        return _w_Register.GetValue();
    }

    protected virtual void OnValueChanged()
    {
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }

}
