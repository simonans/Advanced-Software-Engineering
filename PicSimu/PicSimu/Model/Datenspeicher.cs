using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PicSimu.Model
{
    class Datenspeicher
    {
        public int[] storage;
        public int[] bank1;
        
        private int programCounter;

        public double frequency;
        private double timePerCycle;
        public double duration;

        private int prescaler;
        private bool watchdogIsEnbled;
        private bool watchdog;
        int wathcdogCtr;

        private bool inSleepMode;

        public Latch latch;

        private int eepromStateCounter;
        private bool eepromRead;
        private bool eepromWrite;
        private double eepromTimerCounter;

        private bool requiresTwoCycles;

        public Datenspeicher()
        {
            storage = new int[80];
            bank1 = new int[10];

            //Startwerte:
            storage[2] = 0;   //PCL
            storage[3] = 24;  //Status
            storage[10] = 0;  //PCLATH
            storage[11] = 0;  //Intcon
            bank1[1] = 255;   //Option
            bank1[5] = 31;    //TrisA
            bank1[6] = 255;   //TrisB
            bank1[8] = 0;     //EECON1

            programCounter = 0;
            
            frequency = 4;  //Frequenz über Oberfläche bekommen
            timePerCycle = ((1/frequency) * 4); //µs
            duration = 0;

            prescaler = 0;
            watchdogIsEnbled = false;
            watchdog = false;
            wathcdogCtr = 1;

            latch = new Latch();

            inSleepMode = false;

            eepromStateCounter = 0;
            eepromRead = false;
            eepromWrite = false;
            eepromTimerCounter = 0;

            requiresTwoCycles = false;
        }

        #region function for binding OPTION-Reg to View
        public int getBitFromOptionReg(int mask) 
        {
            int bit = bank1[1] & mask;
            if(bit == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        #endregion
        //beide Funktionen beachten nicht, ob ein Wert unter bestimmten Konditionen nicht gelesen werden sollte da sinnlos!!!
        #region function for binding INTCON-Reg to View

        public int getBitFromIntconReg(int mask)
        {
            int bit = storage[11] & mask;
            if (bit == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        #endregion

        #region function for binding PORT
        //public bool getBitFromPortRAReg(int mask)
        //{
        //    int bit = storage[5] & mask;
        //    if (bit == 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //public bool getBitFromTrisAReg(int mask)
        //{
        //    int bit = bank1[5] & mask;
        //    if (bit == 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //public bool getBitFromPortRBReg(int mask)
        //{
        //    int bit = storage[6] & mask;
        //    if (bit == 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //public bool getBitFromTrisBReg(int mask)
        //{
        //    int bit = bank1[6] & mask;
        //    if (bit == 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        #endregion

        #region Laufzeit/Frequenz
        

        public double getDuration() { return duration; }
        #endregion



        #region Functions for Commands 
        public void writeValueInRegister(int addr, int value)
        {
            if (registerOnBank1(addr))
            {
                bank1[addr] = value;
            }
            else
            {
                storage[addr] = value;
            }

            if (isSpecialRegisterCalled(addr))
                specialRegisterHandler(addr, value);

            return;
        }

        public void writeBitInRegister(int addr, int bitNumber, bool set)
        {
            int reg = getValueFromRegister(addr);

            int tmp = 1;
            tmp = tmp << bitNumber;  //tmp hat an der richtigen Bitstelle eine 1, sonst nur Nullen
            if (set == true)
            {
                reg |= tmp;
            }

            else
            {
                tmp = ~tmp;
                reg &= tmp;
            }

            writeValueInRegister(addr, reg);

            return;
        }

        public int getValueFromRegister(int addr)
        {
            if (addr == 0)  //Indirect Register
            {
                int fsr = storage[4];
                return storage[fsr];
            }

            if (registerOnBank1(addr))
            {
                return bank1[addr];
            }
            
            return storage[addr];
        }

        public int getBitFromRegister(int addr, int bitNumber)
        {
            int val = getValueFromRegister(addr);
            val = val >> bitNumber;  //Das zu untersuchende Bit steht jetzt ganz rechts in val
            val = val & 1;
            return val;
        }
        #endregion



        #region Statusregister Functions
        public void setZeroFlag() { writeBitInRegister(3, 2, true); }
        public void resetZeroFlag() { writeBitInRegister(3, 2, false); }
        public void setCarryFlag() { writeBitInRegister(3, 0, true); }
        public void resetCarryFlag() { writeBitInRegister(3, 0, false); }
        public void setDigitCarry() { writeBitInRegister(3, 1, true); }
        public void resetdigitCarry() { writeBitInRegister(3, 1, false); }
        //Da das Time-Out-Status-Bit und das Power-Down-Status-Bit inverse Bits sind,
        //gleicht das setzen dem speichern einer 0
        //und das rücksetzen dem speichern einer 1
        //im jeweiligen Status-Bit
        public void setPDFlag() { writeBitInRegister(3, 3, false); }
        public void resetPDFlag() { writeBitInRegister(3, 3, true); }
        public void setTOFlag() { writeBitInRegister(3, 4, false); }
        public void resetTOFlag() { writeBitInRegister(3, 4, true); }
        #endregion



        #region Programmcounter
        //Programmcounter functions
        public void increaseProgramCounter()  //Wird in jedem Fetch aufgerufen
        {
            storage[2]++;  //PCL erhöhen
            if (storage[2] > 255)
            {
                storage[2] = 0;
            }
            calculateProgramCounter();
        }

        public void writeInProgramCounter(int value)  //Für CALL und GOTO Befehl
        {
            int newPCL = value;  //PCL Register zusätzlich aktualisieren
            newPCL &= 255;
            storage[2] = newPCL;

            int pageIndex = storage[10];
            pageIndex &= 0b_11000; //Nur Bit 3 und 4 von PCLATH relevant
            programCounter = pageIndex + value;
        }

        public void overrideProgramCounter(int value)  //Für RETURN Befehle
        {
            programCounter = value;
            int newPCL = value;  //PCL Register zusätzlich aktualisieren
            newPCL &= 255;
            storage[2] = newPCL;
        }

        public int getProgramCounter() { return programCounter; }

        private void calculateProgramCounter()
        {
            programCounter &= 0b_1111100000000;
            //int pclath = (storage[10] << 8);
            //programCounter =pclath + storage[2];
            programCounter += storage[2];
        }

        #endregion



        #region CycleHandler
        //TMR0-Register muss für manche Befehle um zwei erhöht werden, da der PIC für die Ausführung zwei Zyklen benötigt
        public void setTwoCyclesRequired()
        {
            requiresTwoCycles = true;
        }

        public void updateClockAfterCycle(Stack stack, EEPROM eeprom)
        {
            //EEPROM Handling
            if(eepromRead)
            {
                storage[8] = eeprom.read(storage[9]);
                eepromRead = false;
            }
            if (eepromWrite)
                eepromWriteHandler(eeprom);

            PortOutputHandler();
            
            TMR0Handler();
            watchdogHandler();
            interruptTester(stack);
        }


        public void upgradeTMR0Prescaler()
        {
            if(PSA() == false)  //Vorteiler hängt am TMR0 Register
                prescaler = 0;
            return;
        }

        private void durationHandler()
        {
            int numberOfCycles = 1;
            timePerCycle = ((1 / frequency) * 4);
            if (requiresTwoCycles) 
            {
                numberOfCycles = 2;
                requiresTwoCycles = false;
            }

            while (numberOfCycles > 0) 
            {
                duration += timePerCycle; 
                numberOfCycles--;
            }
        }


        private void TMR0Handler()
        {
            //if (optionRegisterChanged)   //nach Änderung des Option registers muss eine Runde Pause gemacht werden
            //{
            //    optionRegisterChanged = false;
            //    return;
            //}

            int option_reg = bank1[1];
            int T0CS = option_reg & 0b_00100000;

            if (T0CS > 0)
                //TMR0 ist abhängig von RA4
                //Das wird nicht hier gehandelt, sondern direkt am Input von RA4 (region PortA Input)
                return;

            
            //TMR0 ist abhängig vom internen Quarz
            
                int numberOfCycles = 1;  //Manchmal muss dauert ein Befehl 2 Zyklen, das wird mit dieser while Schleife abgefangen
            if (requiresTwoCycles)
            {
                numberOfCycles = 2;
            }

            while (numberOfCycles > 0)
            {
                TMR0increase();
                numberOfCycles--;
            }
            
        }

        

        private void TMR0increase()
        {
            if (PSA() == false) //Vorteiler hängt am TMR0 Register
            {
                prescaler++;
                if (prescalerTMR0Oweflow() == true)
                {
                    prescaler = 0;
                    storage[1]++;   //TMR0 Rgister erhöhen

                }
            }

            else
            {
                //TMR0 Register ohne Vorteiler erhöhen
                storage[1]++;   //TMR0 Rgister erhöhen
            }

            //TMR0 auf Überlauf prüfen und Bit für Interrupt setzen
            if (storage[1] >= 256)
            {
                storage[1] = 0;
                writeBitInRegister(11, 2, true);
            }
        }


        public void watchdogHandler()
        {
            durationHandler();
            if (!watchdogIsEnbled)
                return;


            if (duration < 18000)
                return;

            if (PSA() == true) //Vorteiler am Watchdog
            {
                double watchdogPrescaler = duration / 18000;
                if (prescalerWatchdogOweflow(watchdogPrescaler) == true)
                {
                    watchdog = true;
                }
            }

            else
                watchdog = true;

            if (watchdog)
            {
                setTOFlag();
               
                if(inSleepMode)
                {
                    inSleepMode = false;
                    wathcdogCtr++;
                }

            }
            return;
        }

        private bool PSA()  
            //PSA = 1 -> Vorteiler zu Watchdog
            //PSA = 0 -> Vprteiler zu TMR0
        {
            int psa = bank1[1];
            psa &= 0b_1000;
            if(psa > 0)
                return true;
            return false;   
        }

        private bool prescalerTMR0Oweflow()
            //Handelt die Vorteiler Rate
        {
            bool overflow = false;
            int prescalerRate = bank1[1];
            prescalerRate &= 0b_111;
            
            double tmr0Rate = Math.Pow(2, prescalerRate) * 2;
            
            if (prescaler >= tmr0Rate) 
                overflow = true; 

            return overflow;
        }


        private bool prescalerWatchdogOweflow(double watchdogPresc)
        //Handelt die Vorteiler Rate 
        {
            bool overflow = false;
            int prescalerRate = bank1[1];
            prescalerRate &= 0b_111;

            double watchdogRate = (Math.Pow(2, prescalerRate)) * wathcdogCtr;
            if (watchdogPresc >= watchdogRate)
                overflow = true;

            return overflow;
        }
        #endregion



        #region Watchdog and Sleep
        public void enableWatchdog() { watchdogIsEnbled = true; }
        public void disableWatchdog() {  watchdogIsEnbled = false; }

        public bool getWatchdogStatus() { return watchdog; }

        public void resetWatchdog() { watchdog = false; }

        public void datenspeicherReset()
        {
            programCounter = 0;
            
            storage[2] = 0;     //PCL
            storage[3] &= 0b_00011111;
            storage[10] = 0;    //PCLATH
            storage[11] &= 0b_1;

            bank1[1] = 0b_11111111;
            bank1[5] = 0b_11111;
            bank1[6] = 0b_11111111;
            
            watchdog = false;
            prescaler = 0;
            wathcdogCtr = 1;
        }
        public void clearwatchdog() 
        {
            if (PSA() == true)
                prescaler = 0;
        }

        public void enterSleepMode() 
        {
            setPDFlag();
            inSleepMode = true; 
        }

        public bool getSleepStatus() { return inSleepMode; }

        #endregion



        #region Interrupts
        private void interruptTester(Stack stack)
        {
            bool interrupt = false;
            if (getBitFromRegister(11, 7) == 0)  //Interrupt Enable nicht gesetzt
                return;

            if ((getBitFromRegister(11, 5) == 1) &&  (getBitFromRegister(11, 2) == 1)) //Timer Interrupt
                interrupt = true;

            if ((getBitFromRegister(11, 3) == 1) && (getBitFromRegister(11, 0) == 1)) //Interrupt RB4-7
                interrupt = true;

            if ((getBitFromRegister(11, 4) == 1) && (getBitFromRegister(11, 1) == 1)) //RB0 (INT) Interrupt
                interrupt = true;

            //EEPROM Interrupt müsste an dieser Stelle realisiert werden, wird allerdings nicht geprüft

            if (interrupt)
                interruptHandler(stack);
            return;
        }

        private void interruptHandler(Stack stack)
        {
            writeBitInRegister(11, 7, false);   //GIE Bit rücksetzen
            stack.addNewElement(programCounter);    //Im Fetch wurde der Programmcounter schon iknrementierrt
                                                    //Also wird der Wert des nächsten Befehls auf den Stack geschoben
            programCounter = 4;     //Adresse 4 im Programmspeicher
            storage[2] = 4;     //PCL Register auch auf 4
        }
        #endregion



        #region Output
        private void PortOutputHandler()
        {
            if (!getTrisABit(0))  //PortA[0] ist Ausgang
                latch.setBitInPortAOut(0, getBitAtPositionFromInt(storage[5], 0));  //PortA[0] in den Ausgang


            if (!getTrisABit(1))  //PortA[1] ist Ausgang

                latch.setBitInPortAOut(1, getBitAtPositionFromInt(storage[5], 1));  //PortA[1] in den Ausgang


            if (!getTrisABit(2))  //PortA[2] ist Ausgang
                latch.setBitInPortAOut(2, getBitAtPositionFromInt(storage[5], 2));  //PortA[2] in den Ausgang


            if (!getTrisABit(3))  //PortA[3] ist Ausgang
                latch.setBitInPortAOut(3, getBitAtPositionFromInt(storage[5], 3));  //PortA[3] in den Ausgang


            if (!getTrisABit(4))  //PortA[4] ist Ausgang
                latch.setBitInPortAOut(4, getBitAtPositionFromInt(storage[5], 4));  //PortA[4] in den Ausgang
        


            if (!getTrisBBit(0))  //PortB[0] ist Ausgang
                latch.setBitInPortBOut(0, getBitAtPositionFromInt(storage[6], 0));  //PortB[0] in den Ausgang


            if (!getTrisBBit(1))  //PortB[1] ist Ausgang
                latch.setBitInPortBOut(1, getBitAtPositionFromInt(storage[6], 1));  //PortB[1] in den Ausgang


            if (!getTrisBBit(2))  //PortB[2] ist Ausgang
                latch.setBitInPortBOut(2, getBitAtPositionFromInt(storage[6], 2));  //PortB[2] in den Ausgang


            if (!getTrisBBit(3))  //PortB[3] ist Ausgang
                latch.setBitInPortBOut(3, getBitAtPositionFromInt(storage[6], 3));  //PortB[3] in den Ausgang


            if (!getTrisBBit(4))  //PortB[4] ist Ausgang
                latch.setBitInPortBOut(4, getBitAtPositionFromInt(storage[6], 4));  //PortB[4] in den Ausgang


            if (!getTrisBBit(5))  //PortB[5] ist Ausgang
                latch.setBitInPortBOut(5, getBitAtPositionFromInt(storage[6], 5));  //PortB[5] in den Ausgang


            if (!getTrisBBit(6))  //PortB[6] ist Ausgang
                latch.setBitInPortBOut(6, getBitAtPositionFromInt(storage[6], 6));  //PortB[6] in den Ausgang


            if (!getTrisBBit(7))  //PortB[7] ist Ausgang
                latch.setBitInPortBOut(7, getBitAtPositionFromInt(storage[6], 7));  //PortB[7] in den Ausgang
        }


        private bool getTrisABit(int bitNumber)
        {
            bool bit = false;
            int val = bank1[5];
            bit = getBitAtPositionFromInt(val, bitNumber);
            
            return bit;
        }

        private bool getTrisBBit(int bitNumber)
        {
            bool bit = false;
            int val = bank1[6];
            bit = getBitAtPositionFromInt(val, bitNumber);

            return bit;
        }
        #endregion



        #region PortA Input
        public void ra4Input(bool high)
        //high = true -> Low-to-High Flanke
        //high = false -> High-to-Low Flanke
        {
            //TMR0 Handler
            int option_reg = bank1[1];
            int T0CS = option_reg & 0b_00100000;

            if (T0CS == 0)  //TMRO hängt an Quarz Frequenz -> Ken Handel erforderlich
                return;

            int T0SE = option_reg & 0b_00010000;

            if (T0SE == 0)   //Low-to-High Flanke erforderlich 
            {
                if (!high)
                    return;
            }

            else           //High-to-Low Falnke erforderlich
            {
                if (high)
                    return;
            }

            TMR0increase();
        }
        #endregion



        #region PortB Input
        public void rb0Input(bool high)
        //high = true -> Low-to-High Flanke
        //high = false -> High-to-Low Flanke
        {
            int INTEDG = bank1[1] & 0b_01000000;  //Bit aus dem OPTION Register, das anzeigt, ob High-to-Low Flanke oder
                                                  //Low-to-High Flane für Interrupt erwartet wird

            if (INTEDG == 0)    //High-to-Low Flanke
            {
                if (!high)
                    storage[11] |= 0b_00000010;     //INTF Bit in INTCON Register setzten
            }

            else               //Low-to-High Flanke
            {
                if (high)
                    storage[11] |= 0b_00000010;
            }
        }

        public void rb4to7Input()
        {
            //RBIF Bit im INTCON Register wird gesetzt wenn es an RB4 - RB7 mindestens eine Zusstandsänderung gab
            //Das Rücksetzen dieses Bits muss im Assemblerprogramm geschehen, die Hardware ist dafür nicht verantwortlich
            storage[11] |= 0b_00000001;
        }
        #endregion



        #region EEPROM
        const double eepromDelayTime = 1000;   //us
        private void eepromWriteHandler(EEPROM eeprom)
        {
            eepromTimerCounter += timePerCycle;

            if (requiresTwoCycles)
                eepromTimerCounter += timePerCycle;

            if (eepromTimerCounter < eepromDelayTime)
                return;

            //EEPROM Schreib Zeit überschritten
            eeprom.writeIn(storage[9], storage[8]);
            eepromTimerCounter = 0;
            eepromWrite = false;
            bank1[8] |= 0b_10000;   //EEIF Bit zurücksetzen
        }
        #endregion


        #region private help functions
        //Hilfsfunktionen
        private bool registerOnBank1(int addr)
        {
            if (addr == 1 || addr == 5 || addr == 6 || addr == 8 || addr == 9)
            {
                if (getBitFromRegister(3, 5) == 1)//if(RP0 Bit == 1)
                    return true; ;
            }
            return false;
        }


        private bool getBitAtPositionFromInt(int value, int bitNumber)  //Bit als boolschen Wert aus einem Register bekommen
        {
            bool bit = false;
            value = value >> bitNumber;   //Zu untersuchendes Bit steht ganz rechts
            value &= 0b_1;
            if (value != 0)
                bit = true;

            return bit;
        }


        private bool isSpecialRegisterCalled(int registerAddr)
        {
            if (registerAddr == 0 || registerAddr == 2 || registerAddr == 8 || registerAddr == 9)  //Hier um spezielle register erweitern
                return true;
            return false;
        }


        private void specialRegisterHandler(int registerAddr, int value)
        {
            switch (registerAddr)
            {
                case 0:
                    int fsr = storage[4];
                    storage[fsr] = storage[0];  //Wert aus indirect in die adresse, die in FSR steht schreiben
                    break;

                //case 1:
                //    if (registerOnBank1(registerAddr) == false)     //Option Register Test
                //        return;
                //    optionRegisterChanged = true;
                //    break;

                case 2: //PCL register wurde verändert -> PC wird neu berechnet
                    programCounter = 0;
                    int pageIndex = (storage[10] << 8); //PCLATH kommt hier zum Einsatz
                    programCounter = pageIndex + storage[2];
                    break;

                case 8:
                    if(registerOnBank1(registerAddr) == false)    //Testen auf EECON1 Register
                        break;
                    
                    int rd_bit = bank1[8];   //EEPROM Read überprüfen
                    rd_bit &= 0b_1;
                    if(rd_bit > 0)
                    {
                        eepromRead = true;
                        bank1[8] &= 0b_11110;  //RD Bit zurücksetzen
                        break;
                    }

                    int wr_bit = bank1[8];   //EEPROM Write prüfen
                    wr_bit &= 0b_10;
                    int wren_bit = bank1[8];
                    wren_bit &= 0b_100;
                    if ((wr_bit > 0) && (wren_bit > 0) && (eepromStateCounter == 2))
                    {
                        eepromWrite = true;
                        eepromStateCounter = 0;  //State Maschine zurücksetzen
                        bank1[8] &= 0b_11101;    //WR Bit zurücksetzen
                    }
                    break;

                case 9:
                    if (registerOnBank1(registerAddr) == false)   //Testen auf EECON2 Register
                        break;
                    if (value == 85)                              //State Maschine hochzählen
                        eepromStateCounter = 1;
                    if (value == 170 && eepromStateCounter == 1)
                        eepromStateCounter = 2;
                    break;
            }
            return;
        }

        #endregion



        #region ViewModel functions
        public string[,] toViewModel()
        {
            int index = 0;
            string[,] data = new string[10, 16];
            
            //Bank0
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    data[i, j] = storage[index++].ToString("X2");
                }
            }

            index = 0;
            //Bank1
            for (int i = 5; i < 10; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (isBank1Index(index))
                        data[i, j] = bank1[index++].ToString("X2");
                    else
                    data[i, j] = storage[index++].ToString("X2");
                }
            }
            return data;
        }


        public void fromViewModel(string data, int index)
        {
            int testIndex = index - 80;

            if (isBank1Index(testIndex))  //Änderung nur auf Bank1
                bank1[testIndex] = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);

            else    //Änderung nur auf Bank0 oder auf beiden Bänken
            {
                if (index > 80)
                    index -= 80;
                storage[index] = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
            }
        }

        private bool isBank1Index(int index)
        {
            switch (index) 
            {
                case 1: return true;
                case 5: return true;
                case 6: return true;
                case 8: return true;
                case 9: return true;
            }
            return false;
        }
        #endregion
    }
}
