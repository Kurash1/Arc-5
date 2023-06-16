using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;

public class ArcInt : IArcNumber
{
	//Regular Stuff
	public int Value { get; set; }
	public ArcInt(int value)
	{
		Value = value;
	}
	public ArcInt(string value)
	{
		Value = int.Parse(value);
	}
	public ArcInt(Block value)
	{
		if (value.Count > 1) 
			throw new Exception("Too many elements given to ArcString");
		if (value.Count < 0) 
			throw new Exception("Too few elements given to ArcString");
		if (value.First == null)
			throw new Exception();
		Value = new ArcInt(value.First.Value).Value;
	}
	public double GetNum() => Value;
	//Type Finding
	public ArcInt AsInt() => this;
	public bool IsInt() => true;
	public IValue GetCopy() => new ArcInt(Value);
	//Contract
	public static IValue Construct(Block s, Dictionary<string, IValue>? vars)
	{
		return new ArcInt(s);
	}
	public IValue ThisConstruct(Block s, Dictionary<string, IValue>? vars) => Construct(s, vars);
	public bool Fulfills(IValue v)
	{
		if (!v.IsInt())
			return false;
		return v.AsInt().Value == Value;
	}

	//Code
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		if(i.MoveNext())
		{
			switch (i.Current)
			{
				case "+=":
					{
						if (!i.MoveNext())
							throw new Exception();

						Value += int.Parse(i.Current);
					}
					break;
				case "-=":
					{
						if (!i.MoveNext())
							throw new Exception();

						Value -= int.Parse(i.Current);
					}
					break;
				case ":=":
					{
						if (!i.MoveNext())
							throw new Exception();

						Compiler.GetScope(i, out Block scope);
						Value = Construct(scope, comp.variables).AsInt().Value;
					}
					break;
				default:
					{
						i.MoveBack();
						result.Add(Value.ToString());
					}
					break;
			}
		}
		else
		{
			result.Add(Value.ToString());
		}
		return i;
	}
	public override string ToString()
	{
		return Value.ToString();
	}
}