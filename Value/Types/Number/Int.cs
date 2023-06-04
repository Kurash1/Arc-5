using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;

public class ArcInt : IArcNumber
{
	public ValueTypeCode TypeCode => ValueTypeCode.Int;
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
	public bool Fulfills(IValue v)
	{
		if (v.TypeCode != TypeCode)
			return false;
		return ((ArcInt)v).Value == Value;
	}

	public override string ToString()
	{
		return Value.ToString();
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		bool Operated = false;
		if(i.MoveNext())
		{
			if(i.Current == "+=")
			{
				if (i.MoveNext())
				{
					Value += int.Parse(i.Current);
					Operated = true;
				}
				else
					throw new Exception();
			}
			else if(i.Current == "-=")
			{
				if (i.MoveNext())
				{
					Value -= int.Parse(i.Current);
					Operated = true;
				}
				else
					throw new Exception();
			}
			else
			{
				i.MoveBack();
			}
		}
		if(!Operated)
			result.Add(ToString());
		return i;
	}
	public double GetNum() => Value;
	public static implicit operator double(ArcInt d) => d.Value;
	public static implicit operator ArcInt(int d) => new(d);
}