using System;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using WpfApp_PIC.Anwednungsschicht.DataRegisterServices;
using WpfApp_PIC.Domänenschicht;

//TMR0 Prescaler wurde entfernt

namespace WpfApp_PIC.Anwednungsschicht
{
    public class Instructions
    {
        readonly DataRegisterService _dataRegisterService;
        readonly W_RegisterService _w_registerService;
        readonly StackService _stackService;
        readonly ProgramCounterService _programCounterService;
        readonly RegularSFR _statusRegisterService;
        readonly RegularSFR _TMR0RegisterService;

        public Instructions(DataRegisterService dataRegisterService, W_RegisterService w_RegisterService, StackService stackService, ProgramCounterService programCounterService, RegularSFR statusRegisterService, RegularSFR tMR0RegisterService)
        {
            _dataRegisterService = dataRegisterService;
            _w_registerService = w_RegisterService;
            _stackService = stackService;
            _programCounterService = programCounterService;
            _statusRegisterService = statusRegisterService;
            _TMR0RegisterService = tMR0RegisterService;
        }

        //In den beiden Hilfsfunktionen "setDestination()" (für alle Befehle mit Destination-Bit)
        //und in "writeInW_Reg()" (für alle Literal Befehle) wird der zu speichernde Wert auf die
        //8 Bits abgeschnitten
        #region Hilfsfunktionen zu BYTE-ORIENTED FILE REGISTER OPERATIONS

        //Lowbyte -eigentlixh nur die unteren 7 bits- (zum verarbeiten im Befehl) zurückgeben 
        private int extractLowbyteOpcode(int Opcode)
        {
            int lowbyte;

            return lowbyte = Opcode & 0b_00000001111111;
        }

        //Bei true muss ins File-register geschrieben werden
        //Bei false muss ins W-Register geschrieben werden
        private bool isDestinationBitSet(int Opcode)
        {
            int destinationBit = Opcode & 0b_00000010000000;
            if (destinationBit != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Speichert nur die 8 relevanten Bits
        private void setDestination(int Opcode, int value)
        {
            if (isDestinationBitSet(Opcode))
            {
                int lowbyte = extractLowbyteOpcode(Opcode);
                if (isOverflow(value))
                {
                    int tmp = value & 0b_11111111;
                    _dataRegisterService.SetValue(lowbyte, tmp);
                }
                else if (isUnderflow(value))
                {
                    int tmp = -value;
                    _dataRegisterService.SetValue(lowbyte, tmp);
                }
                else
                {
                    _dataRegisterService.SetValue(lowbyte, value);
                }
            }
            else
            {
                if (isOverflow(value))
                {
                    int tmp = value & 0b_11111111;
                    _w_registerService.SetValue(tmp);
                }
                else if (isUnderflow(value))
                {
                    int tmp = -value;
                    _w_registerService.SetValue(tmp);
                }
                else
                {
                    _w_registerService.SetValue(value);
                }
            }
        }

        //Bei true muss das Carry-Flag gesetzt werden
        private bool isOverflow(int value)
        {
            if (value > 255)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Bei true muss das Carry-Flag gesetzt werden
        private bool isUnderflow(int value)
        {
            if (value < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Hilfsfunktion, da am meisten verwendet
        private void affectingZeroFLag(int value)
        {
            if (value == 0)
            {
                _statusRegisterService.SetBit(2);
            }
            else
            {
                _statusRegisterService.ResetBit(2);
            }
        }

        //Bei true muss das DC-Flag Dgestezt werden
        private bool isHalfcarryOverflow(int newValue, int oldValue)
        {
            if (oldValue > 15 || newValue <= 15)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool isHalfcarryUnderflow(int value)
        {
            if (value < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region BYTE-ORIENTED FILE REGISTER OPERATIONS
        public void addwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int val = _dataRegisterService.GetValue(lowbyte);

            int sum = val + _w_registerService.GetValue();

            setDestination(Opcode, sum);

            if (isOverflow(sum))
            {
                _statusRegisterService.SetBit(0);
            }
            else
            {
                _statusRegisterService.ResetBit(0);
            }

            if (isHalfcarryOverflow(sum, val))
            {
                _statusRegisterService.SetBit(1);
            }
            else
            {
                _statusRegisterService.ResetBit(1);
            }

            affectingZeroFLag(sum);
        }

        public void subwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int dif = _dataRegisterService.GetValue(lowbyte) - _w_registerService.GetValue();

            //Halfcarry wird vor Carry überprüft, da bei Carry evtl. der Wert verändert wird
            if (isHalfcarryUnderflow(dif))
            {
                _statusRegisterService.ResetBit(1);
            }
            else
            {
                _statusRegisterService.SetBit(1);
            }

            if (isUnderflow(dif))
            {
                _statusRegisterService.ResetBit(0);
                //Bei Underflow muss das Ergebnis nochmal komplett neu berechnet werden
                dif = _w_registerService.GetValue() - _dataRegisterService.GetValue(lowbyte);
                dif = 256 - dif;  //Eigentlich 255 - dif + 1
            }
            else
            {
                _statusRegisterService.SetBit(0);
            }

            setDestination(Opcode, dif);
            affectingZeroFLag(dif);
        }

        public void andwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte) & _w_registerService.GetValue();

            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void iorwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte) | _w_registerService.GetValue();

            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void xorwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte) ^ _w_registerService.GetValue();

            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void clrw()
        {
            _w_registerService.SetValue(0);
            _statusRegisterService.SetBit(2);
        }

        //Axhtung!! Bei Selektion Highbyte und "Destination-Bit" zum identifizieren verwenden 
        public void clrf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            _dataRegisterService.SetValue(lowbyte, 0);
            _statusRegisterService.SetBit(2);
        }

        public void comf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte);
            tmp = ~tmp;
            tmp &= 0b_11111111;
            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void decf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte) - 1;

