﻿using System;
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

	public static Dictionary<string, Func<Block, IValue>> DefinesInterface = new()
	{
		{ "headers", ArcString.Construct },
		{ "target", ArcString.Construct },
		{ "formatting", ArcBool.Construct },
		{ "save", ArcBool.Construct }
	};

	public Defines()
	{

	}
	public Defines(string filepath)
	{
		string file = File.ReadAllText(filepath);
		ArcObject defines = ArcObject.Construct(
			Parser.ParseCode(file),
			DefinesInterface
		).AsObject();

		Set(defines);
	}
	public Defines(ArcObject defines) => Set(defines);
	public void Set(ArcObject defines)
	{
		if (defines.Properties.ContainsKey("headers"))
		{
			Headers = defines.Properties["headers"].AsString().Value.Trim('"');
		}

		if (defines.Properties.ContainsKey("formatting"))
		{
			Formatting = defines.Properties["formatting"].AsBool().Value;
		}
		if (defines.Properties.ContainsKey("save"))
		{
			Save = defines.Properties["save"].AsBool().Value;
		}

		if (defines.Properties.ContainsKey("target"))
		{
			Target = defines.Properties["target"].AsString().Value.Trim('"');
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