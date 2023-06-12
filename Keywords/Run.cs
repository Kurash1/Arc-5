using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcInstance;
namespace Arc;

public partial class Compiler
{
	public static Walker Run(Walker i)
	{
		i = TryGetKeyValue(i, out string _, out Block? value, out bool Copy);
	
		if (value == null)
			throw new Exception();

		ArcObject Right = ArcObject.Construct(value, Defines.DefinesInterface).AsObject();
	
		if (!Right.Properties.ContainsKey("target"))
			throw new Exception();
	
		Defines def = new(Right);
	
		Instance.TranspileTarget(def);
	
		return i;
	}
}
