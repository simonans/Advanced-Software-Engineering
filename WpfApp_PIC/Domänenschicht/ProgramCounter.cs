﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht
{
    public class ProgramCounter
    {
        private int _programcounter;
        private bool _programmCounterExternSet;
        //Wenn PC extern gesetzt wurde (Call, Goto, PCL, PCLATH) muss er nicht nach dem Befehl erhöht werden

        public ProgramCounter()
        {
            _programcounter = 0;
            _programmCounterExternSet = false;
        }

        public int GetProgramCounter()
        {
            return _programcounter;
        }

        public void ResetProgramCounter()
        {
            _programcounter = 0;
        }

        #region Increment ProgramCounter
        public void IncreaseProgramCounter()
        {
            if (IsExternSet())
            {
                ResetExternSetFlag();
            }
            IncrementCounter();
        }

        private bool IsExternSet() => _programmCounterExternSet;

        private void ResetExternSetFlag() => _programmCounterExternSet = false;

        private void IncrementCounter() => _programcounter++;
        #endregion

        public void SetProgrammCounter(int newValue)
        {
            _programcounter = newValue;
            _programmCounterExternSet = true;
        }
    }
}
