using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwendungsschicht.Parser;
public interface IParser
{
    public void ReadLstFile(string filePath, ProgramMemory programmspeicher);
}
