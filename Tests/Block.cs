using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Arc;

namespace ArcTests;
public static partial class Tests
{
    public static void BlockTest()
    {
        Compiler comp = new();

        string result = comp.compile(@"
block primary_culture = {
    `primary_culture` = args
}
primary_culture = finnish
        ").Trim();

        if (result != @"
primary_culture = finnish
".Trim())
            throw new Exception("Failure on Block Test");

        Console.WriteLine("Success on Variable Test");
    }
}