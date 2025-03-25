using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.DataRegisterServices
{
    abstract class Decorator : ISFRService
    {
        protected ISFRService _successor;
        public event EventHandler ValueChanged
        {
            add { _successor.ValueChanged += value; }
            remove { _successor.ValueChanged -= value; }
        }

        public Decorator(ISFRService successor)
        {
            _successor = successor;
        }

        public DataRegister DataRegister { get { return _successor.DataRegister; } }
        public int Address { get { return _successor.Address; } }

        public void SetValue(int Value) { _successor.SetValue(Value); }
        public void SetBit(int BitNumber) { _successor.SetBit(BitNumber); }
        public void ResetBit(int BitNumber) { _successor.ResetBit(BitNumber); }
        public int GetValue()  {return _successor.GetValue(); }
        public int GetBit(int BitNumber) { return _successor.GetBit(BitNumber); }
        public void IncreaseValue() { _successor.IncreaseValue(); }
    }
}
