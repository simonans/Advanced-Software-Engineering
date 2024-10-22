using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PicSimu.Model
{
    internal class Decoder
    {
        public void selctor(Programmspeicher programmspeicher, W_Register w_Register, Datenspeicher datenspeicher, Stack stack, EEPROM eeprom)
            {

            int index = datenspeicher.getProgramCounter();
            
            datenspeicher.increaseProgramCounter(); //Fetch Zyklus

            int opcode = programmspeicher.getStorageField(index);

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
                        Befehle.addwf(opcode, w_Register, datenspeicher);
                        break;

                    case 2:
                        Befehle.subwf(opcode, w_Register, datenspeicher);
                        break;

                    case 5:
                        Befehle.andwf(opcode, w_Register, datenspeicher);
                        break;

                    case 4:
                        Befehle.iorwf(opcode, w_Register, datenspeicher);
                        break;

                    case 6:
                        Befehle.xorwf(opcode, w_Register, datenspeicher);
                        break;

                    case 1:
                        switch(extractSeventhMSBsOpcode(opcode))
                        {
                            case 0:
                                Befehle.clrw(w_Register, datenspeicher);
                                break;

                            case 1:
                                Befehle.clrf(opcode, w_Register, datenspeicher);
                                break;
                        }
                        //0000011 0001100
                        break;

                    case 9:
                        Befehle.comf(opcode, w_Register, datenspeicher);
                        break;

                    case 3:
                        Befehle.decf(opcode, w_Register, datenspeicher);
                        break;

                    case 10:
                        Befehle.incf(opcode, w_Register, datenspeicher);
                        break;

                    case 8:
                        Befehle.movf(opcode, w_Register, datenspeicher);
                        break;

                    case 0:
                        switch (extractSeventhMSBsOpcode(opcode))
                        {
                            case 0:
                                if (opcode == 0)
                                    Befehle.nop();                             
                                break;

                            case 1:
                                Befehle.movwf(opcode, w_Register, datenspeicher);
                                break;
                        }
                        break;

                    case 14:
                        Befehle.swapf(opcode, w_Register, datenspeicher);
                        break;

                    case 13:
                        Befehle.rlf(opcode, w_Register, datenspeicher);
                        break;

                    case 12:
                        Befehle.rrf(opcode, w_Register, datenspeicher);
                        break;

                    case 11:
                        Befehle.decfsz(opcode, w_Register, datenspeicher);
                        break;

                    case 15:
                        Befehle.incfsz(opcode, w_Register, datenspeicher);
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
                        Befehle.bcf(opcode, datenspeicher);
                        break;

                    case 5:
                        Befehle.bsf(opcode, datenspeicher);
                        break;

                    case 6:
                        Befehle.btfsc(opcode, datenspeicher);
                        break;

                    case 7:
                        Befehle.btfss(opcode, datenspeicher);
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
                        Befehle.call(opcode, datenspeicher, stack);
                        break;

                    case 5:
                        Befehle.goTo(opcode, datenspeicher, stack);
                        break;
                }

                switch (extractSixMSBsOpcode(opcode))
                {
                    case 62 or 63:
                        Befehle.addlw(opcode, w_Register, datenspeicher);
                        break;

                    case 57:
                        Befehle.andlw(opcode, w_Register, datenspeicher);
                        break;

                    case 60 or 61:
                        Befehle.sublw(opcode, w_Register, datenspeicher);
                        break;

                    case 56:
                        Befehle.iorlw(opcode, w_Register, datenspeicher);
                        break;

                    case 58:
                        Befehle.xorlw(opcode, w_Register, datenspeicher);
                        break;

                    case 48 or 49 or 50 or 51:
                        Befehle.movlw(opcode, w_Register, datenspeicher);
                        break;

                    case 52 or 53 or 54 or 55:
                        Befehle.returnlw(opcode, datenspeicher, stack, w_Register);
                        break;
                }

                switch (opcode)
                {
                    case 8:
                        Befehle.Return(datenspeicher, stack);
                        break;

                    case 100:
                        Befehle.clrwdt(datenspeicher);
                        break;

                    case 99:
                        Befehle.sleep(datenspeicher);
                        break;

                    case 9:
                        Befehle.retfie(datenspeicher, stack);
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






//  11000000010001
//  11111100000000
