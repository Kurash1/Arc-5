using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
namespace Arc;

public class ArcType : Value
{
    public ValueTypeCode TypeCode => ValueTypeCode.Type;
    public ValueTypeCode Type;
    public ArcType()
    {
        Type = ValueTypeCode.Type;
    }
    public ArcType(ValueTypeCode value)
    {
        Type = value;
    }
    public ArcType(Block value)
    {
        if (value.Count > 1) throw new Exception("Too many elements given to ArcType");
        if (value.Count < 0) throw new Exception("Too few elements given to ArcType");

        string vare = value.First.Value.ToString();
        char firstChar = char.ToUpper(vare[0]);
        vare = firstChar + vare.Substring(1);

        Type = (ValueTypeCode)Enum.Parse(typeof(ValueTypeCode), vare);
    }

    public bool Equals(Value v)
    {
        if (v.TypeCode != TypeCode)
            return false;
        return ((ArcType)v).Type == Type;
    }
    public Block ToBlock()
    {
        return Parser.ParseString(Type.ToString());
    }
    public override string ToString()
    {
        return Type.ToString();
    }
}