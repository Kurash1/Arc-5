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

		string result = comp.Compile(@"
string a = ""baba""
object FirstLayer = {
	string hello = ""World""
	bool true = yes
	bool false = no
	float pi = 3.14
	int i = 0
	object SecondLayer = {
		string hello = ""Wrld""
		list test = [
			100 200 300
		]
		test2 = [
			100 200 300
		]
	}
}
string HelloGet = FirstLayer:hello
string NumAsString = 121
int NumFromString = NumAsString
object global:GlobalObject = {

}

FirstLayer:SecondLayer:hello = HelloGet
		").Trim();

		if (!ResultMatches(result, @"
""Wrld"" = ""World""
"))
			throw new Exception("Failure on Variable Test");

		Console.WriteLine("Success on Variable Test");
	}
}