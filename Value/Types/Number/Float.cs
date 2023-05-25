using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;
public class ArcFloat : IArcNumber
{
	public ValueTypeCode TypeCode => ValueTypeCode.Float;
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
	public override string ToString()
	{
		return Value.ToString("0.000");
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		result.Add(ToString());
		return i;
	}
	public double GetNum() => Value;
	public static implicit operator double(ArcFloat d) => d.Value;
	public static implicit operator ArcFloat(double d) => new(d);
	public Block ToBlock()
	{
		return new Block(Value.ToString());
	}
	public bool Fulfills(IValue v)
	{
		if (v.TypeCode != TypeCode)
			return false;
		return ((ArcFloat)v).Value == Value;
	}
}