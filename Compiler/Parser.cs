using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arc;
public static partial class Parser
{
    public static Regex open = new Regex("^{$", RegexOptions.Compiled);
    public static Regex close = new Regex("^}$", RegexOptions.Compiled);
    public static Regex equal = new Regex("^=$", RegexOptions.Compiled);
    public static Regex inQuotes = new Regex("^\"(.*)\"$", RegexOptions.Compiled);
    public static Regex validVariableKey = new Regex("^[:$a-zA-Z0-9_]+$", RegexOptions.Compiled);
    public static bool isVariableKey(string s)
    {
        return validVariableKey.IsMatch(s);
    }
    public static bool HasEnclosingBrackets(Block s)
    {
        return s.ElementAt(0) == "{" && s.ElementAt(s.Count - 1) == "}";
    }
}