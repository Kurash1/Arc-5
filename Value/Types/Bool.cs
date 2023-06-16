using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;
public class ArcBool : IValue
{
	//Regular Stuff
	public bool Value { get; set; }

	public ArcBool(bool value)
	{
		Value = value;
	}
	public ArcBool(string value)
	{
		value = value.Replace("yes", "true");
		value = value.Replace("no", "false");
		Value = bool.Parse(value);
	}
	public ArcBool(Block value)
	{
		if (value.Count > 1) 
			throw new Exception("Too many elements given to ArcString");
		if (value.Count < 0) 
			throw new Exception("Too few elements given to ArcString");
		if (value.First == null)
			throw new Exception();
		Value = new ArcBool(value.First.Value).Value;
	}
	//Type Finding
	public ArcBool AsBool() => this;
	public bool IsBool() => true;
	public IValue GetCopy() => new ArcBool(Value);
	//Contract
	public static IValue Construct(Block s, Dictionary<string, IValue>? vars) 
	{ 
		return new ArcBool(s);
	}
	public IValue ThisConstruct(Block s, Dictionary<string, IValue>? vars)
	{
		return Construct(s, vars);
	}
	public bool Fulfills(IValue v)
	{
		if (!v.IsBool())
			return false;
		return v.AsBool().Value == Value;
	}
	//Code
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		if (i.MoveNext())
		{
			if (i.Current == "=")
			{
				if (!i.MoveNext())
					throw new Exception();

				Compiler.GetScope(i, out Block scope);
				Value = Construct(scope, comp.variables).AsBool().Value;
			}
			else
			{
				i.MoveBack();
				result.Add(ToString().Replace("True", "yes").Replace("False", "no"));
			}
		}

		return i;
	}
	public override string ToString()
	{
		return Value.ToString();
	}
}