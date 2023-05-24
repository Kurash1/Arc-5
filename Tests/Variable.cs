using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Arc;

namespace ArcTests;
public static partial class Tests
{
    public static void VariableTest()
    {
        Compiler comp = new();

        string result = comp.compile($@"
object FirstLayer = {{
    string hello = ""World""
    bool true = yes
    bool false = no
    float pi = 3.14
    int i = 0
    object SecondLayer = {{
        string hello = ""Wrld""
        list test = {{
            100 200 300
        }}
        test2 = {{
            100 200 300
        }}
    }}
}}
string HelloGet = FirstLayer:hello
string NumAsString = 121
int NumFromString = NumAsString
object global:GlobalObject = {{

}}

FirstLayer:SecondLayer:hello = HelloGet
        ").Trim();

        ArcObject expected = new ArcObject(new Dictionary<string, Value>(){
                { "global", Compiler.global },
                { "hello", new ArcString("\"World\"") },
                { "true", new ArcBool(true) },
                { "false", new ArcBool(false) },
                { "pi", new ArcFloat(3.14) },
                { "i", new ArcInt(0) },
                { "SecondLayer", new ArcObject(new Dictionary<string, Value>(){
                    { "hello", new ArcString("\"Wrld\"") },
                    { "test", new ArcList(new LinkedList<Value>(new[]
                        {
                            new ArcInt(100),
                            new ArcInt(200),
                            new ArcInt(300)
                        }
                    ))},
                    { "test2", new ArcList(new LinkedList<Value>(new[]
                        {
                            new ArcInt(100),
                            new ArcInt(200),
                            new ArcInt(300)
                        }
                    ))}
                }) }
            });

        if (!((ArcObject)comp.variables["FirstLayer"]).Fulfills(expected))
            throw new Exception("Failure on Variable Test");

        if (!ResultMatches(result, @"
""Wrld"" = ""World""
"))
            throw new Exception("Failure on Variable Test");

        Console.WriteLine("Success on Variable Test");
    }
}