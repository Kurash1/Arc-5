using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;
public class ArcFloat : IArcNumber
{
	//Regular Stuff
	public double Value { get; set; }
	public ArcFloat(double value)
	{
		Value = value;
	}
	public ArcFloat(string value)
	{
		Value = double.Parse(value);
	}
	public ArcFloat(Block value)
	{
		if (value.Count > 1) 
			throw new Exception("Too many elements given to ArcString");
		if (value.Count < 0) 
			throw new Exception("Too few elements given to ArcString");
		if (value.First == null)
			throw new Exception();
		Value = new ArcFloat(value.First.Value).Value;
	}
	public double GetNum() => Value;
	//TypeFinding
	public ArcFloat AsFloat() => this;
	public bool IsFloat() => true;
	public IValue GetCopy() => new ArcFloat(Value);
	//Contract
	public static IValue Construct(Block s, Dictionary<string, IValue>? vars) => new ArcFloat(s);
	public IValue ThisConstruct(Block s, Dictionary<string, IValue>? vars) => Construct(s, vars);
	public bool Fulfills(IValue v)
	{
		if (!v.IsFloat())
			return false;
		return v.AsFloat().Value == Value;
	}
	//Code
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		if (i.MoveNext())
		{
			switch (i.Current)
			{
				case "+=":
					{
						if (!i.MoveNext())
							throw new Exception();

						Value += float.Parse(i.Current);
					}
					break;
				case "-=":
					{
						if (!i.MoveNext())
							throw new Exception();

						Value -= float.Parse(i.Current);
					}
					break;
				case ":=":
					{
						if (!i.MoveNext())
							throw new Exception();

						Compiler.GetScope(i, out Block scope);
						Value = Construct(scope, comp.variables).AsFloat().Value;
					}
					break;
				default:
					{
						i.MoveBack();
						result.Add(Value.ToString("0.000"));
					}
					break;
			}
		}
		return i;
	}
	public override string ToString()
	{
		return Value.ToString();
	}
}