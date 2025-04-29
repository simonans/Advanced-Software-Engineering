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
    public class DataRegister : SimpleStorage
    {
        private const int DATA_REGISTER_SIZE = 256;
        private const int NUMBER_OF_SPECIAL_FUNCTION_REGISTERS = 12;
        private int[] _bank1;

        public DataRegister() : base(DATA_REGISTER_SIZE) 
        {
            _bank1 = new int[NUMBER_OF_SPECIAL_FUNCTION_REGISTERS];

            //Startwerte:
            _register[PICConstants.PCL_REGISTER_ADDR] = 0; _bank1[PICConstants.PCL_REGISTER_ADDR] = 0;
            _register[PICConstants.STATUS_REGISTER_ADDR] = 24; _bank1[PICConstants.STATUS_REGISTER_ADDR] = 24;
            _register[PICConstants.PCLATH_REGISTER_ADDR] = 0;
            _register[PICConstants.INTCON_REGISTER_ADDR] = 0; 
            _bank1[PICConstants.OPTION_REGISTER_ADDRR] = 255;   
            _bank1[PICConstants.TRISA_REGISTER_ADDR] = 31;   
            _bank1[PICConstants.TRISB_REGISTER_ADDR] = 255; 
            _bank1[PICConstants.EECON1_REGISTER_ADDR] = 0;     
        }


        public override int GetValue(int index)
        {
            if (index == PICConstants.INTCON_REGISTER_ADDR) 
            {
                int fsr = _register[PICConstants.FSR_REGISTER_ADDR];
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
        public override void SetValue(int index, int value)
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
                    HandleSpecialRegister(index, value);
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
            if (addr == PICConstants.OPTION_REGISTER_ADDRR || 
                addr == PICConstants.TRISA_REGISTER_ADDR || 
                addr == PICConstants.TRISB_REGISTER_ADDR || 
                addr == PICConstants.EECON1_REGISTER_ADDR ||
                addr == PICConstants.EECON2_REGISTER_ADDR)
                return true;
            else
                return false;
        }
        private bool StorageOnBank1(int addr)
        {
            if (DifferentStorageOnBank1(addr))
            {
                if (RP0BitIsSet())     
                    return true; ;
            }
            return false;
        }
        private bool WritingOnBank1()
        {
            if (RP0BitIsSet())    
                return true;
            else
                return false;
        }

        private int GetProgramCounter()
        {
            return GetValue(PICConstants.PCL_REGISTER_ADDR);
        }

        private void SetProgramCounter(int tmp)
        {
            _register[PICConstants.PCL_REGISTER_ADDR] = tmp;
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

        private void HandleSpecialRegister(int register, int value)
        {
            switch (register)
            {
                case 0:
                    int indirectRegister = GetValue(PICConstants.FSR_REGISTER_ADDR);
                    if (indirectRegister != register)
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

        private bool RP0BitIsSet()
        {
            return (GetBit(PICConstants.STATUS_REGISTER_ADDR, PICConstants.RP0_BIT) == 1);
        }
        #endregion
    }
}
