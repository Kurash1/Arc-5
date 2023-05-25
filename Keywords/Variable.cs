using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public partial class Compiler
{
	public Walker Var<T>(Walker i, Func<Block,T> Constructor, bool move = true, string? keyOverride = null) where T : IValue
	{
		if(move)
			i.MoveNext(); //The previous spot is the datatype

		i = GetKeyValue(i, out string key, out Block? value);

		if (!Parser.IsVariableKey(key))
			throw new Exception("Invalid Variable Key");

		if (value == null || value.First == null)
			throw new Exception();

		T Value;
		if (value != null && value.Count == 1 && TryGetVariable(value.First.Value, out IValue? NewValue))
		{
			if (NewValue == null)
				throw new Exception();

			Value = Constructor(NewValue.ToBlock());
		}
		else {
			if (value == null)
				throw new Exception();

			Value = Constructor(value);
		}

		if (keyOverride != null)
			key = keyOverride;

		(Dictionary<string, IValue> dict, string key) Val = GetNewVariable(key); //This will start as null

		Val.dict[Val.key] = Value;

		return i;
	}
}
