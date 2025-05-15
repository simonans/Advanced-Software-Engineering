using WpfApp_PIC.Dom�nenschicht;

namespace WpfApp_PIC.Anwednungsschicht
{
    public class ExecutionModule
    {
        private readonly Decoder _decoder;

        public ExecutionModule(Decoder decoder)
        {
            _decoder = decoder;
        }

        public void RunOneInstruction()
        {
            _decoder.Decode();
        }
    }
}
