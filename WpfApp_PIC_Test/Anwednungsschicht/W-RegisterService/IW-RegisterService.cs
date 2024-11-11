using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Anwednungsschicht.W_RegisterService;

public interface IW_RegisterService
{
    public void SetValue(int value);

    public int GetValue();
}

