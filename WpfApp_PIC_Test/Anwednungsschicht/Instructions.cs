using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Anwednungsschicht.DatenspeicherService;
using WpfApp_PIC.Domänenschicht;

//TMR0 Prescaler wurde entfernt

namespace WpfApp_PIC.Anwednungsschicht
{
    public class Instructions
    {
        readonly DataRegister datenspeicher;
        readonly W_Register w_register;
        readonly Stack stack;
        readonly ProgramCounter programmzähler;
        readonly StatusRegisterService statusRegisterService;
        readonly TMR0RegisterService TMR0RegisterService;

        public Instructions(DataRegister datenspeicher, W_Register w_register, Stack stack, ProgramCounter programmzähler, StatusRegisterService statusRegisterService, TMR0RegisterService tMR0RegisterService)
        {
            this.datenspeicher = datenspeicher;
            this.w_register = w_register;
            this.stack = stack;
            this.programmzähler = programmzähler;
            this.statusRegisterService = statusRegisterService;
            TMR0RegisterService = tMR0RegisterService;
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
                    datenspeicher.SetValue(lowbyte, tmp);
                }
                else if (isUnderflow(value))
                {
                    int tmp = -value;
                    datenspeicher.SetValue(lowbyte, tmp);
                }
                else
                {
                    datenspeicher.SetValue(lowbyte, value);
                }
            }
            else
            {
                if (isOverflow(value))
                {
                    int tmp = value & 0b_11111111;
                    w_register.SetValue(tmp);
                }
                else if (isUnderflow(value))
                {
                    int tmp = -value;
                    w_register.SetValue(tmp);
                }
                else
                {
                    w_register.SetValue(value);
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
                statusRegisterService.SetZeroFlag();
            }
            else
            {
                statusRegisterService.ResetZeroFlag();
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

            int val = datenspeicher.GetValue(lowbyte);

            int sum = val + w_register.GetValue();

            setDestination(Opcode, sum);

            if (isOverflow(sum))
            {
                statusRegisterService.SetCarryFlag();
            }
            else
            {
                statusRegisterService.ResetCarryFlag();
            }

            if (isHalfcarryOverflow(sum, val))
            {
                statusRegisterService.SetDCFlag();
            }
            else
            {
                statusRegisterService.ResetDCFlag();
            }

            affectingZeroFLag(sum);

        }

        public void subwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int dif = datenspeicher.GetValue(lowbyte) - w_register.GetValue();

            //Halfcarry wird vor Carry überprüft, da bei Carry evtl. der Wert verändert wird
            if (isHalfcarryUnderflow(dif))
            {
                statusRegisterService.ResetDCFlag();
            }
            else
            {
                statusRegisterService.SetDCFlag();
            }

            if (isUnderflow(dif))
            {
                statusRegisterService.ResetCarryFlag();
                //Bei Underflow muss das Ergebnis nochmal komplett neu berechnet werden
                dif = w_register.GetValue() - datenspeicher.GetValue(lowbyte);
                dif = 256 - dif;  //Eigentlich 255 - dif + 1
            }
            else
            {
                statusRegisterService.SetCarryFlag();
            }

            setDestination(Opcode, dif);
            affectingZeroFLag(dif);
        }

