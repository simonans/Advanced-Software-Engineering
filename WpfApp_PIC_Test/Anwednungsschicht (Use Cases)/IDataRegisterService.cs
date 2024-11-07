using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht;
public interface IDataRegisterService
{
    int[] GetRegisterValues();
    int GetValue(int index);
    void SetValue(int index, int value);
}

