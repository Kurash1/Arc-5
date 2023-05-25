using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Arc;
public static partial class Utils
{
	public static string Prepend(this string sa, int amount, char c)
	{
		string s = "".PadLeft(amount, c);
		return s + sa;
	}
	public static string RegRep(this string sa, string regex, string replace)
	{
		return Regex.Replace(sa, regex, replace);
	}

}
