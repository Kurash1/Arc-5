﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arc;
public interface IArcNumber : IValue
{
	public double GetNum();
	new bool IsNumber() => true;
}