using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media.TextFormatting;

namespace PicSimu.Model
{
    class Befehle
    {
        //In den beiden Hilfsfunktionen "setDestination()" (für alle Befehle mit Destination-Bit)
        //und in "writeInW_Reg()" (für alle Literal Befehle) wird der zu speichernde Wert auf die
        //8 Bits abgeschnitten
        #region Hilfsfunktionen zu BYTE-ORIENTED FILE REGISTER OPERATIONS

        //Lowbyte -eigentlixh nur die unteren 7 bits- (zum verarbeiten im Befehl) zurückgeben 
        static int extractLowbyteOpcode (int Opcode)
        {
            int lowbyte;

            return lowbyte = Opcode & 0b_00000001111111;

        }
        //Bei true muss ins File-register geschrieben werden
        //Bei false muss ins W-Register geschrieben werden
        static bool isDestinationBitSet(int Opcode)
        {
            int destinationBit = Opcode & 0b_00000010000000;
            if(destinationBit != 0)
            {
                return true;
            }
            else 
            { 
                return false;
            }
        }

        //Speichert nur die 8 relevanten Bits
        static void setDestination(int Opcode, int value, Datenspeicher datenspeicher, W_Register w_Register)
        {
            if (isDestinationBitSet(Opcode))
            {
                int lowbyte = extractLowbyteOpcode(Opcode);
                if (isOverflow(value))
                {
                    int tmp = value & 0b_11111111;
                    datenspeicher.writeValueInRegister(lowbyte, tmp);
                }
                else if (isUnderflow(value))
                {
                    int tmp = -value;
                    datenspeicher.writeValueInRegister(lowbyte, tmp);
                }
                else
                {
                    datenspeicher.writeValueInRegister(lowbyte, value);   
                }

                if (lowbyte == 1)    //TMR0 Register
                {
                    datenspeicher.upgradeTMR0Prescaler();
                }
            }
            else
            {
                if (isOverflow(value))
                {
                    int tmp = value & 0b_11111111;
                    w_Register.writeIn(tmp);
                }
                else if (isUnderflow(value))
                {
                    int tmp = -value;
                    w_Register.writeIn(tmp);
                }
                else
                {
                    w_Register.writeIn(value);
                }
            }
        }

        //Bei true muss das Carry-Flag gesetzt werden
        static bool isOverflow(int value)
        {
            if(value > 255)
            {
                return true;
            }
            else{
                return false;
            }
        }

        //Bei true muss das Carry-Flag gesetzt werden
        static bool isUnderflow(int value)
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

        //Bei true muss das Zero-Flag gesetzt werden
        static bool isZero(int value)
        {
            if (value == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Hilfsfunktion, da am meisten verwendet
        static void affectingZeroFLag(int value, Datenspeicher datenspeicher)
        {
            if (isZero(value))
            {
                datenspeicher.setZeroFlag();
            }
            else
            {
                datenspeicher.resetZeroFlag();
            }
        }

        //Bei true muss das DC-Flag Dgestezt werden
        static bool isHalfcarryOverflow(int newValue, int oldValue)
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

        static bool isHalfcarryUnderflow(int value)
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
        public static void addwf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int val = datenspeicher.getValueFromRegister(lowbyte);

            int sum = val + w_Register.getValue();

            setDestination(Opcode, sum, datenspeicher, w_Register);

            if (isOverflow(sum))
            {
                datenspeicher.setCarryFlag();
            }
            else
            {
                datenspeicher.resetCarryFlag();
            }

            if (isHalfcarryOverflow(sum, val))
            {
                datenspeicher.setDigitCarry();
            }
            else
            {
                datenspeicher.resetdigitCarry();
            }

            affectingZeroFLag(sum, datenspeicher);

        }
    
        public static void subwf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int dif = datenspeicher.getValueFromRegister(lowbyte) - w_Register.getValue();

            //Halfcarry wird vor Carry überprüft, da bei Carry evtl. der Wert verändert wird
            if (isHalfcarryUnderflow(dif))
            {
                datenspeicher.resetdigitCarry();
            }
            else
            {
                datenspeicher.setDigitCarry();
            }

            if (isUnderflow(dif))
            {
                datenspeicher.resetCarryFlag();
                //Bei Underflow muss das Ergebnis nochmal komplett neu berechnet werden
                dif = w_Register.getValue() - datenspeicher.getValueFromRegister(lowbyte);
                dif = 256 - dif;  //Eigentlich 255 - dif + 1
            }
            else
            {
                datenspeicher.setCarryFlag();
            }

            setDestination(Opcode, dif, datenspeicher, w_Register);
            affectingZeroFLag(dif, datenspeicher);
        }
    
