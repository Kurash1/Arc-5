using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

using ArcInstance;
using System.Text;

namespace Arc
{
    public partial class Compiler
    {
        public Dictionary<string, Value> variables = new Dictionary<string, Value>()
        {
            { "global", global }
        };
        public static ArcObject global = new ArcObject();
        public static string directory = null;
        public static Instance owner = null;
        public Compiler(string directory, Instance owner)
        {
            Compiler.directory = directory;
            Compiler.owner = owner;
        }
        public Compiler()
        {

        }
        public string compile(string file)
        {
            return Low_compile(Parser.ParseString(file));
        }
        public string Low_compile(Block code)
        {
            
            List<string> result = new();
            Dictionary<string, Func<Block.Enumerator, Block.Enumerator>> keywords = new()
            {
                { "string", (Block.Enumerator i) => Var(i, (Block s) => new ArcString(s) ) },
                { "bool", (Block.Enumerator i) => Var(i, (Block s) => new ArcBool(s) ) },
                { "float", (Block.Enumerator i) => Var(i, (Block s) => new ArcFloat(s) ) },
                { "int", (Block.Enumerator i) => Var(i, (Block s) => new ArcInt(s) ) },
                { "var", (Block.Enumerator i) => Var(i, (Block s) => Value.Parse(s) ) },
                { "object", (Block.Enumerator i) => Var(i, (Block s) => new ArcObject(s) ) },
                { "list", (Block.Enumerator i) => Var(i, (Block s) => new ArcList(s) ) },
            };

            Block.Enumerator g = code.GetEnumerator();
            g.MoveNext();
            do
            {
                if (keywords.ContainsKey(g.Current))
                {
                    g = keywords[g.Current].Invoke(g);
                    continue;
                }
                else if (TryGetVariable(g.Current, out Value var))
                {
                    if(var != null)
                    {
                        result.Add(var.ToString());
                    }
                    continue;
                }

                result.Add(g.Current);
            } while (g.MoveNext());

            StringBuilder res = new();
            foreach (string s in result)
            {
                res.Append($"{s.Trim()} ");
            }
            return res.ToString();
        }
    }
}
