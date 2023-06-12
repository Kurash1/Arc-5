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
float a = 10.25
int b = 52
bool c = yes
string d = ""Hello""
block e = {
	aba baba
}

object f = {
	float a = 10.25
	int b = 52
	bool c = yes
	string d = ""Hello""
	block e = {
		aba baba
	}
}

f g = {
	a = 5
}

string global:hello = ""Hello""

object aba = {
	string s = global:hello
}
object bebe = {
	string s = global:hello
}
		").Trim();

		Console.WriteLine("Success on Variable Test");
	}
}