using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Pluginschicht.LST_File_Reader;
public interface ILST_File_Reader
{
    public void ReadLstFile(string filePath, ProgramMemory programmspeicher);
}
