using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
	public IValue ThisConstruct(Block s, Dictionary<string, IValue>? vars) => Construct(s, vars);
	public static IValue Construct(Block s, Dictionary<string, IValue>? vars)
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
		if (i.MoveNext())
		{
			switch (i.Current)
			{
				case "+=":
					{
						if (!i.MoveNext())
							throw new Exception();

						i = Compiler.GetScope(i, out Block newbv);

						foreach(string s in newbv)
						{
							Value.Add(s);
						}
					}
					break;
				default:
					{
						i.MoveBack();
						string compiled = comp.Compile(Value);

						result.Add(compiled);
					}
					break;
			}
		}
		else
		{
			string compiled = comp.Compile(Value);

			result.Add(compiled);
		}
		return i;
	}
	public override string ToString()
	{
		return string.Join(' ', Value);
	}
}