            tmp &= 0b_11111111;

            setDestination(Opcode, tmp);
        }

        public void incf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte) + 1;

            setDestination(Opcode, tmp);
        }

        public void movf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte);

            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void movwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _w_registerService.GetValue();

            _dataRegisterService.SetValue(lowbyte, tmp);
        }

        //Genau einen Arbeitszyklus pausieren => konstante Länge der Arbeitszyklen
        public void nop()
        {
            //Muss nichts implementiert werden, da das "Ausführen" (aufrufen dieser leeeren Mtehode) quasi einen Arbeitszyklus dauert
        }

        public void swapf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte);

            int newHigherNibble = (tmp & 0b_00001111) << 4;
            int newLowerNibble = (tmp & 0b_11110000) >> 4;

            int changedNibbles = newHigherNibble + newLowerNibble;

            setDestination(Opcode, changedNibbles);
        }

        public void rlf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte);
            int carryflag = _dataRegisterService.GetBit(3, 0);

            int leftRotated = tmp << 1;

            if (carryflag == 1)
            {
                leftRotated += 1;
            }

            carryflag = (leftRotated & 0b_100000000) >> 8;
            if (carryflag == 1)
            {
                _statusRegisterService.SetBit(0);
            }
            else
            {
                _statusRegisterService.ResetBit(0);
            }

            int lowerEightBit = leftRotated & 0b_011111111;

            setDestination(Opcode, lowerEightBit);
        }

        public void rrf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte);
            int carryflag = _dataRegisterService.GetBit(3, 0);

            int rightRotated = tmp >> 1;

            if (carryflag == 1)
            {
                rightRotated += 128;
            }

            carryflag = tmp & 0b_00000001;
            if (carryflag == 1)
            {
                _statusRegisterService.SetBit(0);
            }
            else
            {
                _statusRegisterService.ResetBit(0);
            }

            setDestination(Opcode, rightRotated);
        }

        public void decfsz(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte) - 1;

            setDestination(Opcode, tmp);

            if (tmp == 0)
            {
                _programCounterService.IncreasePC();
            }
        }

        public void incfsz(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = _dataRegisterService.GetValue(lowbyte) + 1;

            setDestination(Opcode, tmp);

            if (tmp == 256)  //Bedeutet Überlauf, in das Register wird 0 geschrieben, wird in setDestination gehandelt
            {
                _programCounterService.IncreasePC();
            }
        }

        #endregion

        #region Hilfsfunktionen zu BIT_ORIENTED FILE REGISTER OPERATIONS
        private int extractBitNumberFromOpcode(int Opcode)
        {
            int bitNumber = (Opcode & 0b_1110000000) >> 7;
            return bitNumber;
        }

        #endregion

        #region BIT_ORIENTED FILE REGISTER OPERATIONS
        public void bcf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            _dataRegisterService.SetBit(lowbyte, bitNumber, false);
        }

        public void bsf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            _dataRegisterService.SetBit(lowbyte, bitNumber, true);
        }

        public void btfsc(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            int bit = _dataRegisterService.GetBit(lowbyte, bitNumber);

            if (bit == 0)
            {
                _programCounterService.IncreasePC();
            }
        }

        public void btfss(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            int bit = _dataRegisterService.GetBit(lowbyte, bitNumber);

            if (bit == 1)
            {
                _programCounterService.IncreasePC();
            }
        }
        #endregion

        #region Hilfsfunktionen zu LITERAL AND CONTROL OPERATIONS

        //Literal Operations/////////////////////
        private int extractLiteralOpcode(int Opcode)
        {
            int literal;

            return literal = Opcode & 0b_00000011111111;
        }

        //Speichert nur die 8 relevanten Bits

        //Cotrol Operations/////////////////////
        private int getValueForJumpComands(int Opcode)
        {
            return Opcode &= 0b_00011111111111;
        }

        #endregion

        #region LITERAL AND CONTROL OPERATIONS

        //Literal Operations/////////////////////
        public void addlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int val = _w_registerService.GetValue();

            int sum = val + literal;

            _w_registerService.SetValue(sum);

            if (isOverflow(sum))
            {
                _statusRegisterService.SetBit(0);
            }
            else
            {
                _statusRegisterService.ResetBit(0);
            }

            if (isHalfcarryOverflow(sum, val))
            {
                _statusRegisterService.SetBit(1);
            }
            else
            {
                _statusRegisterService.ResetBit(1);
            }

            affectingZeroFLag(sum);
        }

        public void sublw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int dif = literal - _w_registerService.GetValue();

            if (isHalfcarryUnderflow(dif))
            {
                _statusRegisterService.ResetBit(1);
            }
            else
            {
                _statusRegisterService.SetBit(1);
            }

            if (isUnderflow(dif))
            {
                _statusRegisterService.ResetBit(0);
                dif = _w_registerService.GetValue() - literal;  //Bei Underflow muss dif aufgrund der 8 bit Architektur des PIC neu berechnet werden
                dif = 256 - dif;    //Eigentlich dif = 255 - dif + 1
            }
            else
            {
                _statusRegisterService.SetBit(0);
            }

            _w_registerService.SetValue(dif);
            affectingZeroFLag(dif);
        }

        public void andlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = _w_registerService.GetValue() & literal;

            _w_registerService.SetValue(tmp);

            affectingZeroFLag(tmp);
        }

        public void iorlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = _w_registerService.GetValue() | literal;

            _w_registerService.SetValue(tmp);

            affectingZeroFLag(tmp);
        }

        public void xorlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = _w_registerService.GetValue() ^ literal;

            _w_registerService.SetValue(tmp);

            affectingZeroFLag(tmp);
        }

        public void movlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            _w_registerService.SetValue(literal);
        }

        //Cotrol Operations/////////////////////
        public void call(int Opcode)
        {
            int newAdress = getValueForJumpComands(Opcode);
            _stackService.Push(_programCounterService.GetPC());
            _programCounterService.SetPC(newAdress);
        }

        public void goTo(int Opcode)
        {
            int newAdress = getValueForJumpComands(Opcode);
            _programCounterService.SetPC(newAdress);
        }

        public void returnlw(int Opcode)
        {
            int lowbyte = extractLiteralOpcode(Opcode);
            _w_registerService.SetValue(lowbyte);
            _programCounterService.SetPC(_stackService.Pop());
        }

        public void Return()
        {
            _programCounterService.SetPC(_stackService.Pop());
        }

        public void clrwdt()
        {
            //0 -> WatchdogVorteiler und WDT = 0

            _statusRegisterService.SetBit(4);
            _statusRegisterService.SetBit(5);
        }

        public void sleep()
        {
            /*dieser Stromsparende Halt-Zustand kann nur durch ein "RESET" beendet werden. 
             Hier wird nur die Möglichkeit durch ansprechen des Watchdogs implementiert, da nur dieser Fall getestet wird!
             Die Varianten, durch ein "L" am MCLR-Eingang oder durch das Auftreten eines Interrupts den Sleep-Modus zu beenden, 
             werden hier explizit nicht implementiert.*/

            //0 -> WatchdogVorteiler und WDT = 0
            //
            //datenspeicher.setTOFlag();
            //datenspeicher.resetPDFlag();
        }

        public void retfie()
        {
            _dataRegisterService.SetBit(11, 7, true);
            _programCounterService.SetPC(_stackService.Pop());
        }

        #endregion
    }
}
