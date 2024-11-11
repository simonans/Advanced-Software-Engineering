using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Adapterschicht.Parser;
using WpfApp_PIC.Anwendungsschicht;
using WpfApp_PIC.Anwendungsschicht.Parser;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Pluginschicht.LST_File_Reader;

namespace WpfApp_PIC.Anwednungsschicht;
internal class PIC
{
    private readonly IParser _reader;

    private Parser _parser;
    private DataRegister _datenspeicher;
    private ProgramMemory _programmspeicher;
    private W_Register _w_Register;
    private Stack _stack;

    public PIC(string filePath)
    {
        _reader = new LST_File_Reader();
        _parser = new Parser(_reader);
        _datenspeicher = new DataRegister();
        _programmspeicher = new ProgramMemory();
        _w_Register = new W_Register();
        _stack = new Stack();

        _parser.ReadLstFile(filePath, _programmspeicher);
    }
}

