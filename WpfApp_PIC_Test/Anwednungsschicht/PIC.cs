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
    
    private DataRegister _datenspeicher;
    private IProgrammCounterUpdate _programm_counter_update;
    private ProgramCounter _program_counter;
    private ProgramMemory _program_memory;
    private Stack _stack;
    private W_Register _w_register;

    private readonly ILST_File_Reader _reader;

    private Parser _parser;

    private Instructions _instructions;


    public PIC(string filePath)
    {
        _reader = new LST_File_Reader();
        _parser = new Parser(_reader);
        _program_counter = new ProgramCounter();
        _programm_counter_update = new ProgrammCounterService(_program_counter);
        _datenspeicher = new DataRegister(_programm_counter_update);
        _program_memory = new ProgramMemory();
        _w_register = new W_Register();
        _stack = new Stack();

        _parser.ReadLstFile(filePath, _program_memory);
    }
}

