using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DataRegisterServices
{
    public interface ISFRService
    {
        event EventHandler ValueChanged;
        DataRegister DataRegister { get; }
        int Address { get; }
        void SetValue(int Value);
        int GetValue();
        void OnValueChanged();
    }
}
