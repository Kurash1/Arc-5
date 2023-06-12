using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
namespace Arc;

//public class ArcType : IValue
//{
//	//Regular Stuff
//	public Func<Block, IValue> Constructor;
//	public Func<IValue, bool> Contract;
//	public ArcType(Func<Block, IValue> Constructor, Func<IValue, bool> Contract)
//	{
//		this.Constructor = Constructor;
//		this.Contract = Contract;
//	}
//
//	//Type Finding
//	public bool IsType() => true;
//	public ArcType AsType() => this;
//
//	//Contract
//	public IValue ThisConstruct(Block s) => Construct(s);
//	public static IValue Construct(Block s)
//	{
//		if (s.First == null)
//			throw new Exception("Empty value not valid for ArcType");
//		Func<Block, IValue> Constructor;
//		Func<IValue, bool> Contract;
//		switch (s.First.Value)
//		{
//			case "type":
//				Constructor = Construct;
//				Contract = TypeContract;
//				break;
//			default:
//				throw new NotImplementedException();
//		}
//		return new ArcType(Constructor, Contract);
//	}
//	public static bool TypeContract(IValue v)
//	{
//		if (!v.IsType())
//			return false;
//		ArcType _ = (ArcType)v;
//		return true;
//	}
//	public bool Fulfills(IValue v)
//	{
//		return Contract(v);
//	}
//
//	//Code
//	public Walker Call(Walker i, Dictionary<string, ArcPointer> vars)
//	{
//		return Compiler.Var(vars, i, Constructor);
//	}
//	public Walker Call(Walker i, ref List<string> result, Compiler comp)
//	{
//		return Call(i, comp.variables);
//	}
//}