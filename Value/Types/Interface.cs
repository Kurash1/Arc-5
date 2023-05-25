using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;
public class ArcInterface : IValue
{
    public ValueTypeCode TypeCode => ValueTypeCode.Object;
    public Dictionary<string, IValue> Properties;
    public ArcInterface()
    {
        Properties = new Dictionary<string, IValue>();
    }
    public ArcInterface(Dictionary<string, IValue> value)
    {
        Properties = value;
    }
    public ArcInterface(Block code)
    {
        Properties = new Dictionary<string, IValue>();

        if (!Parser.HasEnclosingBrackets(code))
            throw new Exception("Object without enclosing brackets");
        code = Compiler.RemoveEnclosingBrackets(code);

        Compiler comp = new();

        comp.Compile(code);
        foreach (KeyValuePair<string, IValue> kvp in comp.variables)
        {
            Properties.Add(kvp.Key, kvp.Value);
        }
    }
    public Walker Call(Walker i, ref List<string> result, Compiler comp)
    {
        i.MoveNext();
        string baseKey = i.Current;
        Walker g = comp.Var(i, (Block s) => new ArcObject(s), false);
        foreach(KeyValuePair<string, IValue> kvp in Properties)
        {
            if(comp.TryGetVariable($"{baseKey}:{kvp.Key}", out IValue? value))
            {
                if (value == null)
                    throw new Exception();

                if (kvp.Value.TypeCode == ValueTypeCode.Type)
                {
                    if (!kvp.Value.Fulfills(value))
                    {
                        throw new Exception();
                    }
                }
            }
            else
            {
                if(kvp.Value.TypeCode != ValueTypeCode.Type)
                {
                    (Dictionary<string, IValue> dict, string key) = comp.GetNewVariable($"{baseKey}:{kvp.Key}");
                    dict[key] = kvp.Value;
                }
                else
                    throw new Exception();
            }
        }
        return g;
    }
    public IValue this[string key]
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
    public bool Fulfills(IValue v)
    {
        if (v.TypeCode != TypeCode)
            return false;
        foreach (KeyValuePair<string, IValue> kvp in ((ArcInterface)v).Properties)
        {
            if (!Properties.ContainsKey(kvp.Key))
                return false;
            if (!Properties[kvp.Key].Fulfills(kvp.Value))
                return false;
        }
        return true;
    }
    public Block ToBlock()
    {
        StringBuilder sb = new();
        sb.Append("{ ");
        foreach (KeyValuePair<string, IValue> kvp in Properties)
        {
            if (kvp.Key != "global")
                sb.Append($"{kvp.Value.TypeCode.ToString()} {kvp.Key} = {kvp.Value.ToBlock()}");
        }
        sb.Append(" }");
        return Parser.ParseCode(sb.ToString());
    }
    public override string ToString()
    {
        return "[Arc Interface]";
    }
}