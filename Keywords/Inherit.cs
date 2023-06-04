﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public partial class Compiler
{
	public Walker Inherit(Walker i)
	{
		return Inherit(i, variables);
	}
	public static Walker Inherit(Walker i, Dictionary<string, Pointer> vars)
	{
		i.MoveNext(); //Previous is the inherit word

		if (!Parser.equal.IsMatch(i.Current))
			throw new Exception();

		i.MoveNext();

		if (TryGetVariable(vars, i.Current, out Pointer? variable))
		{
			if(variable == null)
				throw new Exception();

			if(variable.Value.TypeCode == ValueTypeCode.Object)
			{
				ArcObject var = (ArcObject)variable.Value;
				foreach (KeyValuePair<string, Pointer> kvp in var.Properties)
				{
					if(kvp.Key != "global")
						vars.Add(kvp.Key, kvp.Value);
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
