using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht;
    public class DataRegister
    {
        private readonly int[] _register;

        public DataRegister()
        {
        _register = new int[80];
        }

        public int GetValue(int index)
        {
            return _register[index];
        }

        public void SetValue(int index, int value)
        {
            _register[index] = value;
        }

        public int[] GetRegisterValues()
        {
            return _register;
        }
    }
