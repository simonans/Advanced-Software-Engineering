using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WpfApp_PIC.Domänenschicht
{
   
    public class DataRegister
    {
        const int NUMBER_OF_SPECIAL_FUNCTION_REGISTERS = 12;

        private int[] _register;
        private int[] _bank1;
        /*private IProgrammCounterUpdate _programmCounterUpdate;*/

        public DataRegister(/*IProgrammCounterUpdate programmCounterUpdate*/)
        {
            _register = new int[256];
            _bank1 = new int[12];

            //Startwerte:
            _register[2] = 0; _bank1[2] = 0;   //PCL
            _register[3] = 24; _bank1[3] = 24;  //Status
            _register[10] = 0; //PCLATH
            _register[11] = 0;  //Intcon
            _bank1[1] = 255;   //Option
            _bank1[5] = 31;    //TrisA
            _bank1[6] = 255;   //TrisB
            _bank1[8] = 0;     //EECON1

            /*_programmCounterUpdate = programmCounterUpdate;*/
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

        public int GetValueBank0(int index)
        {
            return _register[index];
        }
        public int GetValueBank1(int index)
        {
            return _bank1[index];
        }
        public void SetValue(int index, int value)
        {
            if (DifferentStorageOnBank1(index) && WritingOnBank1()) //nur auf Bank1 schreiben
                _bank1[index] = value;
            else if (DifferentStorageOnBank1(index) && !WritingOnBank1()) //nur auf Bank 0 schreiben
                _register[index] = value;
            else // = else if ((!DifferentStorageOnBank1(index) && WritingOnBank1()) || (!DifferentStorageOnBank1(index) && !WritingOnBank1()))
            {
                _register[index] = value;

                if (index < NUMBER_OF_SPECIAL_FUNCTION_REGISTERS)
                {
                    _bank1[index] = value;
                    SpecialRegisterHandler(index, value);
                }
            }
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
        private bool DifferentStorageOnBank1(int addr)
        {
            if (addr == 1 || addr == 5 || addr == 6 || addr == 8 || addr == 9)
                return true;
            else
                return false;
        }
        private bool StorageOnBank1(int addr)
        {
            if (DifferentStorageOnBank1(addr))
            {
                if (GetBit(3, 5) == 1)      //if(RP0 Bit == 1)
                    return true; ;
            }
            return false;
        }
        private bool WritingOnBank1()
        {
            if (GetBit(3, 5) == 1)      //if(RP0 Bit == 1)
                return true;
            else
                return false;
        }

        private int GetProgramCounter()
        {
            return GetValue(2);
        }

        private void SetProgramCounter(int tmp)
        {
            _register[2] = tmp;
        }

        public void PCLUpdate(int value)
        {
            int tmp = GetProgramCounter();
            tmp &= 0xFF00;  //Set Lowbyte to zero
            value &= 0xFF;
            tmp |= value;
            SetProgramCounter(tmp);
        }

        public void PCLATHUpdate(int value)
        {
            int tmp = GetProgramCounter();
            tmp &= 0xE0FF;  //Set Upper Five Bits to zero
            value = value << 8;
            tmp |= value;
            SetProgramCounter(tmp);
        }

        private void SpecialRegisterHandler(int register, int value)
        {
            switch (register)
            {
                case 0:
                    int indirectRegister = GetValue(4);
                    //if (indirectRegister != 0)
                        SetValue(indirectRegister, value);   //Otherwise we have an endless loop
                    break;

                case 2:
                    PCLUpdate(value);
                    break;

                case 10:
                    PCLATHUpdate(value);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
