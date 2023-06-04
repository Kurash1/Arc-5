using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;
public class ArcArray : IValue
{
	public ValueTypeCode TypeCode => ValueTypeCode.Array;
	public LinkedList<Pointer> List { get; set; }
	public ArcArray()
	{
		List = new LinkedList<Pointer>();
	}
	public ArcArray(LinkedList<Pointer> value)
	{
		List = value;
	}
	public ArcArray(Block code)
	{
		List = new LinkedList<Pointer>();

		if (!Parser.HasEnclosingBrackets(code, "[", "]"))
			throw new Exception("Object without enclosing brackets");
		code = Compiler.RemoveEnclosingBrackets(code);

		ArcList ob = new(code);
		List = ob.List;
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		i.MoveNext();
		return Call(i, comp.variables);
	}
	public Walker Call(Walker i, Dictionary<string, Pointer> vars)
	{
		Walker g = Compiler.Var(vars, i, (Block s) => new ArcList(s, List), false);
		return g;
	}
	public bool Fulfills(IValue v)
	{
		if (v.TypeCode != TypeCode)
			return false;
		LinkedList<Pointer> vlist = ((ArcList)v).List;
		if (vlist.Count != List.Count)
			return false;
		LinkedList<Pointer>.Enumerator a = List.GetEnumerator();
		LinkedList<Pointer>.Enumerator b = vlist.GetEnumerator();
		while (a.MoveNext() && b.MoveNext())
		{
			if (!a.Current.Value.Fulfills(b.Current.Value))
				return false;
		}
		return true;
	}
	public override string ToString()
	{
		return "[Arc Array]";
	}
}