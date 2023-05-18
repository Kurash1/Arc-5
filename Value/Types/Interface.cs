using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;
public class ArcInterface : Value
{
    public ValueTypeCode TypeCode => ValueTypeCode.Object;
    public Dictionary<string, Value> Properties;
    public ArcInterface()
    {
        Properties = new Dictionary<string, Value>();
    }
    public ArcInterface(Dictionary<string, Value> value)
    {
        Properties = value;
    }
    public ArcInterface(Block code)
    {
        Properties = new Dictionary<string, Value>();

        if (!Parser.HasEnclosingBrackets(code))
            throw new Exception("Object without enclosing brackets");
        code = Compiler.RemoveEnclosingBrackets(code);

        Compiler comp = new Compiler();

        comp.compile(code);
        foreach (KeyValuePair<string, Value> kvp in comp.variables)
        {
            Properties.Add(kvp.Key, kvp.Value);
        }
    }
    public Block.Enumerator Call(Block.Enumerator i, ref List<string> result, Compiler comp)
    {
        Block.Enumerator g = comp.Var(i, (Block s) => new ArcObject(s));
        i.MoveNext();
        string baseKey = i.Current;
        foreach(KeyValuePair<string, Value> kvp in Properties)
        {
            if(comp.TryGetVariable($"{baseKey}:{kvp.Key}", out Value value))
            {
                if (kvp.Value.TypeCode == ValueTypeCode.Type)
                {
                    if (!kvp.Value.Equals(value))
                    {
                        throw new Exception();
                    }
                }
            }
            else
            {
                if(kvp.Value.TypeCode != ValueTypeCode.Type)
                {
                    var a = comp.GetNewVariable($"{baseKey}:{kvp.Key}");
                    a.dict[a.key] = kvp.Value;
                }
                else
                    throw new Exception();
            }
        }
        return g;
    }
    public Value this[string key]
    {
        get
        {
            if (Properties.ContainsKey(key))
                return Properties[key];

            return null;
        }
        set
        {
            Properties[key] = value;
        }
    }
    public bool Equals(Value v)
    {
        if (v.TypeCode != TypeCode)
            return false;
        foreach (KeyValuePair<string, Value> kvp in ((ArcInterface)v).Properties)
        {
            if (!Properties.ContainsKey(kvp.Key))
                return false;
            if (!Properties[kvp.Key].Equals(kvp.Value))
                return false;
        }
        return true;
    }
    public Block ToBlock()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{ ");
        foreach (KeyValuePair<string, Value> kvp in Properties)
        {
            if (kvp.Key != "global")
                sb.Append($"{kvp.Value.TypeCode.ToString()} {kvp.Key} = {kvp.Value.ToBlock()}");
        }
        sb.Append(" }");
        return Parser.ParseCode(sb.ToString());
    }
    public static bool operator ==(ArcInterface obj1, Value obj2)
    {
        return obj1.Equals(obj2);
    }
    public static bool operator !=(ArcInterface obj1, Value obj2)
    {
        return !obj1.Equals(obj2);
    }
    public static string ToString()
    {
        return "[Arc Interface]";
    }
}