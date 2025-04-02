using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DataRegisterServices
{
    public class RegularSFRService : ISFRService
    {
        private DataRegister _dataRegister;
        private int _address;


        public event EventHandler ValueChanged;

        public RegularSFRService(DataRegister dataRegister, int address) 
        { 
            _dataRegister = dataRegister;
            _address = address;
        }

        public DataRegister DataRegister
        {
            get { return _dataRegister; }
        }

        public void SetValue(int Value)
        {
            _dataRegister.SetValue(_address, Value);
            OnValueChanged();
        }

        public void SetBit(int BitnUmber)
        {
            _dataRegister.SetBit(_address, BitnUmber, true);
        }

        public void ResetBit(int BitnUmber)
        {
            _dataRegister.SetBit(_address, BitnUmber, false);
        }

        public int GetValue()
        {
            return _dataRegister.GetValueBank0(_address);
        }

        public int GetBit(int BitNumber)
        {
            return _dataRegister.GetBit(_address, BitNumber);
        }


        public virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
