using System;
using System.Collections.Generic;
using System.Text;

namespace Arc;
public interface IValue
{
	public IValue GetOrigin() => this;

	public ArcFloat AsFloat() { throw new NotImplementedException(); }
	public ArcInt AsInt() { throw new NotImplementedException(); }
	public ArcBool AsBool() { throw new NotImplementedException(); }
	public ArcString AsString() { throw new NotImplementedException(); }
	//public ArcType AsType() { throw new NotImplementedException(); }
	//public ArcList AsList() { throw new NotImplementedException(); }
	public ArcObject AsObject() { throw new NotImplementedException(); }
	public ArcBlock AsBlock() { throw new NotImplementedException(); }
	//public ArcInterface AsInterface() { throw new NotImplementedException(); }
	//public ArcArray AsArray() { throw new NotImplementedException(); }

	public bool IsFloat() => false;
	public bool IsInt() => false;
	public bool IsBool() => false;
	public bool IsString() => false;
	public bool IsType() => false;
	public bool IsList() => false;
	public bool IsObject() => false;
	public bool IsBlock() => false;
	public bool IsInterface() => false;
	public bool IsArray() => false;

	public IValue GetCopy();

	public static IValue Construct(Block s) { throw new NotImplementedException(); }
	public IValue ThisConstruct(Block s);
	public bool Fulfills(IValue v);
	public Walker Call(Walker i, ref List<string> result, Compiler comp);

	bool IsNumber() => false;
}