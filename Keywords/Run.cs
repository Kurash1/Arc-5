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
		i = TryGetKeyValue(i, out string _, out Block? value);

		if (value == null)
			throw new Exception();

		IValue TypeHeaders = new ArcType(ValueTypeCode.String);
		IValue TypeTarget = new ArcType(ValueTypeCode.String);
		IValue TypeFormatting = new ArcType(ValueTypeCode.Bool);
		IValue TypeSave = new ArcType(ValueTypeCode.Bool);
		ArcObject Right = new(value, new()
		{
			{ "headers", new Pointer(ref TypeHeaders) },
			{ "target", new Pointer(ref TypeTarget) },
			{ "formatting", new Pointer(ref TypeFormatting) },
			{ "save", new Pointer(ref TypeSave) }
		});

		if (!Right.Properties.ContainsKey("target"))
			throw new Exception();

		Defines def = new(Right);

		Instance.TranspileTarget(def);

		return i;
	}
}
