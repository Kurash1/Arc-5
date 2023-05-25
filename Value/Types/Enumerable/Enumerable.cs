﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;
public interface IArcEnumerable : IValue, IEnumerable
{
    public new IEnumerator GetEnumerator();
}