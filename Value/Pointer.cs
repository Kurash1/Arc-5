using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;
public class Pointer
{
	public IValue Value;
	public Pointer(ref IValue value)
	{
		Value = value;
	}
	public Pointer(string over = "")
	{
		if (over == "global")
			Value = Compiler.global;
		else
			throw new Exception();
	}
}