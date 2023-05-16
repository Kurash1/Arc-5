using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arc;
public partial class Compiler
{
    public static Block.Enumerator GetScope(Block.Enumerator i, out Block scope)
    {
        scope = new();    
        int indent = 0;
        do
        {
            if(Parser.open.IsMatch(i.Current))
                indent++;
            if (Parser.close.IsMatch(i.Current))
                indent--;
            scope.AddLast(i.Current);
            if(indent > 0)
                i.MoveNext();
        } while (indent > 0);
        return i;
    }
    public static Block RemoveEnclosingBrackets(Block scope)
    {
        scope.RemoveFirst();
        scope.RemoveLast();
        return scope;
    }
    public static Block.Enumerator GetValue(Block.Enumerator i, out Block value)
    {
        value = new();
        if (!Parser.open.IsMatch(i.Current))
        {
            value.AddLast(i.Current);
        }
        else
        {
            i = GetScope(i, out Block scope);
            value = scope;
        }
        return i;
    }
    public Block.Enumerator GetKeyValue(Block.Enumerator i, out string key, out Block value)
    {
        key = i.Current;

        i.MoveNext();

        if (!Parser.equal.IsMatch(i.Current))
            throw new Exception($@"Expecting an equal sign between Key and Value of a variable declaration was: {i.Current}");

        i.MoveNext();

        i = GetValue(i, out value);

        return i;
    }
    public static string StringListSum(List<string> list)
    {
        return string.Join(" ", list);
    }

    public (Dictionary<string, Value> dict, string key) GetNewVariable(string locator)
    {
        if (locator.Contains(':'))
        {
            string[] KeyLocator = locator.Split(':');
            int f = 0;
            Dictionary<string, Value> currentDict = variables;
            string currentKey;
            do
            {
                currentKey = KeyLocator[f];
                if (currentDict.ContainsKey(currentKey))
                {
                    if (currentDict[currentKey].TypeCode == ValueTypeCode.Object)
                    {
                        currentDict = ((ArcObject)currentDict[currentKey]).Properties;
                    }
                    else
                    {
                        throw new Exception($"Tried assigning to a key in a nonobjectKey");
                    }
                }
                f++;
            } while (KeyLocator.Length > f);
        
            if (!currentDict.ContainsKey(currentKey))
                currentDict.Add(currentKey, null);
            return (currentDict, currentKey);
        }
        else
        {
            if (!variables.ContainsKey(locator))
                variables.Add(locator, null);
            return (variables,locator);
        }
    }
    public bool TryGetVariable(string locator, out Value? var)
    {
        if (locator.Contains(':'))
        {
            string[] KeyLocator = locator.Split(':');
            int f = 0;
            Dictionary<string, Value> currentDict = variables;
            string currentKey;
            do
            {
                currentKey = KeyLocator[f];
                if (currentDict.ContainsKey(currentKey))
                {
                    if (KeyLocator.Length > f + 1)
                    {
                        if (currentDict[currentKey].TypeCode == ValueTypeCode.Object)
                        {
                            currentDict = ((ArcObject)currentDict[currentKey]).Properties;
                        }
                    }
                }
                else
                {
                    var = null;
                    return false;
                }
                f++;
            } while (KeyLocator.Length > f);
            if (!currentDict.ContainsKey(currentKey))
            {
                var = null;
                return false;
            }
            var = currentDict[currentKey];
            return true;
        }
        else
        {
            if (!variables.ContainsKey(locator))
            {
                var = null;
                return false;
            }
            var = variables[locator];
            return true;
        }
    }
}