using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht;

public class DataRegisterService : IDataRegisterService
{
    private readonly DataRegister _dataRegister;

    public DataRegisterService(DataRegister dataRegister)
    {
        _dataRegister = dataRegister;
    }

    public int[] GetRegisterValues()
    {
        return _dataRegister.GetRegisterValues();
    }

    public int GetValue(int index)
    {
        return _dataRegister.GetValue(index);
    }

    public void SetValue(int index, int value)
    {
        _dataRegister.SetValue(index, value);
    }
}


