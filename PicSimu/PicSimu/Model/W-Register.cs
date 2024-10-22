using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimu.Model
{
    class W_Register
    {
        private int storage;

        public W_Register() { storage = 0; }

        public void writeIn(int value) 
        {
            storage = value; 
        }
        public int getValue() { return storage; }

        public void wregisterReset() { storage = 0; }
    }
}
