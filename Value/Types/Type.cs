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
        if(!Enum.IsDefined(typeof(ValueTypeCode), vare))
        {
            throw new Exception();
        }
        Type = (ValueTypeCode)Enum.Parse(typeof(ValueTypeCode), vare);
    }
    public Walker Call(Walker i, ref List<string> result, Compiler comp)
    {
        Dictionary<string, Func<Walker, Walker>> keywords = new()
        {
            { "string", (Walker i) => comp.Var(i, (Block s) => new ArcString(s)) },
            { "bool", (Walker i) => comp.Var(i, (Block s) => new ArcBool(s)) },
            { "float", (Walker i) => comp.Var(i, (Block s) => new ArcFloat(s)) },
            { "int", (Walker i) => comp.Var(i, (Block s) => new ArcInt(s)) },
            { "object", (Walker i) => comp.Var(i, (Block s) => new ArcObject(s)) },
            { "interface", (Walker i) => comp.Var(i, (Block s) => new ArcInterface(s)) },
            { "list", (Walker i) => comp.Var(i, (Block s) => new ArcList(s)) },
            { "type", (Walker i) => comp.Var(i, (Block s) => new ArcType(s)) }
        };
        i = keywords[Type.ToString().ToLower()](i);
        return i;
    }
    public bool Fulfills(Value v)
    {
        return v.TypeCode == Type;
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