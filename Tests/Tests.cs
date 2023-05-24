using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arc;
namespace ArcTests;
public static partial class Tests
{
    static bool ResultMatches(string result, string expected)
    {
        result = result.RegRep("\\s+", " ").Trim();
        expected = expected.RegRep("\\s+", " ").Trim();
        return result == expected;
    }
}