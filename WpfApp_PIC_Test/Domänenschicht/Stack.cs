using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_PIC.Domänenschicht;

public class Stack
{
    private int[] _register;
    private int _stackPointer;

    public Stack()
    {
        _register = new int[8];
        _stackPointer = 0;
    }
    public void Push(int value)
    {
        _register[_stackPointer] = value;
        IncrementStackPointer();
    }

    public int Pop()
    {
        DecrementStackPointer();
        int topOfTheStack = _register[_stackPointer];
        Push(0);
        return topOfTheStack;

    }



    private void DecrementStackPointer()
    {
        _stackPointer--;
        if (_stackPointer < 0)
        {
            _stackPointer = 7;
        }
    }
    private void IncrementStackPointer()
    {
        _stackPointer++;
        if (_stackPointer >= 8)
        {
            _stackPointer = 0;
        }
    }

}
