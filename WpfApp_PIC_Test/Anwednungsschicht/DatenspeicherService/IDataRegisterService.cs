using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht;
public interface IDataRegisterService
{
    int[] GetAllBank0Values();
    int[] GetAllBank1Values();
    int GetValue(int index);
    void SetValue(int index, int value);
    // Event hinzufügen
    event EventHandler StatusChanged;
}

