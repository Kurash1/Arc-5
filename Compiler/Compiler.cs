using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

using ArcInstance;
using System.Text;

namespace Arc
{
	public partial class Compiler
	{
	   public Dictionary<string, IValue> variables = new()
		{
			{ "global", global }
		};
		public static ArcObject global = new();

#pragma warning disable CS8618 // Fields are assigned by Arc5.cs
		public static string directory;
		public static Instance owner;
#pragma warning restore CS8618

		public Compiler() { }
		public string Compile(string file, bool preprocessor = false)
		{
			if (preprocessor)
				file = Parser.Preprocessor(file);
			return Compile(Parser.ParseCode(file));
		}
		public string Compile(Block code)
		{
			
			List<string> result = new();
			Dictionary<string, Func<Walker, Walker>> keywords = new()
			{
				{ "string", (Walker i) => Var(i, (Block s) => new ArcString(s) ) },
				{ "bool", (Walker i) => Var(i, (Block s) => new ArcBool(s) ) },
				{ "float", (Walker i) => Var(i, (Block s) => new ArcFloat(s) ) },
				{ "int", (Walker i) => Var(i, (Block s) => new ArcInt(s) ) },
				{ "var", (Walker i) => Var(i, (Block s) => IValue.Parse(s) ) },
				{ "object", (Walker i) => Var(i, (Block s) => new ArcObject(s) ) },
				{ "block", (Walker i) => Var(i, (Block s) => new ArcBlock(s) ) },
				{ "interface", (Walker i) => Var(i, (Block s) => new ArcInterface(s) ) },
				{ "list", (Walker i) => Var(i, (Block s) => new ArcList(s) ) },
				{ "type", (Walker i) => Var(i, (Block s) => new ArcType(s) ) },
				{ "inherit", (Walker i) => Inherit(i) },
				{ "require", (Walker i) => Require(i) },
				{ "foreach", (Walker i) => Foreach(i, ref result) },
			};

			Walker g = new(code);
			do
			{
				if (keywords.ContainsKey(g.Current))
				{
					g = keywords[g.Current].Invoke(g);
					continue;
				}
				else if (TryGetVariable(g.Current, out IValue? variable))
				{
					if(variable != null)
					{
						g = variable.Call(g, ref result, this);
					}
					continue;
				}
				else if(TryTrimOne(g.Current, '`', out string? newValue))
				{
					if (newValue == null)
						throw new Exception();
					result.Add(newValue);
					continue;
				}

				result.Add(g.Current);
			} while (g.MoveNext());

			StringBuilder res = new();
			foreach (string s in result)
			{
				res.Append($"{s.Trim()} ");
			}

			if (Defines.Formatting)
				return Parser.FormatCode(res.ToString());
			else
				return res.ToString();
		}
	}
}
