using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht
{
    public class Decoder
    {
        private Instructions _instructions;
        private ProgramCounterService _programCounterService;
        private ProgramMemoryService _programMemoryService;

        public Decoder(Instructions instructions, ProgramCounterService programCounterService, ProgramMemoryService programMemoryService)
        {
            _instructions = instructions;
            _programCounterService = programCounterService;
            _programMemoryService = programMemoryService;
        }

        public void Decode()
        {
            int index = _programCounterService.GetPC();

            _programCounterService.IncreasePC(); //Fetch Zyklus

            int opcode = _programMemoryService.GetValue(index);

            #region Hilfsfunktionen zu BYTE-ORIENTED FILE REGISTER OPERATIONS
            //Highbyte (zum erkennen des Befehls) zurückgeben
            static int extractSixMSBsOpcode(int Opcode)
            {
                int highbyte;
                highbyte = Opcode >> 8;
                return highbyte;
            }

            static int extractSeventhMSBsOpcode(int Opcode)
            {
                int bit = Opcode >> 7;
                bit &= 0b_1;
                return bit;
            }

            #endregion
            try
            {
                #region BYTE-ORIENTED FILE REGISTER OPERATIONS
                switch (extractSixMSBsOpcode(opcode))
                {
                    case 7:
                        _instructions.addwf(opcode);
                        break;

                    case 2:
                        _instructions.subwf(opcode);
                        break;

                    case 5:
                        _instructions.andwf(opcode);
                        break;

                    case 4:
                        _instructions.iorwf(opcode);
                        break;

                    case 6:
                        _instructions.xorwf(opcode);
                        break;

                    case 1:
                        switch (extractSeventhMSBsOpcode(opcode))
                        {
                            case 0:
                                _instructions.clrw();
                                break;

                            case 1:
                                _instructions.clrf(opcode);
                                break;
                        }
                        //0000011 0001100
                        break;

                    case 9:
                        _instructions.comf(opcode);
                        break;

                    case 3:
                        _instructions.decf(opcode);
                        break;

                    case 10:
                        _instructions.incf(opcode);
                        break;

                    case 8:
                        _instructions.movf(opcode);
                        break;

                    case 0:
                        switch (extractSeventhMSBsOpcode(opcode))
                        {
                            case 0:
                                if (opcode == 0)
                                    _instructions.nop();
                                break;

                            case 1:
                                _instructions.movwf(opcode);
                                break;
                        }
                        break;

                    case 14:
                        _instructions.swapf(opcode);
                        break;

                    case 13:
                        _instructions.rlf(opcode);
                        break;

                    case 12:
                        _instructions.rrf(opcode);
                        break;

                    case 11:
                        _instructions.decfsz(opcode);
                        break;

                    case 15:
                        _instructions.incfsz(opcode);
                        break;
                }
                #endregion

                #region Hilfsfunktionen zu BIT_ORIENTED FILE REGISTER OPERATIONS
                static int extractFourthMSBsOpcode(int Opcode)
                {
                    int bit;

                    return bit = (Opcode & 0b_11110000000000) >> 10;
                }
                #endregion

                #region BIT_ORIENTED FILE REGISTER OPERATIONS
                switch (extractFourthMSBsOpcode(opcode))
                {
                    case 4:
                        _instructions.bcf(opcode);
                        break;

                    case 5:
                        _instructions.bsf(opcode);
                        break;

                    case 6:
                        _instructions.btfsc(opcode);
                        break;

                    case 7:
                        _instructions.btfss(opcode);
                        break;
                }
                #endregion

                #region Hilfsfunktionen zu LITERAL AND CONTROL OPERATIONS
                static int extractThirdMSBsOpcode(int Opcode)
                {
                    int bit = Opcode & 0b_11100000000000;

                    return (bit >> 11);
                }
                #endregion

                #region LITERAL AND CONTROL OPERATIONS
                switch (extractThirdMSBsOpcode(opcode))
                {
                    case 4:
                        _instructions.call(opcode);
                        break;

                    case 5:
                        _instructions.goTo(opcode);
                        break;
                }

                switch (extractSixMSBsOpcode(opcode))
                {
                    case 62 or 63:
                        _instructions.addlw(opcode);
                        break;

                    case 57:
                        _instructions.andlw(opcode);
                        break;

                    case 60 or 61:
                        _instructions.sublw(opcode);
                        break;

                    case 56:
                        _instructions.iorlw(opcode);
                        break;

                    case 58:
                        _instructions.xorlw(opcode);
                        break;

                    case 48 or 49 or 50 or 51:
                        _instructions.movlw(opcode);
                        break;

                    case 52 or 53 or 54 or 55:
                        _instructions.returnlw(opcode);
                        break;
                }

                switch (opcode)
                {
                    case 8:
                        _instructions.Return();
                        break;

                    case 100:
                        _instructions.clrwdt();
                        break;

                    case 99:
                        _instructions.sleep();
                        break;

                    case 9:
                        _instructions.retfie();
                        break;

                    default:
                        throw new Exception("Decoder konnte den eingelesenen Opcode als keinen gültigen Befehl auswerten.");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }
    }
}
