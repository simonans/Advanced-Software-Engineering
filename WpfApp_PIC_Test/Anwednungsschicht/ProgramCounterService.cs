using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht
{
    public class ProgramCounterService /*: IProgrammCounterUpdate*/
    {
        private ProgramCounter _programcounter;
        public event EventHandler ValueChanged;

        public ProgramCounterService(ProgramCounter programcounter)
        {
            _programcounter = programcounter;
        }

        /*public void PCLUpdate(int value)
        {
            int tmp = _programcounter.GetProgramCounter();
            tmp &= 0xFF00;  //Set Lowbyte to zero
            value &= 0xFF;
            tmp |= value;
            _programcounter.SetProgrammCounter(tmp);
            OnValueChanged();
        }

        public void PCLATHUpdate(int value)
        {
            int tmp = _programcounter.GetProgramCounter();
            tmp &= 0xE0FF;  //Set Upper Five Bits to zero
            value = value << 8;
            tmp |= value;
            _programcounter.SetProgrammCounter(tmp);
            OnValueChanged();
        }*/

        public int GetPC()
        {
            return _programcounter.GetProgramCounter();
        }

        public void SetPC(int newValue)
        {
            _programcounter.SetProgrammCounter(newValue);
            MessageBox.Show("Test");
            OnValueChanged();
        }

        public void IncreasePC()
        {
            _programcounter.IncreaseProgramCounter();
            MessageBox.Show("Test");
            OnValueChanged();
        }
        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}

