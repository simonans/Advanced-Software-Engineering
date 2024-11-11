using WpfApp_PIC.Anwendungsschicht.Parser;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Pluginschicht.LST_File_Reader;

namespace WpfApp_PIC.Adapterschicht.Parser;
public class Parser
{
    private readonly IParser _reader;
    public Parser(IParser reader)
    {
        _reader = reader;
    }

    public void ReadLstFile(string filePath, ProgramMemory programmspeicher)
    {
        _reader.ReadFile(filePath, programmspeicher);
    }


}