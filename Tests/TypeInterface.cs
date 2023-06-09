﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arc;
namespace ArcTests;
public static partial class Tests
{
    public static void TypeAndInterfaceTest()
    {
        Compiler comp = new();

        string result = comp.compile($@"
interface country = {{
  type name = string
  type adj = string
  type color = list
}}
country ade = {{
  string name = ""Adelea""
  string adj = ""Adelean""
  list color = {{ 100 100 100 }}
}}


type a = bool
type b = int
type c = float
type d = string
type e = object
type f = list
type g = type
type h = block
a a2 = yes
b b2 = 10
c c2 = 10.1
d d2 = ""Hello""
e e2 = {{
    string test = ""Hello""
}}
f f2 = {{
    test test test
}}

g g2 = bool

h h2 = {{
    test test test
}}
        ").Trim();

        Console.WriteLine("Success on Type And Interface Test");
    }
}