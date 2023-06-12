using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class ArcPointer : IValue
{
	//Regular Stuff
	public IValue Value;
	public ArcPointer(ref IValue value)
	{
		Value = value;
	}
	public ArcPointer(IValue value)
	{
		Value = value;
	}
	
	//Type Finding
	public ArcFloat AsFloat() { return Value.AsFloat(); }
	public ArcInt AsInt() { return Value.AsInt(); }
	public ArcBool AsBool() { return Value.AsBool(); }
	public ArcString AsString() { return Value.AsString(); }
	public ArcObject AsObject() { return Value.AsObject(); }
	public ArcBlock AsBlock() { return Value.AsBlock(); }
	public bool IsFloat() => Value.IsFloat();
	public bool IsInt() => Value.IsInt();
	public bool IsBool() => Value.IsBool();
	public bool IsString() => Value.IsString();
	public bool IsObject() => Value.IsObject();
	public bool IsBlock() => Value.IsBlock();

	public IValue GetCopy() => Value.GetCopy();

	public IValue GetOrigin() => Value.GetOrigin();
	//Contract
	public IValue ThisConstruct(Block s) => Construct(s);
	public IValue Construct(Block s)
	{
		return Value.ThisConstruct(s);
	}
	public bool Fulfills(IValue v)
	{
		return Value.Fulfills(v);
	}

	//Code
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		return Value.Call(i, ref result, comp);
	}
	public override string ToString()
	{
		return Value.ToString();
	}
}