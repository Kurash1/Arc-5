using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data;
using System.Text;

using Arc;

namespace ArcInstance
{
	public class Instance
	{
		readonly string directory = AppDomain.CurrentDomain.BaseDirectory;
		public void Run(bool a = false)
		{
			System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en");

#if !DEBUG
			try
			{
#endif
				Defines.GetDefines(directory + "arc.defines");

				Compiler.directory = directory;
				Compiler.owner = this;

#if DEBUG
				ArcTests.Tests.VariableTest();
				ArcTests.Tests.InheritTest();
				ArcTests.Tests.TypeAndInterfaceTest();
				ArcTests.Tests.RequireTest();
				ArcTests.Tests.UncompiledTest();
				ArcTests.Tests.ForeachTest();
				ArcTests.Tests.BlockTest();
				ArcTests.Tests.PreprocessorTest();
#endif
				//if(Defines.Target == null)
				//{
				//
				//}
				//else
				//{
				//	string fileLocation = Path.Combine(directory, Defines.Target);
				//	string file = File.ReadAllText(fileLocation);
				//	string newFileLocation = fileLocation.Replace(".arc", ".txt");
				//	Transpile(newFileLocation, file);
				//}
				//
				//void Transpile(string location, string text)
				//{
				//	Compiler comp = new();
				//	string result = comp.compile(Defines.Headers + text, true);
				//	File.WriteAllText(location, result);
				//}
#if !DEBUG
			}
			catch(Exception e) { Console.WriteLine(e.Message); };
#endif
		}
	}
}
