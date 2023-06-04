using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class Defines
{
	public static Defines Global = new();
	private string? Target;
	private bool? Formatting;
	private bool? Save;
	private string? Headers;
	public Defines()
	{

	}
	public Defines(string filepath)
	{
		string file = File.ReadAllText(filepath);
		ArcObject defines = new(Parser.ParseCode(file));

		Set(defines);
	}
	public Defines(ArcObject defines) => Set(defines);
	public void Set(ArcObject defines)
	{
		if (defines.Properties.ContainsKey("headers"))
		{
			Headers = ((ArcString)defines["headers"].Value).Value.Trim('"');
		}

		if (defines.Properties.ContainsKey("formatting"))
		{
			Formatting = ((ArcBool)defines["formatting"].Value).Value;
		}
		if (defines.Properties.ContainsKey("save"))
		{
			Save = ((ArcBool)defines["save"].Value).Value;
		}

		if (defines.Properties.ContainsKey("target"))
		{
			Target = ((ArcString)defines["target"].Value).Value.Trim('"');
		}
	}
	public bool GetFormatting()
	{
		if (Formatting != null)
			return (bool)Formatting;
		if (Global == null)
			throw new Exception();
		if(Global.Formatting != null)
			return (bool)Global.Formatting;
		throw new Exception();
	}
	public bool GetSave()
	{
		if (Save != null)
			return (bool)Save;
		if (Global == null)
			throw new Exception();
		if(Global.Save != null)
			return (bool)Global.Save;
		throw new Exception();
	}
	public string GetHeaders()
	{
		if (Headers != null)
			return Headers;
		if (Global == null)
			throw new Exception();
		if(Global.Headers != null)
			return Global.Headers;
		throw new Exception();
	}
	public string GetTarget()
	{
		if (Target != null)
			return Target;
		if (Global == null)
			throw new Exception();
		if(Global.Target != null)
			return Global.Target;
		throw new Exception();
	}
}