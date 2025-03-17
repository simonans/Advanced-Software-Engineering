using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Adapterschicht.Parser;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using WpfApp_PIC.Anwendungsschicht;
using WpfApp_PIC.Anwendungsschicht.Parser;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Pluginschicht.LST_File_Reader;

namespace WpfApp_PIC.Anwednungsschicht;
internal class PIC
{
    //Domänenschicht
    private DataRegister _data_register;
    private IProgrammCounterUpdate _programm_counter_update;
    private ProgramCounter _program_counter;
    private ProgramMemory _program_memory;
    private Stack _stack;
    private W_Register _w_register;

    //Anwendungsschicht
    private readonly ILST_File_Reader _reader;
    private Parser _parser;
    private Instructions _instructions;
    private Decoder _decoder;


    //Hier dann noch Adapterschicht und Pluginschicht


    public PIC(string filePath, Parser parser)
    {
        _stack = new Stack();
        _w_register = new W_Register();
        _program_memory = new ProgramMemory();
        _program_counter = new ProgramCounter();
        _data_register = new DataRegister(new ProgramCounterService(_program_counter));
        
        _parser = parser;

        _instructions = new Instructions
            (_data_register, _w_register, _stack, _program_counter, new StatusRegisterService(_data_register), new TMR0RegisterService(_data_register));
        _decoder = new Decoder(_instructions, new ProgramCounterService(_program_counter), new ProgramMemoryService(_program_memory));

        _parser.ReadLstFile(filePath, _program_memory);
    }
    
    public DataRegister GetDataRegister()
    {
        return _data_register;
    }

    public Stack GetStack()
    {
        return _stack;
    }

    public W_Register GetW_Register()
    {
        return _w_register;
    }

    public ProgramMemory GetProgramMemory()
    {
        return _program_memory;
    }

    public ProgramCounter GetProgramCounter()
    {
        return _program_counter;
    }

    //public void RunOneInstruction()
    //{
    //    _decoder.Decode();
    //}

}

