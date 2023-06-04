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
	public Dictionary<string, Pointer> Properties;
	public ArcObject()
	{
		Properties = new();
	}
	public ArcObject(Dictionary<string, Pointer> value)
	{
		Properties = value;
	}
	public ArcObject(Block code, ArcInterface? Interface = null)
	{
		Properties = new Dictionary<string, Pointer>()
		{
			{ "global", new Pointer("global") }
		};

		if (Parser.HasEnclosingBrackets(code))
			code = Compiler.RemoveEnclosingBrackets(code);

		if(code.Count == 0) return;

		Dictionary<string, Func<Walker, bool, Walker>> keywords = new()
		{
			{ "string", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcString(s), move) },
			{ "bool", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcBool(s), move) },
			{ "float", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcFloat(s), move) },
			{ "int", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcInt(s), move) },
			{ "object", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcObject(s), move) },
			{ "block", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcBlock(s), move) },
			{ "interface", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcInterface(s), move) },
			{ "list", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcList(s), move) },
			{ "array", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcArray(s), move) },
			{ "type", (Walker i, bool move) => Compiler.Var(Properties, i, (Block s) => new ArcType(s), move) },
			{ "inherit", (Walker i, bool _) => Compiler.Inherit(i, Properties) }
		};

		Walker i = new(code);

		do
		{
			if (keywords.ContainsKey(i.Current))
			{
				i = keywords[i.Current].Invoke(i, true);
				continue;
			}
			else if(Interface != null && Interface.Properties.ContainsKey(i.Current))
			{
				if(Interface[i.Current].Value.TypeCode == ValueTypeCode.Type)
				{
					i = keywords[
							((ArcType)Interface[i.Current].Value).Type.ToString().ToLower()
						].Invoke(i, false);
				}
				else
				{
					i = keywords[
							Interface[i.Current].Value.TypeCode.ToString().ToLower()
						].Invoke(i, false);
				}
			}
			else
			{
				Compiler.Var(Properties, i, (Block s) => IValue.Parse(s), false);
				continue;
			}
		} while (i.MoveNext());
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		throw new NotImplementedException();
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
		foreach(KeyValuePair<string, Pointer> kvp in ((ArcObject)v).Properties)
		{
			if (kvp.Key == "global")
				continue;
			if (!Properties.ContainsKey(kvp.Key))
				return false;
			if(!Properties[kvp.Key].Value.Fulfills(kvp.Value.Value))
				return false;
		}
		return true;
	}
	public new static string ToString() => "[Arc Object]";

	public IEnumerator GetEnumerator()
	{
		return Properties.GetEnumerator();
	}
}