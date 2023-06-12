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
		public static readonly string directory = AppDomain.CurrentDomain.BaseDirectory;
		public void Run(bool a = false)
		{
			System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en");

#if !DEBUG
			try
			{
#endif
				Defines def = new(directory + "arc.defines");
				Defines.Global = def;

				Compiler.directory = directory;
				Compiler.owner = this;

#if DEBUG
				ArcTests.Tests.VariableTest();
				//ArcTests.Tests.InheritTest();
				//ArcTests.Tests.TypeAndInterfaceTest();
				//ArcTests.Tests.RequireTest();
				//ArcTests.Tests.UncompiledTest();
				//ArcTests.Tests.ForeachTest();
				//ArcTests.Tests.BlockTest();
				//ArcTests.Tests.PreprocessorTest();
#endif
				if(def.GetTarget() == null)
				{
				
				}
				else
				{
					TranspileTarget(def);
				}
#if !DEBUG
			}
			catch(Exception e) { Console.WriteLine(e.Message); };
#endif
		}
		public static void TranspileTarget(Defines def)
		{
			string fileLocation = Path.Combine(directory, def.GetTarget());
		
			if (fileLocation.EndsWith("/"))
			{
				string[] files = Directory.GetFiles(fileLocation);
				foreach(string file in files)
				{
					Compiler comp = new();
					string fileContent = File.ReadAllText(file);
					string newFileLocation = file.Replace(".arc", ".txt");
					Transpile(comp, newFileLocation, fileContent, def);
				}
			}
			else
			{
				Compiler comp = new();
				string file = File.ReadAllText(fileLocation);
				string newFileLocation = fileLocation.Replace(".arc", ".txt");
				Transpile(comp, newFileLocation, file, def);
			}
		}
		public static void Transpile(Compiler comp, string location, string text, Defines def)
		{
			string result = comp.Compile(def.GetHeaders() + text, true, def);
			if(def.GetSave() == true)
				File.WriteAllText(location, result);
		}
	}
}
