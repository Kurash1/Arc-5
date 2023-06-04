using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public partial class Compiler
{
	public Walker Foreach(Walker i, ref List<string> result)
	{
		i.MoveNext(); //Previous is the inherit word

		(Dictionary<string, Pointer> dict, string key) = GetNewVariable(i.Current);

		i.MoveNext();

		if (i.Current != "in")
			throw new Exception();

		i.MoveNext();

		if (!TryGetVariable(i.Current, out Pointer? enumerableObject))
			throw new Exception();
		if (enumerableObject == null)
			throw new Exception();

		i.MoveNext();

		if(!Parser.equal.IsMatch(i.Current))
			throw new Exception();

		i.MoveNext();

		GetValue(i, out Block codeblock);

		if (Parser.HasEnclosingBrackets(codeblock))
			RemoveEnclosingBrackets(codeblock);

		if(enumerableObject.Value.TypeCode == ValueTypeCode.Object)
		{
			foreach (KeyValuePair<string, Pointer> kvp in (ArcObject)enumerableObject.Value)
			{
				if (kvp.Key == "global")
					continue;
				IValue Newkey = new ArcString(kvp.Key);
				IValue NewValue = new ArcObject(new Dictionary<string, Pointer>()
				{
					{ "key", new Pointer(ref Newkey) },
					{ "value", new Pointer(ref kvp.Value.Value) }
				});
				dict[key] = new Pointer(ref NewValue);

				result.Add(Compile(codeblock));

				dict.Remove(key);
			}
		}
		else if(enumerableObject.Value.TypeCode == ValueTypeCode.List)
		{
			ArcList lst = (ArcList)enumerableObject.Value;
			foreach(Pointer val in lst)
			{
				dict[key] = new Pointer(ref val.Value);

				result.Add(Compile(codeblock));

				dict.Remove(key);
			}
		}

		return i;
	}
}
