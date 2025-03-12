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
    private DataRegisterService _data_register_service;
    private PCLATHRegisterService _pclath_register_service;
    private PCLRegisterService _pcl_register_service;
    private StatusRegisterService _status_register_service;
    private TMR0RegisterService _tmr0_register_service;
    private Instructions _instructions;
    private ProgramMemoryService _program_memory_service;
    private StackService _stack_service;
    private W_RegisterService _w_register_service;
    

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
        _data_register_service = new DataRegisterService(_data_register);
        _pclath_register_service = new PCLATHRegisterService(_data_register);
        _pcl_register_service = new PCLRegisterService(_data_register);
        _status_register_service = new StatusRegisterService(_data_register);
        _tmr0_register_service = new TMR0RegisterService(_data_register);
        _instructions = new Instructions(_data_register, _w_register, _stack, _program_counter, _status_register_service, _tmr0_register_service);
        _program_memory_service = new ProgramMemoryService(_program_memory);
        _stack_service = new StackService(_stack);
        _w_register_service = new W_RegisterService(_w_register);

        _parser.ReadLstFile(filePath, _program_memory);
    }
}

