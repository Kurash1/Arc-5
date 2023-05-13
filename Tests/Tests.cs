using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Arc;

namespace ArcTests;
public static partial class Tests
{
    public static void TypeAndInterfaceTest()
    {
        Compiler comp = new();

        string result = comp.compile($@"
type a = int
        ").Trim();

        Console.WriteLine("Success on Type And Interface Test");
    }
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
    }}
}}
FirstLayer:SecondLayer:hello
string HelloGet = FirstLayer:hello
HelloGet
string NumAsString = 121
int NumFromString = NumAsString
object global:GlobalObject = {{

}}
        ").Trim();

        ArcObject expected = new ArcObject(new Dictionary<string, Value>(){
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
                    }))}
                }) }
            });

        if (!((ArcObject)comp.variables["FirstLayer"]).Equals(expected))
            throw new Exception("Failure on Variable Test");

        if (result != "\"Wrld\" \"World\"")
            throw new Exception("Failure on Variable Test");

        Console.WriteLine("Success on Variable Test");
    }
}