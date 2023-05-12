﻿using System;
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
        string directory = AppDomain.CurrentDomain.BaseDirectory;
        public void test(bool a = false)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en");

#if !DEBUG
            try
            {
#endif
            ArcTests.Tests.VariableTest(directory, this);
#if !DEBUG
            }
            catch(Exception e) { Console.WriteLine(e.Message); };
#endif
        }
    }
}