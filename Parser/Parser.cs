using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;

public static partial class Parser
{
    public static Block ParseString(string str)
    {
        var retval = new Block();
        if (string.IsNullOrWhiteSpace(str)) return retval;
        int ndx = 0;
        string s = "";
        bool insideDoubleQuote = false;
        int indent = 0;
        int indent2 = 0;

        while (ndx < str.Length)
        {
            if ((str[ndx] == ' ' || str[ndx] == '\n' || str[ndx] == '\t') && !insideDoubleQuote && indent == 0 && indent2 == 0)
            {
                if (!string.IsNullOrWhiteSpace(s.Trim())) retval.AddLast(s.Trim());
                s = "";
            }
            if (str[ndx] == '"') insideDoubleQuote = !insideDoubleQuote;
            if (str[ndx] == '(') indent++;
            if (str[ndx] == '[') indent2++;
            if (str[ndx] == ')') indent--;
            if (str[ndx] == ']') indent2--;
            s += str[ndx];
            ndx++;
        }
        if (!string.IsNullOrWhiteSpace(s.Trim())) retval.AddLast(s.Trim());
        return retval;
    }
}