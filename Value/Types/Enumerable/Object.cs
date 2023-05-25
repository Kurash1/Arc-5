using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
namespace Arc;

public class ArcObject : IArcEnumerable
{
	public ValueTypeCode TypeCode => ValueTypeCode.Object;
	public Dictionary<string, IValue> Properties;
	public ArcObject()
	{
		Properties = new Dictionary<string, IValue>();
	}
	public ArcObject(Dictionary<string, IValue> value)
	{
		Properties = value;
	}
	public ArcObject(Block code)
	{
		Properties = new Dictionary<string, IValue>();

		if (Parser.HasEnclosingBrackets(code))
			code = Compiler.RemoveEnclosingBrackets(code);

		if (code.First != null)
		{
			Compiler comp = new();

			string result = comp.Compile(code);
			
			Block newBlock = Parser.ParseCode(result);
			if (newBlock.First != null)
			{
				Walker i = new(newBlock);
				do
				{
					i = comp.Var(i, (Block s) => IValue.Parse(s), false);
				} while (i.MoveNext());
			}

			foreach (KeyValuePair<string, IValue> kvp in comp.variables)
			{
				Properties.Add(kvp.Key, kvp.Value);
			}
		}
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		throw new NotImplementedException();
	}
	public IValue this[string key]
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
		foreach(KeyValuePair<string, IValue> kvp in ((ArcObject)v).Properties)
		{
			if (kvp.Key == "global")
				continue;
			if (!Properties.ContainsKey(kvp.Key))
				return false;
			if(!Properties[kvp.Key].Fulfills(kvp.Value))
				return false;
		}
		return true;
	}
	public Block ToBlock()
	{
		StringBuilder sb = new();
		sb.Append("{ ");
		foreach(KeyValuePair<string, IValue> kvp in Properties)
		{
			if (kvp.Key != "global")
				sb.Append($"{kvp.Value.TypeCode.ToString()} {kvp.Key} = {kvp.Value.ToBlock()}");
		}
		sb.Append(" }");
		return Parser.ParseCode(sb.ToString());
	}
	public new static string ToString() => "[Arc Object]";

	public IEnumerator GetEnumerator()
	{
		return Properties.GetEnumerator();
	}
}