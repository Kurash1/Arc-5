using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class ArcString : IValue
{
    public ValueTypeCode TypeCode => ValueTypeCode.String;
    public string Value { get; set; }

    public ArcString(string value)
    {
        Value = value;
    }
    public ArcString(Block value)
    {
        if (value.Count > 1) 
            throw new Exception("Too many elements given to ArcString");
        if (value.Count < 0) 
            throw new Exception("Too few elements given to ArcString");
        if (value.First == null) 
            throw new Exception();
        Value = new ArcString(value.First.Value).Value;
    }
    public Walker Call(Walker i, ref List<string> result, Compiler comp) 
    {
        result.Add(Value);
        return i;
    }
    public override string ToString()
    {
        return Value;
    }
    public Block ToBlock()
    {
        return new Block(Value);
    }
    public bool Fulfills(IValue v)
    {
        if(v.TypeCode != TypeCode)
            return false;
        return ((ArcString)v).Value == Value;
    }
    public bool StartsWith(string s) => Value.StartsWith(s);
    public bool EndsWith(string s) => Value.EndsWith(s);
    public string Substring(int start, int length) => Value.Substring(start, length);
    public int Length => Value.Length;
    public bool Contains(string s) => Value.Contains(s);

    public static implicit operator string(ArcString d) => d.Value;
    public static implicit operator ArcString(string d) => new(d);
}