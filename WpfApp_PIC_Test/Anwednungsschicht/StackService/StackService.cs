using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht.StackService;
public class StackService : IStackService
{
    private readonly Stack _stack;

    public StackService(Stack stack)
    {
        _stack = stack;
    }

    public void Push(int value)
    {
        _stack.Push(value);
    }

    public int Pop()
    {
        return _stack.Pop();
    }
}

