using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService;

public class DataRegisterService 
{
    private readonly DataRegister _dataRegister;
    public event EventHandler StatusChanged;

    public DataRegisterService(DataRegister dataRegister)
    {
        _dataRegister = dataRegister;
    }

    public int[] GetAllBank0Values()
    {
        return _dataRegister.GetBank0();
    }

    public int[] GetAllBank1Values()
    {
        return _dataRegister.GetBank1();
    }

    public int GetValue(int index)
    {
        return _dataRegister.GetValue(index);
    }

    public void SetValue(int index, int value)
    {
        _dataRegister.SetValue(index, value);
        OnStatusChanged();
    }

    public int GetBit(int index, int bitNumber)
    {
        return _dataRegister.GetBit(index, bitNumber);
    }

    public void SetBit(int index, int bitNumber, bool set)
    {
        _dataRegister.SetBit(index, bitNumber, set);
        OnStatusChanged();
    }


    protected virtual void OnStatusChanged()
    {
        StatusChanged?.Invoke(this, EventArgs.Empty);
    }
}


