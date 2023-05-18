using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class ArcString : Value
{
    public ValueTypeCode TypeCode => ValueTypeCode.String;
    public string Value { get; set; }

    public ArcString(string value)
    {
        Value = value;
    }
    public ArcString(Block value)
    {
        if (value.Count > 1) throw new Exception("Too many elements given to ArcString");
        if (value.Count < 0) throw new Exception("Too few elements given to ArcString");
        Value = new ArcString(value.First.Value).Value;
    }
    public Block.Enumerator Call(Block.Enumerator i, ref List<string> result, Compiler comp) 
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
    public bool Fulfills(Value v)
    {
        if(v.TypeCode != TypeCode)
            return false;
        return ((ArcString)v).Value == Value;
    }
    public static bool operator ==(ArcString obj1, Value obj2)
    {
        return obj1.Fulfills(obj2);
    }
    public static bool operator !=(ArcString obj1, Value obj2)
    {
        return !obj1.Fulfills(obj2);
    }

    public bool StartsWith(string s) => Value.StartsWith(s);
    public bool EndsWith(string s) => Value.EndsWith(s);
    public string Substring(int start, int length) => Value.Substring(start, length);
    public int Length => Value.Length;
    public bool Contains(string s) => Value.Contains(s);

    public static implicit operator string(ArcString d) => d.Value;
    public static implicit operator ArcString(string d) => new ArcString(d);
}