        public static void andwf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte) & w_Register.getValue();

            setDestination(Opcode, tmp, datenspeicher, w_Register);

            affectingZeroFLag(tmp, datenspeicher);
        }

        public static void iorwf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte) | w_Register.getValue();

            setDestination(Opcode, tmp, datenspeicher, w_Register);

            affectingZeroFLag(tmp, datenspeicher);
        }

        public static void xorwf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte) ^ w_Register.getValue();

            setDestination(Opcode, tmp, datenspeicher, w_Register);

            affectingZeroFLag(tmp, datenspeicher);
        }
    
        public static void clrw(W_Register w_Register, Datenspeicher datenspeicher)
        {
            w_Register.writeIn(0);
            datenspeicher.setZeroFlag();
        }

        //Axhtung!! Bei Selektion Highbyte und "Destination-Bit" zum identifizieren verwenden 
        public static void clrf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            datenspeicher.writeValueInRegister(lowbyte, 0);
            datenspeicher.setZeroFlag();
        }
    
        public static void comf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte);
            tmp = ~tmp;
            tmp &= 0b_11111111;
            setDestination(Opcode, tmp, datenspeicher, w_Register);

            affectingZeroFLag(tmp, datenspeicher);
        }

        public static void decf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte) - 1;

            tmp &= 0b_11111111;

            setDestination(Opcode, tmp, datenspeicher, w_Register);

        }

        public static void incf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte) + 1;

            setDestination(Opcode, tmp, datenspeicher, w_Register);
        }

        public static void movf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte);

            setDestination(Opcode, tmp, datenspeicher, w_Register);

            affectingZeroFLag(tmp, datenspeicher);
        }

        public static void movwf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = w_Register.getValue();

            datenspeicher.writeValueInRegister(lowbyte, tmp);

            if (lowbyte == 1)
            {    
                    datenspeicher.upgradeTMR0Prescaler(); 
            }
        }

        //Genau einen Arbeitszyklus pausieren => konstante Länge der Arbeitszyklen
        public static void nop()
        {
            //Muss nichts implementiert werden, da das "Ausführen" (aufrufen dieser leeeren Mtehode) quasi einen Arbeitszyklus dauert
        }

        public static void swapf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte);

            int newHigherNibble = (tmp & 0b_00001111) << 4;
            int newLowerNibble = (tmp & 0b_11110000) >> 4;

            int changedNibbles = newHigherNibble + newLowerNibble;

            setDestination(Opcode, changedNibbles, datenspeicher, w_Register);

        }

        public static void rlf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte);
            int carryflag = datenspeicher.getBitFromRegister(3, 0);

            int leftRotated = tmp << 1;


            if (carryflag == 1)
            {
                leftRotated += 1;
            }

            carryflag = (leftRotated & 0b_100000000) >> 8;
            if(carryflag == 1)
            {
                datenspeicher.setCarryFlag();
            }
            else
            {
                datenspeicher.resetCarryFlag();
            }

            int lowerEightBit = leftRotated & 0b_011111111;
            
            setDestination(Opcode, lowerEightBit, datenspeicher, w_Register);
        }

        public static void rrf(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte);
            int carryflag = datenspeicher.getBitFromRegister(3, 0);

            int rightRotated = tmp >> 1;


            if (carryflag == 1)
            {
                rightRotated += 128;
            }
            
            carryflag = tmp & 0b_00000001;
            if (carryflag == 1)
            {
                datenspeicher.setCarryFlag();
            }
            else
            {
                datenspeicher.resetCarryFlag();
            }
            
            setDestination(Opcode, rightRotated, datenspeicher, w_Register);
        }

        public static void decfsz(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte) - 1;

            setDestination(Opcode, tmp, datenspeicher, w_Register);

            if (tmp == 0)
            {
                datenspeicher.increaseProgramCounter();
                datenspeicher.setTwoCyclesRequired();   
            }

        }

        public static void incfsz(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);

            int tmp = datenspeicher.getValueFromRegister(lowbyte) + 1;

            setDestination(Opcode, tmp, datenspeicher, w_Register);

            if (tmp == 256)  //Bedeutet Überlauf, in das Register wird 0 geschrieben, wird in setDestination gehandelt
            {
                datenspeicher.increaseProgramCounter();
                datenspeicher.setTwoCyclesRequired();
            }
        }

        #endregion

        #region Hilfsfunktionen zu BIT_ORIENTED FILE REGISTER OPERATIONS
        static int extractBitNumberFromOpcode(int Opcode)
        {
            int bitNumber = (Opcode & 0b_1110000000) >> 7;
            return bitNumber;
        }

        #endregion

        #region BIT_ORIENTED FILE REGISTER OPERATIONS
        public static void bcf(int Opcode, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            datenspeicher.writeBitInRegister(lowbyte, bitNumber, false);

            if (lowbyte == 1)    //TMR0 Register
                datenspeicher.upgradeTMR0Prescaler();

        }
        public static void bsf(int Opcode, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            datenspeicher.writeBitInRegister(lowbyte, bitNumber, true);
            
            if(lowbyte == 1)    //TMR0 Register
                datenspeicher.upgradeTMR0Prescaler();

        }

        public static void btfsc(int Opcode, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            int bit = datenspeicher.getBitFromRegister(lowbyte, bitNumber);

            if (bit == 0)
            {
                datenspeicher.increaseProgramCounter();
                datenspeicher.setTwoCyclesRequired();
            }
        }

        public static void btfss(int Opcode, Datenspeicher datenspeicher)
        {
            int lowbyte = extractLowbyteOpcode(Opcode);
            int bitNumber = extractBitNumberFromOpcode(Opcode);
            int bit = datenspeicher.getBitFromRegister(lowbyte, bitNumber);

            if (bit == 1)
            {
                datenspeicher.increaseProgramCounter();
                datenspeicher.setTwoCyclesRequired();
            }
        }
        #endregion

        #region Hilfsfunktionen zu LITERAL AND CONTROL OPERATIONS

        //Literal Operations/////////////////////
        static int extractLiteralOpcode(int Opcode)
        {
            int literal;

            return literal = Opcode & 0b_00000011111111;

        }

        //Speichert nur die 8 relevanten Bits
        static void writeInW_Reg(W_Register w_Register, int value)
        {
            if (isOverflow(value))
            {
                int tmp = value & 0b_11111111;
                w_Register.writeIn(tmp);
            }
            if (isUnderflow(value))
            {
                int tmp = - value;
                w_Register.writeIn(tmp);
            }
            else
            {
                w_Register.writeIn(value);
            }
        }


        //Cotrol Operations/////////////////////
        static int getValueForJumpComands(int Opcode)
        {
            return Opcode &= 0b_00011111111111;
        }

        #endregion

        #region LITERAL AND CONTROL OPERATIONS

        //Literal Operations/////////////////////
        public static void addlw(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int literal = extractLiteralOpcode(Opcode);

            int val = w_Register.getValue();

            int sum = val + literal;

            writeInW_Reg(w_Register, sum);

            if (isOverflow(sum))
            {
                datenspeicher.setCarryFlag();
            }
            else
            {
                datenspeicher.resetCarryFlag();
            }

            if (isHalfcarryOverflow(sum, val))
            {
                datenspeicher.setDigitCarry();
            }
            else
            {
                datenspeicher.resetdigitCarry();
            }

            affectingZeroFLag(sum, datenspeicher);

        }

        public static void sublw(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int literal = extractLiteralOpcode(Opcode);

            int dif = literal - w_Register.getValue();

            
           
            if (isHalfcarryUnderflow(dif))
            {
                datenspeicher.resetdigitCarry();
            }
            else
            {
                datenspeicher.setDigitCarry();
            }
           
            if (isUnderflow(dif))
            {
                datenspeicher.resetCarryFlag();
                dif = w_Register.getValue() - literal;  //Bei Underflow muss dif aufgrund der 8 bit Architektur des PIC neu berechnet werden
                dif = 256 - dif;    //Eigentlich dif = 255 - dif + 1
            }
            else
            {
                datenspeicher.setCarryFlag();
            }

            writeInW_Reg(w_Register, dif);
            affectingZeroFLag(dif, datenspeicher);

        }

        public static void andlw(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = w_Register.getValue() & literal;

            writeInW_Reg(w_Register, tmp);

            affectingZeroFLag(tmp, datenspeicher);
        }

        public static void iorlw(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = w_Register.getValue() | literal;

            writeInW_Reg(w_Register, tmp);

            affectingZeroFLag(tmp, datenspeicher);
        }

        public static void xorlw(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int literal = extractLiteralOpcode(Opcode);

            int tmp = w_Register.getValue() ^ literal;

            writeInW_Reg(w_Register, tmp);

            affectingZeroFLag(tmp, datenspeicher);
        }

        public static void movlw(int Opcode, W_Register w_Register, Datenspeicher datenspeicher)
        {
            int literal = extractLiteralOpcode(Opcode);

            writeInW_Reg(w_Register, literal);
        }


        //Cotrol Operations/////////////////////
        public static void call(int Opcode, Datenspeicher datenspeicher, Stack stack)
        {
            int newAdress = getValueForJumpComands(Opcode);
            stack.addNewElement(datenspeicher.getProgramCounter());
            datenspeicher.writeInProgramCounter(newAdress);

            datenspeicher.setTwoCyclesRequired();
        }

        public static void goTo(int Opcode, Datenspeicher datenspeicher, Stack stack)
        {
            int newAdress = getValueForJumpComands(Opcode);
            datenspeicher.writeInProgramCounter(newAdress);

            datenspeicher.setTwoCyclesRequired();
        }

        public static void returnlw(int Opcode, Datenspeicher datenspeicher, Stack stack, W_Register w_register)
        {
            int lowbyte = extractLiteralOpcode(Opcode);
            w_register.writeIn(lowbyte);
            datenspeicher.overrideProgramCounter(stack.getElement());

            datenspeicher.setTwoCyclesRequired(); 
        }

        public static void Return(Datenspeicher datenspeicher, Stack stack)
        {
            datenspeicher.overrideProgramCounter(stack.getElement());

            datenspeicher.setTwoCyclesRequired();
        }

        public static void clrwdt(Datenspeicher datenspeicher)
        {
            //0 -> WatchdogVorteiler und WDT = 0
            datenspeicher.clearwatchdog();
            datenspeicher.setTOFlag();
            datenspeicher.setPDFlag();
        }

        public static void sleep(Datenspeicher datenspeicher)
        {
            /*dieser Stromsparende Halt-Zustand kann nur durch ein "RESET" beendet werden. 
             Hier wird nur die Möglichkeit durch ansprechen des Watchdogs implementiert, da nur dieser Fall getestet wird!
             Die Varianten, durch ein "L" am MCLR-Eingang oder durch das Auftreten eines Interrupts den Sleep-Modus zu beenden, 
             werden hier explizit nicht implementiert.*/
            datenspeicher.enterSleepMode();

            //0 -> WatchdogVorteiler und WDT = 0
            //datenspeicher.clearwatchdog();
            //datenspeicher.setTOFlag();
            //datenspeicher.resetPDFlag();
        }

        public static void retfie(Datenspeicher datenspeicher, Stack stack)
        {
            datenspeicher.writeBitInRegister(11, 7, true);
            datenspeicher.overrideProgramCounter(stack.getElement());

            datenspeicher.setTwoCyclesRequired();
        }


        #endregion
    }
}
