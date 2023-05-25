using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public partial class Compiler
{
	public Walker Inherit(Walker i)
	{
		i.MoveNext(); //Previous is the inherit word

		if (!Parser.equal.IsMatch(i.Current))
			throw new Exception();

		i.MoveNext();

		if (TryGetVariable(i.Current, out IValue? variable))
		{
			if(variable == null)
				throw new Exception();

			if(variable.TypeCode == ValueTypeCode.Object)
			{
				ArcObject var = (ArcObject)variable;
				foreach (KeyValuePair<string, IValue> kvp in var.Properties)
				{
					if(kvp.Key != "global")
						variables.Add(kvp.Key, kvp.Value);
				}
			}
			else
			{
				throw new Exception();
			}
		}
		else
		{
			throw new Exception();
		}

		return i;
	}
}
