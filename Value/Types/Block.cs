using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class ArcBlock : IValue
{
	public ValueTypeCode TypeCode => ValueTypeCode.Block;
	public Block Value { get; set; }
	public ArcBlock(Block value)
	{
		if(Parser.HasEnclosingBrackets(value))
			value = Compiler.RemoveEnclosingBrackets(value);
		this.Value = value;
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		i = comp.Var(i, (Block s) => IValue.Parse(s), false, "args");

		string compiled = comp.Compile(Value);

		result.Add(compiled);

		return i;
	}
	public override string ToString()
	{
		throw new Exception();
	}
	public bool Fulfills(IValue v)
	{
		if (v.TypeCode != TypeCode)
			return false;
		return ((ArcBlock)v).Value == Value;
	}
}