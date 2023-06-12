using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
namespace Arc;

//public class ArcList : IArcEnumerable
//{
//	//Regular Stuff
//	public LinkedList<IValue> List { get; set; }
//	public IValue Type;
//	public ArcList()
//	{
//		List = new();
//	}
//	public ArcList(LinkedList<IValue> value)
//	{
//		List = value;
//	}
//	//Type Finding
//	public bool IsList() => true;
//	public ArcList AsList() => this;
//	//Contract
//	public IValue ThisConstruct(Block s) => Construct(s);
//	public static IValue Construct(Block s)
//	{
//		ArcList NewList = new ArcList();
//		NewList.List = new();
//		if (Parser.HasEnclosingBrackets(s))
//			s = Compiler.RemoveEnclosingBrackets(s);
//
//		Walker i = new(s);
//
//		do
//		{
//			i = Compiler.GetValue(i, out Block words);
//			IValue newValue = NewList.Type.ThisConstruct(words);
//			NewList.List.AddLast(new ArcPointer(ref newValue));
//		} while (i.MoveNext());
//
//		return NewList;
//	}
//	public bool Fulfills(IValue v)
//	{
//		if (!v.IsString())
//			return false;
//		ArcList a = v.AsList();
//		return List == a.List;
//	}
//	//Code
//	public Walker Call(Walker i, ref List<string> result, Compiler comp)
//	{
//		throw new NotImplementedException();
//	}
//	public override string ToString()
//	{
//		return string.Join(' ', List);
//	}
//	public IEnumerator GetEnumerator()
//	{
//		return List.GetEnumerator();
//	}
//}