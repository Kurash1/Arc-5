using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;

public class ArcInt : ArcNumber
{
    public ValueTypeCode TypeCode => ValueTypeCode.Int;
    public int Value { get; set; }

    public ArcInt(int value)
    {
        Value = value;
    }
    public ArcInt(string value)
    {
        Value = int.Parse(value);
    }
    public ArcInt(Block value)
    {
        if (value.Count > 1) throw new Exception("Too many elements given to ArcString");
        if (value.Count < 0) throw new Exception("Too few elements given to ArcString");
        Value = new ArcInt(value.First.Value).Value;
    }
    public Block ToBlock()
    {
        return new Block(Value.ToString());
    }
    public bool Equals(Value v)
    {
        if (v.TypeCode != TypeCode)
            return false;
        return ((ArcInt)v).Value == Value;
    }
    public static bool operator ==(ArcInt obj1, Value obj2)
    {
        return obj1.Equals(obj2);
    }
    public static bool operator !=(ArcInt obj1, Value obj2)
    {
        return !obj1.Equals(obj2);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
    public double getNum() => Value;
    public static implicit operator double(ArcInt d) => d.Value;
    public static implicit operator ArcInt(int d) => new ArcInt(d);
}