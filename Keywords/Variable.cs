using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public partial class Compiler
{
	public Walker Var<T>(Walker i, Func<Block, T> Constructor, bool move = true, string? keyOverride = null) where T : IValue
	{
		return Var(variables, i, Constructor, move, keyOverride);
	}
	public static Walker Var<T>(Dictionary<string, Pointer> vars, Walker i, Func<Block,T> Constructor, bool move = true, string? keyOverride = null) where T : IValue
	{
		if(move)
			i.MoveNext(); //The previous spot is the datatype

		i = GetKeyValue(i, out string key, out Block? value);

		if (!Parser.IsVariableKey(key))
			throw new Exception("Invalid Variable Key");

		if (value == null || value.First == null)
			throw new Exception();

		IValue Value;
		if (value.Count == 1 && TryGetVariable(vars, value.First.Value, out Pointer? NewValue))
		{
			if (NewValue == null)
				throw new Exception();

			Value = NewValue.Value;
		}
		else {
			if (value == null)
				throw new Exception();

			Value = Constructor(value);
		}

		if (keyOverride != null)
			key = keyOverride;

		(Dictionary<string, Pointer> dict, string key) Val = GetNewVariable(vars, key); //This will start as null

		Val.dict[Val.key] = new Pointer(ref Value);

		return i;
	}
}
