using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimu.Model
{
    class Latch
    {
        private bool[] portA_Out;
        private bool[] portB_Out;
        

        public Latch()
        {
            portA_Out = new bool[5];
            portB_Out = new bool[8];
        }


        public void setBitInPortAOut(int bitNumber, bool set) 
        {
            if (set)
                portA_Out[bitNumber] = true;
            else
                portA_Out[bitNumber] = false;
        }
        public void setBitInPortBOut(int bitNumber, bool set)
        {
            if (set)
                portB_Out[bitNumber] = true;
            else
                portB_Out[bitNumber] = false;
        }

        public bool getBitInPortBOut(int num)
        {
            bool val = portB_Out[num];
            return val;
        }

        public bool getBitInPortAOut(int num)
        {
            bool val = portA_Out[num];
            return val;
        }


    }
}
