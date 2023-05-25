using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Arc;

public static partial class Parser
{
    public static string Preprocessor(string input)
    {
        List<(string OldValue, string NewValue)> replaces = new();

        Regex Replace = new("/replace (\\S+) with (\\S+)", RegexOptions.Compiled);
        input = Replace.Replace(input, delegate (Match m) {
            replaces.Add((m.Groups[1].Value, m.Groups[2].Value));
            return "";
        });

        foreach ((string OldValue, string NewValue) in replaces)
        {
            input = input.Replace(OldValue, NewValue);
        }

        return input;
    }
    public static Block ParseCode(string str)
    {
        Block retval = new();
        if (string.IsNullOrWhiteSpace(str)) return retval;
        int ndx = 0;
        string s = "";
        bool insideDoubleQuote = false;
        int indent = 0;

        while (ndx < str.Length)
        {
            if ((str[ndx] == ' ' || str[ndx] == '\n' || str[ndx] == '\t') && !insideDoubleQuote && indent == 0)
            {
                if (!string.IsNullOrWhiteSpace(s.Trim())) retval.AddLast(s.Trim());
                s = "";
            }
            if (str[ndx] == '"') insideDoubleQuote = !insideDoubleQuote;
            if (str[ndx] == '(') indent++;
            if (str[ndx] == ')') indent--;
            s += str[ndx];
            ndx++;
        }
        if (!string.IsNullOrWhiteSpace(s.Trim())) retval.AddLast(s.Trim());
        return retval;
    }
    public static string FormatCode(string str)
    {
        MatchCollection matches = formatter.Matches(str);

        string[] vs = matches.Select(m => m.Value).ToArray();
        int indent = 0;
        for (int i = 0; i < vs.Length; i++)
        {
            if (vs[i].EndsWith('{'))
            {
                vs[i] = vs[i].Prepend(indent, '\t');
                indent++;
            }
            else if (vs[i].EndsWith('}'))
            {
                indent--;
                vs[i] = vs[i].Prepend(indent, '\t');
            }
            else
            {
                vs[i] = vs[i].Prepend(indent, '\t');
            }
        }

        return string.Join(Environment.NewLine, vs);
    }
}