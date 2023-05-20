using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arc;
namespace ArcTests;
public static partial class Tests
{
    public static void ForeachTest()
    {
        Compiler comp = new();

        string result = comp.compile($@"
list args = {{
    kazakh
    khalkha
    korchin
}}
OR = {{
    foreach $culture in args = {{
        `primary_culture` = $culture
    }}
}}
        ").Trim();

        string expected = @"
OR = {
	primary_culture = kazakh
	primary_culture = khalkha
	primary_culture = korchin
}
".Trim();
        if (result != expected)
            throw new Exception();

        Console.WriteLine("Success on Foreach Test");
    }
}