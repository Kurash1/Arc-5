using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public partial class Compiler
{
    public Walker Require(Walker i)
    {
        i.MoveNext(); 

        i = TryGetKeyValue(i, out string key, out Block value);

        if(TryGetVariable(key, out Value? left))
        {
            if (value != null)
            {
                Value right = Value.Parse(value);
                
                if (!right.Fulfills(left))
                {
                    throw new Exception();
                }
            }
        }
        else
        {
            throw new Exception();
        }

        return i;
    }
}
