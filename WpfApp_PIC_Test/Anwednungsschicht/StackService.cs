using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_PIC.Domänenschicht;

namespace WpfApp_PIC.Anwednungsschicht;
public class StackService
{
    private readonly Stack _stack;
    public event EventHandler StatusChanged;

    public StackService(Stack stack)
    {
        _stack = stack;
    }

    public void Push(int value)
    {
        _stack.Push(value);
        OnStatusChanged();
    }

    public int Pop()
    {
        int value = _stack.Pop();
        OnStatusChanged();
        return value;
    }
    public IEnumerable<int> GetStackValues()
    {
        return _stack.GetValues();
    }
    protected virtual void OnStatusChanged()
    {
        StatusChanged?.Invoke(this, EventArgs.Empty);
    }
}

