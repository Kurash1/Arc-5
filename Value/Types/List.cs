using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
namespace Arc;

public class ArcList : Value
{
    public ValueTypeCode TypeCode => ValueTypeCode.List;
    public LinkedList<Value> List { get; set; }
    public ArcList()
    {
        List = new();
    }
    public ArcList(LinkedList<Value> value)
    {
        List = value;
    }
    public ArcList(Block code)
    {
        List = new();

        if (Parser.HasEnclosingBrackets(code))
            code = Compiler.RemoveEnclosingBrackets(code);

        Walker i = new(code);

        do
        {
            i = Compiler.GetValue(i, out Block words);
            List.AddLast(Value.Parse(words));
        } while(i.MoveNext());
    }
    public Block ToBlock()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{ ");
        foreach (Value a in List)
        {
            sb.Append($"{a.ToBlock()}");
        }
        sb.Append(" }");
        return Parser.ParseCode(sb.ToString());
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Value val in List)
        {
            sb.Append($"{val.ToString()} ");
        }

        string s = sb.ToString();
        return s;
    }
    public Walker Call(Walker i, ref List<string> result, Compiler comp)
    {
        throw new NotImplementedException();
    }
    public bool Fulfills(Value v)
    {
        if (v.TypeCode != TypeCode)
            return false;
        LinkedList<Value> vlist = ((ArcList)v).List;
        if(vlist.Count != List.Count)
            return false;
        LinkedList<Value>.Enumerator a = List.GetEnumerator();
        LinkedList<Value>.Enumerator b = vlist.GetEnumerator();
        while(a.MoveNext() && b.MoveNext())
        {
            if(!a.Current.Fulfills(b.Current))
                return false;
        }
        return true;
    }
    public static bool operator ==(ArcList obj1, Value obj2)
    {
        return obj1.Fulfills(obj2);
    }
    public static bool operator !=(ArcList obj1, Value obj2)
    {
        return !obj1.Fulfills(obj2);
    }
}