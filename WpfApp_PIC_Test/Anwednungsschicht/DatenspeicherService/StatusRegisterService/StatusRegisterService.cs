using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService.StatusRegisterService;
public class StatusRegisterService : IStatusRegisterService
{
    private readonly DataRegister _dataRegister;
    public StatusRegisterService(DataRegister dataRegister) 
    {
        _dataRegister = dataRegister;
    }

    public int GetValue()
    {
        return _dataRegister.GetValue(3);
    }
    public void SetValue(int value)
    {
        _dataRegister.SetValue(3, value);
    }

    public int GetRP0()
    {
        return _dataRegister.GetBit(3, 5);
    }

    public void SetRP0(int value)
    {
        _dataRegister.SetBit(3, 5, true);
    }

}
