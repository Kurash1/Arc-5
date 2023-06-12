using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;
public class ArcBlock : IValue
{
	//Regular Stuff
	public Block Value { get; set; }
	public ArcBlock(Block value)
	{
		if (Parser.HasEnclosingBrackets(value))
			value = Compiler.RemoveEnclosingBrackets(value);
		Value = value;
	}
	//Type Finding
	public bool IsBlock() => true;
	public ArcBlock AsBlock() => this;
	public IValue GetCopy() => new ArcBlock(Value);
	//Contract
	public IValue ThisConstruct(Block s) => Construct(s);
	public static IValue Construct(Block s)
	{
		return new ArcBlock(s);
	}
	public bool Fulfills(IValue v)
	{
		if(!v.IsBlock()) 
			return false;
		ArcBlock a = v.AsBlock();
		return Value == a.Value;
	}
	//Code
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		//i = comp.Var(i, (Block s) => IValue.Parse(s), false, "args");

		string compiled = comp.Compile(Value);

		result.Add(compiled);

		return i;
	}
	public override string ToString()
	{
		return string.Join(' ', Value);
	}
}