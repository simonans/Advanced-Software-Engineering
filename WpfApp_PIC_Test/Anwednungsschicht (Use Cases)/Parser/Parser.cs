using WpfApp_PIC.Anwendungsschicht.Parser;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Pluginschicht.LST_File_Reader;

namespace WpfApp_PIC.Adapterschicht.Parser;
public class Parser : IParser
{
    private readonly ILST_File_Reader _reader;
    public Parser(ILST_File_Reader reader)
    {
        _reader = reader;
    }

    public void ReadLstFile(string filePath, ProgramMemory programmspeicher)
    {
        _reader.ReadLstFile(filePath, programmspeicher);
    }


}