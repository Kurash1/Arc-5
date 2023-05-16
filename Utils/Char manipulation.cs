using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public static partial class Utisl
{
    public static char ToUpper(this char left)
    {
        if (char.IsLower(left))
        {
            // Use the built-in method char.ToUpper to convert the character to uppercase
            return char.ToUpper(left);
        }

        // If the character is already uppercase or not a letter, return it as is
        return left;
    }
}
