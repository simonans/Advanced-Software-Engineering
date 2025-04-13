using WpfApp_PIC;
using WpfApp_PIC.Domänenschicht;

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
}