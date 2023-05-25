using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class ArcBool : IValue
{
	public ValueTypeCode TypeCode => ValueTypeCode.Bool;
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
	public override string ToString()
	{
		string value = Value.ToString();
		value = value.Replace("true", "yes");
		value = value.Replace("false", "no");
		return value;
	}
	public static implicit operator bool(ArcBool d) => d.Value;
	public static implicit operator ArcBool(bool d) => new(d);
	public Block ToBlock()
	{
		return new Block(Value.ToString());
	}
	public bool Fulfills(IValue v)
	{
		if (v.TypeCode != TypeCode)
			return false;
		return ((ArcBool)v).Value == Value;
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		result.Add(ToString());
		return i;
	}
}