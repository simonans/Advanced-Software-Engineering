using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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


    public void SetCarryFlag()
    {
        _dataRegister.SetBit(3, 0, true);
    }

    public void ResetCarryFlag()
    {
        _dataRegister.SetBit(3, 0, false);
    }


    public void SetDCFlag()
    {
        _dataRegister.SetBit(3, 1, true);
    }

    public void ResetDCFlag()
    {
        _dataRegister.SetBit(3, 1, false);
    }


    public void SetZeroFlag()
    {
        _dataRegister.SetBit(3, 2, true);
    }

    public void ResetZeroFlag()
    {
        _dataRegister.SetBit(3, 2, false);
    }


    public void SetPDFlag()
    {
        _dataRegister.SetBit(3, 3, true);
    }

    public void ResetPDFlag()
    {
        _dataRegister.SetBit(3, 3, false);
    }


    public void SetTOFlag()
    {
        _dataRegister.SetBit(3, 4, true);
    }

    public void ResetTOFlag()
    {
        _dataRegister.SetBit(3, 4, false);
    }


    public int GetRP0()
    {
        return _dataRegister.GetBit(3, 5);
    }

    public void SetRP0()
    {
        _dataRegister.SetBit(3, 5, true);
    }

}
