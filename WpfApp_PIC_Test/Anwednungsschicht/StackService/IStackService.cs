using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Anwednungsschicht.StackService;
public interface IStackService
{
void Push(int value);

int Pop();
}

