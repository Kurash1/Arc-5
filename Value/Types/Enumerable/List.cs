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
	public LinkedList<Pointer> List { get; set; }
	public ArcList()
	{
		List = new();
	}
	public ArcList(LinkedList<Pointer> value)
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
			IValue newValue = IValue.Parse(words);
			List.AddLast(new Pointer(ref newValue));
		} while(i.MoveNext());
	}
	public ArcList(Block code, LinkedList<Pointer> arr)
	{
		List = new();

		if (Parser.HasEnclosingBrackets(code, "[", "]"))
			code = Compiler.RemoveEnclosingBrackets(code);

		Walker i = new(code);

		Dictionary<string, Func<Block, IValue>> keywords = new()
		{
			{ "string", (Block s) => new ArcString(s) },
			{ "bool", (Block s) => new ArcBool(s) },
			{ "float", (Block s) => new ArcFloat(s) },
			{ "int", (Block s) => new ArcInt(s) },
			{ "object", (Block s) => new ArcObject(s) },
			{ "block", (Block s) => new ArcBlock(s) },
			{ "interface", (Block s) => new ArcInterface(s) },
			{ "list", (Block s) => new ArcList(s) },
			{ "array", (Block s) => new ArcArray(s) },
			{ "type", (Block s) => new ArcType(s) }
		};

		foreach(Pointer pointer in arr)
		{
			i = Compiler.GetValue(i, out Block words);
			IValue newValue = keywords[((ArcType)pointer.Value).Type.ToString().ToLower()](words);
			List.AddLast(new Pointer(ref newValue));
			if (!i.MoveNext())
				throw new Exception();
		}
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
		LinkedList<Pointer> vlist = ((ArcList)v).List;
		if(vlist.Count != List.Count)
			return false;
		LinkedList<Pointer>.Enumerator a = List.GetEnumerator();
		LinkedList<Pointer>.Enumerator b = vlist.GetEnumerator();
		while(a.MoveNext() && b.MoveNext())
		{
			if(!a.Current.Value.Fulfills(b.Current.Value))
				return false;
		}
		return true;
	}
	public IEnumerator GetEnumerator()
	{
		return List.GetEnumerator();
	}
}