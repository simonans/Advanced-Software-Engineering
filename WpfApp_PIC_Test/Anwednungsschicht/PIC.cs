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
    

    public PIC(string filePath)
    {
        _program_counter = new ProgramCounter();
        _programm_counter_update = new ProgrammCounterService(_program_counter);
        _data_register = new DataRegister(_programm_counter_update);
        _program_memory = new ProgramMemory();
        _w_register = new W_Register();
        _stack = new Stack();

        _reader = new LST_File_Reader();
        _parser = new Parser(_reader);
        _instructions = new Instructions
            (_data_register, _w_register, _stack, _program_counter, new StatusRegisterService(_data_register), new TMR0RegisterService(_data_register));
        _decoder = new Decoder(_instructions, new ProgrammCounterService(_program_counter));

        _parser.ReadLstFile(filePath, _program_memory);
    }


    public void RunOneInstruction()
    {
        _decoder.Decode();
    }

}

