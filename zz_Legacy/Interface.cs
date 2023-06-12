using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;
//public class ArcInterface : IValue, IEnumerable
//{
//	//Regular Stuff
//	public Dictionary<string, ArcPointer> Properties;
//	public ArcInterface()
//	{
//		Properties = new Dictionary<string, ArcPointer>();
//	}
//	public ArcInterface(Dictionary<string, ArcPointer> value)
//	{
//		Properties = value;
//	}
//	public ArcInterface(Block code)
//	{
//		Properties = new Dictionary<string, ArcPointer>();
//
//		if (!Parser.HasEnclosingBrackets(code))
//			throw new Exception("Object without enclosing brackets");
//		code = Compiler.RemoveEnclosingBrackets(code);
//
//		ArcObject ob = new(code);
//		foreach (KeyValuePair<string, ArcPointer> kvp in ob.Properties)
//		{
//			Properties.Add(kvp.Key, kvp.Value);
//		}
//	}
//
//	//
//	//public Walker Call(Walker i, ref List<string> result, Compiler comp)
//	//{
//	//	i.MoveNext();
//	//	string baseKey = i.Current;
//	//	Walker g = comp.Var(i, (Block s) => new ArcObject(s, this), false);
//	//	foreach(KeyValuePair<string, ArcPointer> kvp in Properties)
//	//	{
//	//		if(comp.TryGetVariable($"{baseKey}:{kvp.Key}", out ArcPointer? value))
//	//		{
//	//			if (value == null)
//	//				throw new Exception();
//	//
//	//			if (kvp.Value.Value.TypeCode == ValueTypeCode.Type)
//	//			{
//	//				if (!kvp.Value.Value.Fulfills(value.Value))
//	//				{
//	//					throw new Exception();
//	//				}
//	//			}
//	//		}
//	//		else
//	//		{
//	//			if(kvp.Value.Value.TypeCode != ValueTypeCode.Type)
//	//			{
//	//				(Dictionary<string, ArcPointer> dict, string key) = comp.GetNewVariable($"{baseKey}:{kvp.Key}");
//	//				dict[key] = new ArcPointer(ref kvp.Value.Value);
//	//			}
//	//			else
//	//				throw new Exception();
//	//		}
//	//	}
//	//	return g;
//	//}
//	//public Walker Call(Walker i, Dictionary<string, ArcPointer> vars)
//	//{
//	//	i.MoveNext();
//	//	string baseKey = i.Current;
//	//	Walker g = Compiler.Var(vars, i, (Block s) => new ArcObject(s, this), false);
//	//	foreach(KeyValuePair<string, ArcPointer> kvp in Properties)
//	//	{
//	//		if(Compiler.TryGetVariable(vars, $"{baseKey}:{kvp.Key}", out ArcPointer? value))
//	//		{
//	//			if (value == null)
//	//				throw new Exception();
//	//
//	//			if (kvp.Value.Value.TypeCode == ValueTypeCode.Type)
//	//			{
//	//				if (!kvp.Value.Value.Fulfills(value.Value))
//	//				{
//	//					throw new Exception();
//	//				}
//	//			}
//	//		}
//	//		else
//	//		{
//	//			if(kvp.Value.Value.TypeCode != ValueTypeCode.Type)
//	//			{
//	//				(Dictionary<string, ArcPointer> dict, string key) = Compiler.GetNewVariable(vars, $"{baseKey}:{kvp.Key}");
//	//				dict[key] = new ArcPointer(ref kvp.Value.Value);
//	//			}
//	//			else
//	//				throw new Exception();
//	//		}
//	//	}
//	//	return g;
//	//}
//	//public ArcPointer this[string key]
//	//{
//	//	get
//	//	{
//	//		if (Properties.ContainsKey(key))
//	//			return Properties[key];
//	//
//	//		return null;
//	//	}
//	//	set
//	//	{
//	//		Properties[key] = value;
//	//	}
//	//}
//	//public bool Fulfills(IValue v)
//	//{
//	//	if (v.TypeCode != TypeCode)
//	//		return false;
//	//	foreach (KeyValuePair<string, ArcPointer> kvp in ((ArcInterface)v).Properties)
//	//	{
//	//		if (!Properties.ContainsKey(kvp.Key))
//	//			return false;
//	//		if (!Properties[kvp.Key].Value.Fulfills(kvp.Value.Value))
//	//			return false;
//	//	}
//	//	return true;
//	//}
//	//public override string ToString()
//	//{
//	//	return "[Arc Interface]";
//	//}
//	//public IEnumerator GetEnumerator()
//	//{
//	//	return Properties.GetEnumerator();
//	//}
//	//public void Add(string key, ref IValue value)
//	//{
//	//	Properties.Add(key, new ArcPointer(ref value));
//	//}
//	//public void Add(string key, ArcPointer value)
//	//{
//	//	Properties.Add(key, value);
//	//}
//}