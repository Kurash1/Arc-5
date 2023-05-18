using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arc;
namespace ArcTests;
public static partial class Tests
{
    public static void RequireTest()
    {
        Compiler comp = new();
            string result = comp.compile($@"
int a = 10
int c = 20

require a
require b = int
require c = 20 
        ").Trim();
    }
}