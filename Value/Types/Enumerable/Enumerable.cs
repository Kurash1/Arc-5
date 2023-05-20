﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;
public interface ArcEnumerable : Value, IEnumerable
{
    public new IEnumerator GetEnumerator();
}