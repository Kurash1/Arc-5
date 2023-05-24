using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class ArcBlock : Value
{
    public ValueTypeCode TypeCode => ValueTypeCode.Block;
    public Block value { get; set; }
    public ArcBlock(Block value)
    {
        if(Parser.HasEnclosingBrackets(value))
            value = Compiler.RemoveEnclosingBrackets(value);
        this.value = value;
    }
    public Walker Call(Walker i, ref List<string> result, Compiler comp)
    {
        i = comp.Var(i, (Block s) => Value.Parse(s), false, "args");

        string compiled = comp.compile(value);

        result.Add(compiled);

        return i;
    }
    public override string ToString()
    {
        throw new Exception();
    }
    public Block ToBlock()
    {
        throw new Exception();
    }
    public bool Fulfills(Value v)
    {
        if (v.TypeCode != TypeCode)
            return false;
        return ((ArcBlock)v).value == value;
    }
}