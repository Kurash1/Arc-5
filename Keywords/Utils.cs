using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arc;
public partial class Compiler
{
	public static Walker GetScope(Walker i, out Block scope)
	{
		scope = new();	
		int indent = 0;
		do
		{
			if(Parser.open.IsMatch(i.Current))
				indent++;
			if (Parser.close.IsMatch(i.Current))
				indent--;
			scope.AddLast(i.Current);
			if(indent > 0)
				i.MoveNext();
		} while (indent > 0);
		return i;
	}
	public static bool TryTrimOne(string value, char s, out string? newValue)
	{
		newValue = null;
		if(value == null)
			return false;
		if (value.Length < 2)
			return false;
		if (value[0] != s)
			return false;
		if (value[^1] != s)
			return false;
		newValue = value[1..^1];
		return true;
	}
	public static Block RemoveEnclosingBrackets(Block scope)
	{
		scope.RemoveFirst();
		scope.RemoveLast();
		return scope;
	}
	public static Walker GetValue(Walker i, out Block value)
	{
		value = new();
		if (!Parser.open.IsMatch(i.Current))
		{
			value.AddLast(i.Current);
		}
		else
		{
			i = GetScope(i, out Block scope);
			value = scope;
		}
		return i;
	}
	public static Walker TryGetKeyValue(Walker i, out string key, out Block? value)
	{
		key = i.Current;
		
		i.MoveNext();

		if (!Parser.equal.IsMatch(i.Current))
		{
			i.MoveBack();
			value = null;
			return i;
		}

		i.MoveNext();

		i = GetValue(i, out value);

		return i;
	}
	public static Walker GetKeyValue(Walker i, out string key, out Block? value)
	{
		i = TryGetKeyValue(i, out key, out value);
		if (value == null)
			throw new Exception();
		return i;
	}
	public static string StringListSum(List<string> list)
	{
		return string.Join(" ", list);
	}

	public (Dictionary<string, IValue> dict, string key) GetNewVariable(string locator)
	{
		if (locator.Contains(':'))
		{
			string[] KeyLocator = locator.Split(':');
			int f = 0;
			Dictionary<string, IValue> currentDict = variables;
			string currentKey;
			do
			{
				currentKey = KeyLocator[f];
				if (currentDict.ContainsKey(currentKey))
				{
					if (currentDict[currentKey].TypeCode == ValueTypeCode.Object)
					{
						currentDict = ((ArcObject)currentDict[currentKey]).Properties;
					}
					else
					{
						throw new Exception($"Tried assigning to a key in a nonobjectKey");
					}
				}
				f++;
			} while (KeyLocator.Length > f);
		
			if (!currentDict.ContainsKey(currentKey))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
				currentDict.Add(currentKey, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			return (currentDict, currentKey);
		}
		else
		{
			if (!variables.ContainsKey(locator))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
				variables.Add(locator, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			return (variables,locator);
		}
	}
	public bool TryGetVariable(string locator, out IValue? var)
	{
		if (locator.Contains(':'))
		{
			string[] KeyLocator = locator.Split(':');
			int f = 0;
			Dictionary<string, IValue> currentDict = variables;
			string currentKey;
			do
			{
				currentKey = KeyLocator[f];
				if (currentDict.ContainsKey(currentKey))
				{
					if (KeyLocator.Length > f + 1)
					{
						if (currentDict[currentKey].TypeCode == ValueTypeCode.Object)
						{
							currentDict = ((ArcObject)currentDict[currentKey]).Properties;
						}
					}
				}
				else
				{
					var = null;
					return false;
				}
				f++;
			} while (KeyLocator.Length > f);
			if (!currentDict.ContainsKey(currentKey))
			{
				var = null;
				return false;
			}
			var = currentDict[currentKey];
			return true;
		}
		else
		{
			if (!variables.ContainsKey(locator))
			{
				var = null;
				return false;
			}
			var = variables[locator];
			return true;
		}
	}
}