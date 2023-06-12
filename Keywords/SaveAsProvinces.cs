using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcInstance;
namespace Arc;

public partial class Compiler
{
	public Walker SaveAsProvinces(Walker i)
	{
		i = TryGetKeyValue(i, out string _, out Block? value, out bool Copy);

		if (value == null)
			throw new Exception();

		ArcObject Right;

		if (value.First == null)
			throw new Exception();

		if (value.Count == 1 && TryGetVariable(value.First.Value, out IValue? NewValue))
		{
			if (NewValue == null)
				throw new Exception();
			Right = NewValue.AsObject();
		}
		else
			Right = ArcObject.Construct(value).AsObject();

		StringBuilder loc = new StringBuilder();
		StringBuilder def = new StringBuilder();
		loc.Append("l_english:\n");
		foreach(KeyValuePair<string, IValue> province in Right.Properties)
		{
			ArcObject arcObject = province.Value.AsObject();
			int id = arcObject.Properties["id"].AsInt().Value;
			string name = arcObject.Properties["name"].AsString().Value;
			Block color = arcObject.Properties["color"].AsBlock().Value;
			Block history = arcObject.Properties["history"].AsBlock().Value;
			loc.Append($" PROV{id}: \"{name.Trim('"')}\"\n");
			def.Append($"{id};{string.Join(';',color)};;x\n");

			string result = "";
			if (history.Count > 0) result = Compile(history);

			File.WriteAllText($"{directory}/target/history/provinces/{id}.txt", result);
		}

		File.WriteAllText(directory + "/target/localisation/replace/es_provinces_l_english.yml", Parser.ConvertStringToUtf8Bom(loc.ToString()));
		File.WriteAllText(directory + "/target/map/definition.csv", def.ToString());

		return i;
	}
}
