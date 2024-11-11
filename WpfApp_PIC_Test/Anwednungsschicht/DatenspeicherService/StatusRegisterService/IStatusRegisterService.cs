using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace WpfApp_PIC.Anwednungsschicht.DatenspeicherService.StatusRegisterService;
public interface IStatusRegisterService
{
    int GetValue();
    void SetValue(int value);

    //int GetcarryFlag();
    //void SetCarryFlag(int value);

    //int GetDCFlag();
    //void SetDCFlag(int value);

    //int GetZeroFlag();
    //void SetZeroFlag(int value);

    //int GetnPDFlag();
    //void SetnPDFlag(int value);

    //int GetnTOFlag();
    //void SetnTOFlag(int value);

    int GetRP0();
    void SetRP0(int value);

    //int GetRP1();
    //void SetRP1(int value);

    //int GetIRP();
    //void SetIRPD(int value);


}

