using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimu.Model
{
    class Stack
    {
        public int[] stackStorage;
        public int stackPointer;
        private int highestValue;
        public Stack()
        { 
            stackStorage = new int[8];
            stackPointer = 0;
        }

        public void addNewElement(int element)
        {
            stackStorage[stackPointer] = element;
            stackPointer++;
            if  (stackPointer >= 8) 
            {
                stackPointer = 0;
            }
        }

        public int getElement() 
        { 
            stackPointer--;
            if (stackPointer < 0)
            {
                stackPointer = 7;
            }
            int tmp = stackStorage[stackPointer];
            stackStorage[stackPointer] = 0;
            return tmp;  
        }

       


    }
}
