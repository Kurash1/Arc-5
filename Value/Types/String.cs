using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class ArcString : IValue
{
	//Regular Stuff
	public string Value { get; set; }
	public ArcString(string value)
	{
		Value = value;
	}

	//Type Finding
	public bool IsString() => true;
	public ArcString AsString() => this;
	public IValue GetCopy() => new ArcString(Value);
	//Contract
	public IValue ThisConstruct(Block s) => Construct(s);
	public static IValue Construct(Block s)
	{
		string NewString = string.Join(' ', s);
		ArcString NewArcString = new ArcString(NewString);
		return NewArcString;
	}
	public bool Fulfills(IValue v)
	{
		if (!v.IsString())
			return false;
		ArcString a = v.AsString();
		return Value == a.Value;
	}

	//Code
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		if (i.MoveNext())
		{
			if(i.Current == "=")
			{
				if (!i.MoveNext())
					throw new Exception();

				Compiler.GetScope(i, out Block scope);
				Value = Construct(scope).AsString().Value;
			}
			else
			{
				i.MoveBack();
				result.Add(Value);
			}
		}
		else
		{
			i.MoveBack();
			result.Add(Value);
		}

		return i;
	}

	public override string ToString()
	{
		return Value.ToString();
	}
}