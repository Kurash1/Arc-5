using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
namespace Arc;

public class ArcObject : Value
{
    public ValueTypeCode TypeCode => ValueTypeCode.Object;
    public Dictionary<string, Value> Properties;
    public ArcObject()
    {
        Properties = new Dictionary<string, Value>();
    }
    public ArcObject(Dictionary<string, Value> value)
    {
        Properties = value;
    }
    public ArcObject(Block code)
    {
        Properties = new Dictionary<string, Value>();

        if (!Parser.HasEnclosingBrackets(code))
            throw new Exception("Object without enclosing brackets");
        code = Compiler.RemoveEnclosingBrackets(code);

        Compiler comp = new Compiler();

        comp.compile(code);
        foreach(KeyValuePair<string, Value> kvp in comp.variables)
        {
            Properties.Add(kvp.Key, kvp.Value);
        }
    }
    public Block.Enumerator Call(Block.Enumerator i, ref List<string> result, Compiler comp)
    {
        throw new NotImplementedException();
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
        foreach(KeyValuePair<string, Value> kvp in ((ArcObject)v).Properties)
        {
            if (!Properties.ContainsKey(kvp.Key))
                return false;
            if(!Properties[kvp.Key].Equals(kvp.Value))
                return false;
        }
        return true;
    }
    public Block ToBlock()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{ ");
        foreach(KeyValuePair<string, Value> kvp in Properties)
        {
            if (kvp.Key != "global")
                sb.Append($"{kvp.Value.TypeCode.ToString()} {kvp.Key} = {kvp.Value.ToBlock()}");
        }
        sb.Append(" }");
        return Parser.ParseString(sb.ToString());
    }
    public static bool operator ==(ArcObject obj1, Value obj2)
    {
        return obj1.Equals(obj2);
    }
    public static bool operator !=(ArcObject obj1, Value obj2)
    {
        return !obj1.Equals(obj2);
    }
    public override string ToString() => ToString(true);
    public string ToString(bool CausesWarning = true)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{ ");
        foreach (KeyValuePair<string, Value> kvp in Properties)
        {
            if(kvp.Key != "global")
                if(kvp.Value.TypeCode == ValueTypeCode.Object)
                    sb.Append($"{kvp.Value.TypeCode.ToString()} {kvp.Key} = {((ArcObject)kvp.Value).ToString(false)} ");
                else
                    sb.Append($"{kvp.Value.TypeCode.ToString()} {kvp.Key} = {kvp.Value.ToString()} ");
        }
        sb.Append(" } ");

        string s = sb.ToString();
        if(!CausesWarning)
            Console.WriteLine($"Warning: Trying to print object {s}".Pastel(ConsoleColor.Yellow));
        return s;
    }
}