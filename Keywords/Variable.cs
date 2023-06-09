﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public partial class Compiler
{
    public Walker Var<T>(Walker i, Func<Block,T> Constructor, bool move = true, string keyOverride = null) where T : Value
    {
        if(move)
            i.MoveNext(); //The previous spot is the datatype

        i = GetKeyValue(i, out string key, out Block value);

        if (!Parser.isVariableKey(key))
            throw new Exception("Invalid Variable Key");

        T Value;
        if (value.Count == 1 && TryGetVariable(value.First.Value, out Value NewValue))
        {
            Value = Constructor(NewValue.ToBlock());
        }
        else {
            Value = Constructor(value);
        }

        if (keyOverride != null)
            key = keyOverride;

        (Dictionary<string, Value> dict, string key) Val = GetNewVariable(key); //This will start as null

        Val.dict[Val.key] = Value;

        return i;
    }
}
