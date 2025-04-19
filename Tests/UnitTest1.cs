using WpfApp_PIC;
using WpfApp_PIC.Domänenschicht;
using WpfApp_PIC.Anwednungsschicht;
using WpfApp_PIC.Adapterschicht.ViewModel;
using Xunit;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using WpfApp_PIC.Pluginschicht.LST_File_Reader;
using WpfApp_PIC.Anwendungsschicht.Parser;

namespace Tests
{
    public class UnitTest1
    {

        [Fact]
        public void Test1()
        {
            W_Register w_register_under_test = new W_Register();

            w_register_under_test.SetValue(5);

            Assert.Equal(5, w_register_under_test.GetValue());
        }
    }

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
    }
    #endregion

    #region Adapterschicht
    #endregion

    #region Pluginschicht unter verwendung von Mock-Objekten
    #endregion
}