using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using Moq;
using WpfApp_PIC.Anwendungsschicht.Parser;
using WpfApp_PIC.Pluginschicht.LST_File_Reader;
using WpfApp_PIC.Adapterschicht.Parser;

namespace Tests;

    #region Domänenschicht
    public class DataRegisterTests
    {
        [Fact]
        public void GetValue_ShouldReturnCorrectValue()
        {
            // Arrange
            var dataRegister = new DataRegister();
            dataRegister.SetValue(5, 42);

            // Act
            int value = dataRegister.GetValue(5);

            // Assert
            Assert.Equal(42, value);
        }

        [Fact]
        public void SetValue_ShouldUpdateValueCorrectly()
        {
            // Arrange
            var dataRegister = new DataRegister();

            // Act
            dataRegister.SetValue(10, 99);

            // Assert
            Assert.Equal(99, dataRegister.GetValue(10));
        }

        [Fact]
        public void GetBit_ShouldReturnCorrectBitValue()
        {
            // Arrange
            var dataRegister = new DataRegister();
            dataRegister.SetValue(0, 0b1010); // Binär: 1010

            // Act
            int bitValue = dataRegister.GetBit(0, 1); // Bit 1 sollte 1 sein

            // Assert
            Assert.Equal(1, bitValue);
        }

        [Fact]
        public void SetBit_ShouldUpdateBitCorrectly()
        {
            // Arrange
            var dataRegister = new DataRegister();
            dataRegister.SetValue(0, 0b0000); // Binär: 0000

            // Act
            dataRegister.SetBit(0, 2, true); // Setze Bit 2 auf 1

            // Assert
            Assert.Equal(0b0100, dataRegister.GetValue(0)); // Binär: 0100
        }
        [Fact]
        public void GetValueBank0_ShouldReturnCorrectValue()
        {
            // Arrange
            var dataRegister = new DataRegister();
            dataRegister.SetValue(5, 42);

            // Act
            int value = dataRegister.GetValueBank0(5);

            // Assert
            Assert.Equal(42, value);
        }

        [Fact]
        public void GetValueBank1_ShouldReturnCorrectValue()
        {
            // Arrange
            var dataRegister = new DataRegister();
            dataRegister.SetValue(5, 42);

            // Act
            int value = dataRegister.GetValueBank1(5);

            // Assert
            Assert.Equal(31, value); // Annahme: Bank1 ist initial auf 31 im TRISA-Register gesetzt
        } 
        
    }
#endregion

    #region Anwendungsschicht
    public class DataRegisterServiceTests
    {
        [Fact]
        public void SetValue_ShouldUpdateRegisterValue()
    {
        // Arrange
        var dataRegister = new DataRegister();
        var service = new DataRegisterService(dataRegister);

        // Act
        service.SetValue(5, 42);

        // Assert
        Assert.Equal(42, service.GetValue(5));
    }
        [Fact]
        public void GetAllBank0Values_ShouldReturnAllValues()
    {
        // Arrange
        var dataRegister = new DataRegister();
        var service = new DataRegisterService(dataRegister);

        // Act
        var values = service.GetAllBank0Values();

        // Assert
        Assert.NotNull(values);
        Assert.Equal(256, values.Length); // Annahme: Bank0 hat 128 Werte
    }
    
        [Fact]
        public void GetAllBank1Values_ShouldReturnAllValues()
    {
        // Arrange
        var dataRegister = new DataRegister();
        var service = new DataRegisterService(dataRegister);

        // Act
        var values = service.GetAllBank1Values();

        // Assert
        Assert.NotNull(values);
        Assert.Equal(12, values.Length); // Annahme: Bank1 hat 128 Werte
    }

        [Fact]
        public void ParserReadLSTFileAndWriteInProgramMemory()
    {
        // Arrange
        var mockReader = new Mock<ILST_File_Reader>();
        var programMemory = new ProgramMemory();

        // Setup mit Callback
        mockReader.Setup(r => r.ReadFile(It.IsAny<string>(), It.IsAny<ProgramMemory>()))
                  .Callback<string, ProgramMemory>((filePath, mem) =>
                  {
                      mem.SetValue(0x0001, 0x1234); // Simuliere Schreibvorgang im Mock
                  });

        var parser = new Parser(mockReader.Object);

        // Act
        parser.ReadLstFile("test.lst", programMemory);  //The LST file name doesn't matter in the unit test
                                                        //since the lst file reader is mocked

        // Assert
        Assert.Equal(0x1234, programMemory.GetValue(0x0001));


    }
}
    #endregion

    #region Adapterschicht
    #endregion

    #region Pluginschicht
    public class LSTFileReaderTests
    {
        [Fact]
        public void ReadFile_ShouldHandleNonWhiteSpaceLines()
        {
            // Arrange
            var reader = new LST_File_Reader();
            var programMemory = new ProgramMemory();
            string filePath = "test.lst";

            // Simulieren Sie eine Datei mit einer Zeile ohne Leerzeichen
            File.WriteAllText(filePath, "0001 1234");

            // Act
            reader.ReadFile(filePath, programMemory);

            // Assert
            Assert.Equal(0x1234, programMemory.GetValue(0x0001));
        }
    }
#endregion
