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

        (Dictionary<string, Value> dict, string key) variable = GetNewVariable(i.Current);

        i.MoveNext();

        if (i.Current != "in")
            throw new Exception();

        i.MoveNext();

        Value? enumerableObject;
        if (!TryGetVariable(i.Current, out enumerableObject))
            throw new Exception();
        if(enumerableObject == null)
            throw new Exception();

        i.MoveNext();

        if(!Parser.equal.IsMatch(i.Current))
            throw new Exception();

        i.MoveNext();

        Block codeblock;
        GetValue(i, out codeblock);

        if(Parser.HasEnclosingBrackets(codeblock))
            RemoveEnclosingBrackets(codeblock);

        if(enumerableObject.TypeCode == ValueTypeCode.Object)
        {
            foreach (KeyValuePair<string, Value> kvp in (ArcObject)enumerableObject)
            {
                variable.dict[variable.key] = new ArcObject(new Dictionary<string, Value>()
                {
                    { "key", new ArcString(kvp.Key) },
                    { "value", kvp.Value }
                });

                result.Add(compile(codeblock));

                variable.dict.Remove(variable.key);
            }
        }
        else if(enumerableObject.TypeCode == ValueTypeCode.List)
        {
            foreach (Value value in (ArcList)enumerableObject)
            {
                variable.dict[variable.key] = value;

                result.Add(compile(codeblock));

                variable.dict.Remove(variable.key);
            }
        }

        return i;
    }
}
