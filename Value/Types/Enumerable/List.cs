using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
namespace Arc;

public class ArcList : IArcEnumerable
{
	public ValueTypeCode TypeCode => ValueTypeCode.List;
	public LinkedList<IValue> List { get; set; }
	public ArcList()
	{
		List = new();
	}
	public ArcList(LinkedList<IValue> value)
	{
		List = value;
	}
	public ArcList(Block code)
	{
		List = new();

		if (Parser.HasEnclosingBrackets(code, "[", "]"))
			code = Compiler.RemoveEnclosingBrackets(code);

		Walker i = new(code);

		do
		{
			i = Compiler.GetValue(i, out Block words);
			List.AddLast(IValue.Parse(words));
		} while(i.MoveNext());
	}
	public Block ToBlock()
	{
		StringBuilder sb = new();
		sb.Append("{ ");
		foreach (IValue a in List)
		{
			sb.Append($"{a.ToBlock()}");
		}
		sb.Append(" }");
		return Parser.ParseCode(sb.ToString());
	}
	public override string ToString()
	{
		StringBuilder sb = new();
		foreach (IValue val in List)
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
	public bool Fulfills(IValue v)
	{
		if (v.TypeCode != TypeCode)
			return false;
		LinkedList<IValue> vlist = ((ArcList)v).List;
		if(vlist.Count != List.Count)
			return false;
		LinkedList<IValue>.Enumerator a = List.GetEnumerator();
		LinkedList<IValue>.Enumerator b = vlist.GetEnumerator();
		while(a.MoveNext() && b.MoveNext())
		{
			if(!a.Current.Fulfills(b.Current))
				return false;
		}
		return true;
	}
	public IEnumerator GetEnumerator()
	{
		return List.GetEnumerator();
	}
}