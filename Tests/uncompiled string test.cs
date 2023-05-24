using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arc;
namespace ArcTests;
public static partial class Tests
{
    public static void UncompiledTest()
    {
        Compiler comp = new();

        string result = comp.compile($@"
block add_treasury = {{
    require args = int
    `add_treasury` = args
}}
`add_treasury` = a
        ").Trim();

        Console.WriteLine("Success on Uncompiled Text Test");
    }
}