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
	
		(Dictionary<string, IValue> dict, string key) = GetNewVariable(i.Current);
	
		i.MoveNext();
	
		if (i.Current != "in")
			throw new Exception();
	
		i.MoveNext();
	
		if (!TryGetVariable(i.Current, out IValue? enumerableObject))
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
	
		if(enumerableObject.IsObject())
		{
			foreach (KeyValuePair<string, IValue> kvp in enumerableObject.AsObject())
			{
				IValue NewValue = new ArcObject(new Dictionary<string, IValue>()
				{
					{ "key", new ArcString(kvp.Key) },
					{ "value", kvp.Value }
				});
				dict[key] = new ArcPointer(ref NewValue);
	
				result.Add(Compile(codeblock));
	
				dict.Remove(key);
			}
		}
		//else if(enumerableObject.Value.TypeCode == ValueTypeCode.List)
		//{
		//	ArcList lst = (ArcList)enumerableObject.Value;
		//	foreach(ArcPointer val in lst)
		//	{
		//		dict[key] = new ArcPointer(ref val.Value);
		//
		//		result.Add(Compile(codeblock));
		//
		//		dict.Remove(key);
		//	}
		//}
	
		return i;
	}
}
