using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public static class Defines
{
    public static string? Target;
    public static bool Formatting = false;
    public static string Headers = "";
    public static void GetDefines(string filepath)
    {
        string file = File.ReadAllText(filepath);
        ArcObject defines = new ArcObject(Parser.ParseCode(file));

        if(defines.Properties.ContainsKey("headers"))
            Headers = ((ArcString)defines["headers"]).Value.Trim('"');

        if(defines.Properties.ContainsKey("formatting"))
            Formatting = ((ArcBool)defines["formatting"]).Value;

        if(defines.Properties.ContainsKey("target"))
            Target = ((ArcString)defines["target"]).Value.Trim('"');
    }
}