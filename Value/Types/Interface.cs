using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;
public class ArcInterface : IValue, IEnumerable
{
	public ValueTypeCode TypeCode => ValueTypeCode.Object;
	public Dictionary<string, Pointer> Properties;
	public ArcInterface()
	{
		Properties = new Dictionary<string, Pointer>();
	}
	public ArcInterface(Dictionary<string, Pointer> value)
	{
		Properties = value;
	}
	public ArcInterface(Block code)
	{
		Properties = new Dictionary<string, Pointer>();

		if (!Parser.HasEnclosingBrackets(code))
			throw new Exception("Object without enclosing brackets");
		code = Compiler.RemoveEnclosingBrackets(code);

		ArcObject ob = new(code);
		foreach (KeyValuePair<string, Pointer> kvp in ob.Properties)
		{
			Properties.Add(kvp.Key, kvp.Value);
		}
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		i.MoveNext();
		string baseKey = i.Current;
		Walker g = comp.Var(i, (Block s) => new ArcObject(s, this), false);
		foreach(KeyValuePair<string, Pointer> kvp in Properties)
		{
			if(comp.TryGetVariable($"{baseKey}:{kvp.Key}", out Pointer? value))
			{
				if (value == null)
					throw new Exception();

				if (kvp.Value.Value.TypeCode == ValueTypeCode.Type)
				{
					if (!kvp.Value.Value.Fulfills(value.Value))
					{
						throw new Exception();
					}
				}
			}
			else
			{
				if(kvp.Value.Value.TypeCode != ValueTypeCode.Type)
				{
					(Dictionary<string, Pointer> dict, string key) = comp.GetNewVariable($"{baseKey}:{kvp.Key}");
					dict[key] = new Pointer(ref kvp.Value.Value);
				}
				else
					throw new Exception();
			}
		}
		return g;
	}
	public Walker Call(Walker i, Dictionary<string, Pointer> vars)
	{
		i.MoveNext();
		string baseKey = i.Current;
		Walker g = Compiler.Var(vars, i, (Block s) => new ArcObject(s, this), false);
		foreach(KeyValuePair<string, Pointer> kvp in Properties)
		{
			if(Compiler.TryGetVariable(vars, $"{baseKey}:{kvp.Key}", out Pointer? value))
			{
				if (value == null)
					throw new Exception();

				if (kvp.Value.Value.TypeCode == ValueTypeCode.Type)
				{
					if (!kvp.Value.Value.Fulfills(value.Value))
					{
						throw new Exception();
					}
				}
			}
			else
			{
				if(kvp.Value.Value.TypeCode != ValueTypeCode.Type)
				{
					(Dictionary<string, Pointer> dict, string key) = Compiler.GetNewVariable(vars, $"{baseKey}:{kvp.Key}");
					dict[key] = new Pointer(ref kvp.Value.Value);
				}
				else
					throw new Exception();
			}
		}
		return g;
	}
	public Pointer this[string key]
	{
		get
		{
			if (Properties.ContainsKey(key))
				return Properties[key];

			return null;
		}
		set
		{
			Properties[key] = value;
		}
	}
	public bool Fulfills(IValue v)
	{
		if (v.TypeCode != TypeCode)
			return false;
		foreach (KeyValuePair<string, Pointer> kvp in ((ArcInterface)v).Properties)
		{
			if (!Properties.ContainsKey(kvp.Key))
				return false;
			if (!Properties[kvp.Key].Value.Fulfills(kvp.Value.Value))
				return false;
		}
		return true;
	}
	public override string ToString()
	{
		return "[Arc Interface]";
	}
	public IEnumerator GetEnumerator()
	{
		return Properties.GetEnumerator();
	}
	public void Add(string key, ref IValue value)
	{
		Properties.Add(key, new Pointer(ref value));
	}
	public void Add(string key, Pointer value)
	{
		Properties.Add(key, value);
	}
}