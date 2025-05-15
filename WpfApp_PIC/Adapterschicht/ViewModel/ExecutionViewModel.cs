using System.Windows.Input;
using WpfApp_PIC.Anwednungsschicht;

namespace WpfApp_PIC.Adapterschicht.ViewModel
{
    public class ExecutionViewModel : ViewModelBase
    {
        private readonly ExecutionModule _executionModule;

        public ExecutionViewModel(ExecutionModule executionModule)
        {
            _executionModule = executionModule;
            RunOneInstructionCommand = new RelayCommand(ExecuteRunOneInstruction);
        }

        public ICommand RunOneInstructionCommand { get; }

        private void ExecuteRunOneInstruction()
        {
            _executionModule.RunOneInstruction();
        }
    }
}
