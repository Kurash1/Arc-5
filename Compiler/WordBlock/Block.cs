using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc;

public class Block : LinkedList<Word>
{
    public Block(string s)
    {
        AddLast(s);
    }
    public Block()
    {

    }
}