using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
public class StatusRegisterService
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



    public int GetCarryFlag()
    {
        return _dataRegister.GetBit(3, 0);
    }

    public void SetCarryFlag()
    {
        _dataRegister.SetBit(3, 0, true);
    }

    public void ResetCarryFlag()
    {
        _dataRegister.SetBit(3, 0, false);
    }

    public int GetDCFlag()
    {
        return _dataRegister.GetBit(3, 1);
    }

    public void SetDCFlag()
    {
        _dataRegister.SetBit(3, 1, true);
    }

    public void ResetDCFlag()
    {
        _dataRegister.SetBit(3, 1, false);
    }

    public int GetZeroFlag()
    {
        return _dataRegister.GetBit(3, 2);
    }

    public void SetZeroFlag()
    {
        _dataRegister.SetBit(3, 2, true);
    }

    public void ResetZeroFlag()
    {
        _dataRegister.SetBit(3, 2, false);
    }

    /*Da das Time-Out-Status-Bit und das Power-Down-Status-Bit inverse Bits sind,
      gleicht das setzen dem speichern einer 0
      und das rücksetzen dem speichern einer 1
      im jeweiligen Status-Bit*/
    public int GetPDFlag()
    {
        return _dataRegister.GetBit(3, 3);
    }

    public void SetPDFlag()
    {
        _dataRegister.SetBit(3, 3, false);
    }

    public void ResetPDFlag()
    {
        _dataRegister.SetBit(3, 3, true);
    }

    public int GetTOFlag()
    {
        return _dataRegister.GetBit(3, 4);
    }

    public void SetTOFlag()
    {
        _dataRegister.SetBit(3, 4, false);
    }

    public void ResetTOFlag()
    {
        _dataRegister.SetBit(3, 4, true);
    }

    public int GetRP0()
    {
        return _dataRegister.GetBit(3, 5);
    }

    public void SetRP0()
    {
        _dataRegister.SetBit(3, 5, true);
    }
    public void ResetRP0()
    {
        _dataRegister.SetBit(3, 5, false);
    }

}
