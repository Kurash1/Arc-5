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
			Right = ArcObject.Construct(value, null).AsObject();

		StringBuilder loc = new();
		StringBuilder def = new();
		StringBuilder imp = new();
		StringBuilder sea = new();
		StringBuilder lake = new();
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

			if(arcObject.Properties.ContainsKey("impassible"))
			{
				bool impassible = arcObject.Properties["impassible"].AsBool().Value;

				if (impassible)
				{
					imp.Append($"{id} ");
				}
			}
			if(arcObject.Properties.ContainsKey("sea"))
			{
				bool issea = arcObject.Properties["sea"].AsBool().Value;

				if (issea)
				{
					sea.Append($"{id} ");
				}
			}
			if(arcObject.Properties.ContainsKey("lake"))
			{
				bool islake = arcObject.Properties["lake"].AsBool().Value;

				if (islake)
				{
					lake.Append($"{id} ");
				}
			}

			string result = "";
			if (history.Count > 0) result = Compile(history);

			File.WriteAllText($"{directory}/target/history/provinces/{id} - ARC.txt", result);
		}

		File.WriteAllText(directory + "/target/localisation/replace/es_provinces_l_english.yml", Parser.ConvertStringToUtf8Bom(loc.ToString()));
		File.WriteAllText(directory + "/target/map/definition.csv", def.ToString());
		File.WriteAllText(directory + "/target/map/climate.txt", $@"
tropical = {{
    
}}

arid = {{
    
}}

arctic = {{
	
}}

mild_winter = {{
    
}}


normal_winter = {{
    
}}

severe_winter = {{
    
}}

impassable = {{
	{imp}
}}

mild_monsoon = {{
    
}}

normal_monsoon = {{
    
}}

severe_monsoon = {{
    
}}

equator_y_on_province_image = 224");
		File.WriteAllText(directory + "/target/map/default.map", $@"
width = 4096
height = 2816

max_provinces = {Right.Properties.Count+1}
sea_starts = {{ 
	{sea}
}}

only_used_for_random = {{
}}

lakes = {{
	{lake}
}}

force_coastal = {{
}}

definitions = ""definition.csv""
provinces = ""provinces.bmp""
positions = ""positions.txt""
terrain = ""terrain.bmp""
rivers = ""rivers.bmp""
terrain_definition = ""terrain.txt""
heightmap = ""heightmap.bmp""
tree_definition = ""trees.bmp""
continent = ""continent.txt""
adjacencies = ""adjacencies.csv""
climate = ""climate.txt""
region = ""region.txt""
superregion = ""superregion.txt""
area = ""area.txt""
provincegroup = ""provincegroup.txt""
ambient_object = ""ambient_object.txt""
seasons = ""seasons.txt""
trade_winds = ""trade_winds.txt""

# Define which indices in trees.bmp palette which should count as trees for automatic terrain assignment
tree = {{ 3 4 7 10 }}
");

		return i;
	}
}
