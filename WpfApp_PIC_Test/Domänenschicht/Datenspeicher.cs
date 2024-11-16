using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WpfApp_PIC.Domänenschicht
{
    public class DataRegister
    {
        private int[] _register;
        private int[] _bank1;
        private IProgrammCounterUpdate _programmCounterUpdate;

        public DataRegister()
        {
            _register = new int[256];
            _bank1 = new int[10];

            //Startwerte:
            _register[2] = 0;   //PCL
            _register[3] = 24;  //Status
            _register[10] = 0;  //PCLATH
            _register[11] = 0;  //Intcon
            _bank1[1] = 255;   //Option
            _bank1[5] = 31;    //TrisA
            _bank1[6] = 255;   //TrisB
            _bank1[8] = 0;     //EECON1
        }


        public int GetValue(int index)
        {
            if (index == 0)  //Indirect Register Handler
            {
                int fsr = _register[4];
                return _register[fsr];
            }

            else if (StorageOnBank1(index))
                return _bank1[index];

            else 
                return _register[index];
        }

        public void SetValue(int index, int value)
        {
            if (StorageOnBank1(index))
                 _bank1[index] = value;
            else
                _register[index] = value;
        }

        public int GetBit(int index, int bitNumber)
        {
            int val = GetValue(index);
            val = val >> bitNumber;  //The bit to be examined is now at the far right in val
            val = val & 1;
            return val;

        }

        public void SetBit(int index, int bitNumber, bool set)
        {
            int reg = GetValue(index);

            int tmp = 1;
            tmp = tmp << bitNumber;
            if (set == true)
            {
                reg |= tmp;
            }

            else
            {
                tmp = ~tmp;
                reg &= tmp;
            }

            SetValue(index, reg);

            return;
        }

        public int[] GetBank0()
        {
            return _register;
        }

        public int[] GetBank1()
        {
            return _bank1;
        }


        #region private help functions
        private bool StorageOnBank1(int addr)
        {
            if (addr == 1 || addr == 5 || addr == 6 || addr == 8 || addr == 9)
            {
                if (GetBit(3, 5) == 1)      //if(RP0 Bit == 1)
                    return true; ;
            }
            return false;
        }

         
        private void specialRegisterCalled(int index, int value)
        {
            switch(index)
            {
                case 0:
                    SetValue(GetValue(4), value);
                    break;

                case 2:
                    _programmCounterUpdate.PCLUpdate(value);
                    break;

                case 10:
                    _programmCounterUpdate.PCLATHUpdate(value);
                    break;
            }
        }
        #endregion
    }
}
