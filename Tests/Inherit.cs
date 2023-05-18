using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arc;
namespace ArcTests;
public static partial class Tests
{
    public static void InheritTest()
    {
        Compiler comp = new();

        string result = comp.compile($@"
object FirstLayer = {{
    object SecondLayer = {{
        int A = 1
        int B = 2
        int C = 3
    }}
    inherit = SecondLayer
}}
inherit = FirstLayer
A
B
C
        ").Trim();

        ArcObject expected = new ArcObject(new Dictionary<string, Value>(){
                { "SecondLayer", new ArcObject(new Dictionary<string, Value>(){
                    { "A", new ArcInt(1) },
                    { "B", new ArcInt(2) },
                    { "C", new ArcInt(3) },
                }) },
                { "A", new ArcInt(1) },
                { "B", new ArcInt(2) },
                { "C", new ArcInt(3) }
            });

        if (!((ArcObject)comp.variables["FirstLayer"]).Fulfills(expected))
            throw new Exception("Failure on Inherit Test");

        if (!((ArcInt)comp.variables["A"]).Fulfills(new ArcInt(1)))
            throw new Exception("Failure on Inherit Test");

        if (!((ArcInt)comp.variables["B"]).Fulfills(new ArcInt(2)))
            throw new Exception("Failure on Inherit Test");

        if (!((ArcInt)comp.variables["C"]).Fulfills(new ArcInt(3)))
            throw new Exception("Failure on Inherit Test");



        if (result != "1 2 3")
            throw new Exception("Failure on Inherit Test");

        Console.WriteLine("Success on Inherit Test");
    }
}