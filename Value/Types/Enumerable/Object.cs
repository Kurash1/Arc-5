using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
namespace Arc;

public class ArcObject : IArcEnumerable
{
	//Regular Stuff
	public Dictionary<string, IValue> Properties;
	public ArcObject()
	{
		Properties = new();
	}
	public ArcObject(Dictionary<string, IValue> value)
	{
		Properties = value;
	}
	//Type Finding
	public bool IsObject() => true;
	public ArcObject AsObject() => this;
	public IValue GetCopy() => new ArcObject(Properties);
	//Contract
	public IValue ThisConstruct(Block code, Dictionary<string, IValue>? vars) {
		ArcObject t = Construct(code, Contract(), vars).AsObject();
		foreach(KeyValuePair<string, IValue> pair in Properties)
		{
			if(!t.Properties.ContainsKey(pair.Key))
			{
				t.Properties.Add(pair.Key, new ArcPointer(pair.Value));
			}
		}
		return t;
	}
	public static IValue Construct(Block code, Dictionary<string, IValue>? vars)
	{
		return Construct(code, new(), vars);
	}
	public static IValue Construct(Block code, Dictionary<string, Func<Block, Dictionary<string, IValue>?, IValue>> Types, Dictionary<string, IValue>? vars)
	{
		ArcObject newValue = new()
		{
			Properties = new()
		};

		if (Parser.HasEnclosingBrackets(code))
			code = Compiler.RemoveEnclosingBrackets(code);

		if (code.Count == 0) return newValue;

		Dictionary<string, Func<Walker, Walker>> keywords = new()
		{
			{ "string", (Walker i) => Compiler.Var(newValue.Properties, vars, i, ArcString.Construct) },
			{ "bool", (Walker i) => Compiler.Var(newValue.Properties, vars, i, ArcBool.Construct) },
			{ "float", (Walker i) => Compiler.Var(newValue.Properties, vars, i, ArcFloat.Construct) },
			{ "int", (Walker i) => Compiler.Var(newValue.Properties, vars, i, ArcInt.Construct) },
			{ "object", (Walker i) => Compiler.Var(newValue.Properties, vars, i, ArcObject.Construct) },
			{ "block", (Walker i) => Compiler.Var(newValue.Properties, vars, i, ArcBlock.Construct) }
		};

		Walker i = new(code);

		do
		{
			if (keywords.ContainsKey(i.Current))
			{
				i = keywords[i.Current](i);
				continue;
			}
			else if (Types.ContainsKey(i.Current))
			{
				i = Compiler.Var(newValue.Properties, vars, i, Types[i.Current], false);
				continue;
			}
			else if(Compiler.TryGetVariable(vars, i.Current, out IValue? var))
			{
				if (var == null)
					throw new Exception();

				i = Compiler.Var(newValue.Properties, vars, i, var.ThisConstruct);
				continue;
			}
			else
			{
				throw new Exception($"Unknown Datatype for {i.Current}");
			}
		} while (i.MoveNext());

		return newValue;
	}
	public bool Fulfills(IValue v)
	{
		if (!v.IsObject())
			return false;
		ArcObject a = v.AsObject();
		return Properties == a.Properties;
	}
	//Code
	public Dictionary<string, Func<Block, Dictionary<string, IValue>?, IValue>> Contract()
	{
		Dictionary<string, Func<Block, Dictionary<string, IValue>?, IValue>> contract = new();
		foreach(KeyValuePair<string, IValue> i in Properties)
		{
			contract.Add(i.Key, i.Value.ThisConstruct);
		}
		return contract;
	}
	public Walker Call(Walker i, ref List<string> result, Compiler comp)
	{
		return comp.Var(i, ThisConstruct);
	}
	public override string ToString()
	{
		return string.Join(' ', Properties);
	}
	public IEnumerator GetEnumerator()
	{
		return Properties.GetEnumerator();
	}
}