        public void andwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte) & w_register.GetValue();

            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void iorwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte) | w_register.GetValue();

            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void xorwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte) ^ w_register.GetValue();

            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void clrw()
        {
            w_register.SetValue(0);
            statusRegisterService.SetZeroFlag();
        }

        //Axhtung!! Bei Selektion Highbyte und "Destination-Bit" zum identifizieren verwenden 
        public void clrf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            datenspeicher.SetValue(lowbyte, 0);
            statusRegisterService.SetZeroFlag();
        }

        public void comf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte);
            tmp = ~tmp;
            tmp &= 0b_11111111;
            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void decf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte) - 1;

            tmp &= 0b_11111111;

            setDestination(Opcode, tmp);

        }

        public void incf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte) + 1;

            setDestination(Opcode, tmp);
        }

        public void movf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte);

            setDestination(Opcode, tmp);

            affectingZeroFLag(tmp);
        }

        public void movwf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = w_register.GetValue();

            datenspeicher.SetValue(lowbyte, tmp);
        }

        //Genau einen Arbeitszyklus pausieren => konstante Länge der Arbeitszyklen
        public void nop()
        {
            //Muss nichts implementiert werden, da das "Ausführen" (aufrufen dieser leeeren Mtehode) quasi einen Arbeitszyklus dauert
        }

        public void swapf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte);

            int newHigherNibble = (tmp & 0b_00001111) << 4;
            int newLowerNibble = (tmp & 0b_11110000) >> 4;

            int changedNibbles = newHigherNibble + newLowerNibble;

            setDestination(Opcode, changedNibbles);

        }

        public void rlf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte);
            int carryflag = datenspeicher.GetBit(3, 0);

            int leftRotated = tmp << 1;


            if (carryflag == 1)
            {
                leftRotated += 1;
            }

            carryflag = (leftRotated & 0b_100000000) >> 8;
            if (carryflag == 1)
            {
                statusRegisterService.SetCarryFlag();
            }
            else
            {
                statusRegisterService.ResetCarryFlag();
            }

            int lowerEightBit = leftRotated & 0b_011111111;

            setDestination(Opcode, lowerEightBit);
        }

        public void rrf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte);
            int carryflag = datenspeicher.GetBit(3, 0);

            int rightRotated = tmp >> 1;


            if (carryflag == 1)
            {
                rightRotated += 128;
            }

            carryflag = tmp & 0b_00000001;
            if (carryflag == 1)
            {
                statusRegisterService.SetCarryFlag();
            }
            else
            {
                statusRegisterService.ResetCarryFlag();
            }

            setDestination(Opcode, rightRotated);
        }

        public void decfsz(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte) - 1;

            setDestination(Opcode, tmp);

            if (tmp == 0)
            {
                programmzähler.IncreaseProgramCounter();

            }

        }

        public void incfsz(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.GetValue(lowbyte) + 1;

            setDestination(Opcode, tmp);

            if (tmp == 256)  //Bedeutet Überlauf, in das Register wird 0 geschrieben, wird in setDestination gehandelt
            {
                programmzähler.IncreaseProgramCounter();

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
            datenspeicher.SetBit(lowbyte, bitNumber, false);
        }
        public void bsf(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            datenspeicher.SetBit(lowbyte, bitNumber, true);
        }

        public void btfsc(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            int bit = datenspeicher.GetBit(lowbyte, bitNumber);

            if (bit == 0)
            {
                programmzähler.IncreaseProgramCounter();

            }
        }

        public void btfss(int Opcode)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            int bit = datenspeicher.GetBit(lowbyte, bitNumber);

            if (bit == 1)
            {
                programmzähler.IncreaseProgramCounter();

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

            int val = w_register.GetValue();

            int sum = val + literal;

            w_register.SetValue(sum);

            if (isOverflow(sum))
            {
                statusRegisterService.SetCarryFlag();
            }
            else
            {
                statusRegisterService.ResetCarryFlag();
            }

            if (isHalfcarryOverflow(sum, val))
            {
                statusRegisterService.SetDCFlag();
            }
            else
            {
                statusRegisterService.ResetDCFlag();
            }

            affectingZeroFLag(sum);

        }

        public void sublw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int dif = literal - w_register.GetValue();



            if (isHalfcarryUnderflow(dif))
            {
                statusRegisterService.ResetDCFlag();
            }
            else
            {
                statusRegisterService.SetDCFlag();
            }

            if (isUnderflow(dif))
            {
                statusRegisterService.ResetCarryFlag();
                dif = w_register.GetValue() - literal;  //Bei Underflow muss dif aufgrund der 8 bit Architektur des PIC neu berechnet werden
                dif = 256 - dif;    //Eigentlich dif = 255 - dif + 1
            }
            else
            {
                statusRegisterService.SetCarryFlag();
            }

            w_register.SetValue(dif);
            affectingZeroFLag(dif);

        }

        public void andlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = w_register.GetValue() & literal;

            w_register.SetValue(tmp);

            affectingZeroFLag(tmp);
        }

        public void iorlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = w_register.GetValue() | literal;

            w_register.SetValue(tmp);

            affectingZeroFLag(tmp);
        }

        public void xorlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = w_register.GetValue() ^ literal;

            w_register.SetValue(tmp);

            affectingZeroFLag(tmp);
        }

        public void movlw(int Opcode)
        {
            int literal = extractLiteralOpcode(Opcode);

            w_register.SetValue(literal);
        }


        //Cotrol Operations/////////////////////
        public void call(int Opcode)
        {
            int newAdress = getValueForJumpComands(Opcode);
            stack.Push(programmzähler.GetProgramCounter());
            programmzähler.SetProgrammCounter(newAdress);


        }

        public void goTo(int Opcode)
        {
            int newAdress = getValueForJumpComands(Opcode);
            programmzähler.SetProgrammCounter(newAdress);


        }

        public void returnlw(int Opcode)
        {
            int lowbyte = extractLiteralOpcode(Opcode);
            w_register.SetValue(lowbyte);
            programmzähler.SetProgrammCounter(stack.Pop());


        }

        public void Return()
        {
            programmzähler.SetProgrammCounter(stack.Pop());


        }

        public void clrwdt()
        {
            //0 -> WatchdogVorteiler und WDT = 0

            statusRegisterService.SetTOFlag();
            statusRegisterService.SetPDFlag();
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
            datenspeicher.SetBit(11, 7, true);
            programmzähler.SetProgrammCounter(stack.Pop());


        }


        #endregion
    }
}

