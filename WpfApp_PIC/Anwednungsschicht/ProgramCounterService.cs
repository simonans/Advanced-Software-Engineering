﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_PIC.Anwednungsschicht.DataRegisterServices;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht
{
    public class ProgramCounterService /*: IProgrammCounterUpdate*/
    {
        private ProgramCounter _programcounter;
        private RegularSFRService _pclRegisterService;
        private RegularSFRService _pclathRegisterService;
        public event EventHandler ValueChanged;

        public ProgramCounterService(ProgramCounter programcounter, RegularSFRService pclRegisterService, RegularSFRService pclathRegisterService)
        {
            _programcounter = programcounter;
            _pclRegisterService = pclRegisterService;
            _pclathRegisterService = pclathRegisterService;
        }

        public int GetPC()
        {
            return _programcounter.GetProgramCounter();
        }

        public void SetPC(int newValue)
        {
            _programcounter.SetProgrammCounter(newValue);
            _pclRegisterService.SetValue(newValue & 0xFF);
            _pclathRegisterService.SetValue((newValue >> 8) & 0xFF);
            OnValueChanged();
        }

        public void IncreasePC()
        {
            _programcounter.IncreaseProgramCounter();
            _pclRegisterService.SetValue(_programcounter.GetProgramCounter() & 0xFF);
            _pclathRegisterService.SetValue((_programcounter.GetProgramCounter() >> 8) & 0xFF);
            OnValueChanged();
        }
        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}

