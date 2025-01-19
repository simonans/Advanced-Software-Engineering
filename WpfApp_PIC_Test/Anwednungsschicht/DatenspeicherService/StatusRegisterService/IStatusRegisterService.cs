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
    void SetCarryFlag();
    void ResetCarryFlag();

    //int GetDCFlag();
    void SetDCFlag();
    void ResetDCFlag();

    //int GetZeroFlag();
    void SetZeroFlag();
    void ResetZeroFlag();

   
    void SetPDFlag();
    void ResetPDFlag();

   
    void SetTOFlag();
    void ResetTOFlag();


    int GetRP0();
    void SetRP0();

    //int GetRP1();
    //void SetRP1(int value);

    //int GetIRP();
    //void SetIRPD(int value);


}

